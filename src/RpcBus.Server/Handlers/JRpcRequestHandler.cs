using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RpcBus.Exceptions;
using RpcBus.Models;

using Wolverine;

namespace RpcBus.Server.Handlers;

public class JRpcRequestHandler
{
    private readonly JRpcServerOptions options;
    private readonly IMessageBus bus ;
    private readonly JRpcAuthorizationHandler authorization;
    private readonly JRpcAuthenticationHandler authentication;

    public JRpcRequestHandler(IOptionsSnapshot<JRpcServerOptions> options, IMessageBus bus, JRpcAuthorizationHandler authorization, JRpcAuthenticationHandler authentication)
    {
        this.options = options.Value;
        this.bus = bus;
        this.authorization = authorization;
        this.authentication = authentication;
    }

    public async Task<JRpcResponse> Handle(HttpContext context, JRpcRequest rpcRequest)
    {
        // create success response
        var success = (object? value) =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            // serialize result
            var responseBody = JsonSerializer.SerializeToElement(value, options.JsonOptions);

            // write response
            return JRpcResponse.Success(rpcRequest.Id!.Value, responseBody);
        };
        // create failure response
        var failure = (Exception e) =>
        {
            context.Response.StatusCode = e switch
            {
                JRpcNotFoundException => (int)HttpStatusCode.NotFound,
                JRpcUnauthorizedException => (int)HttpStatusCode.Unauthorized,
                JRpcBadRequestException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };
            
            return JRpcResponse.Failure(rpcRequest.Id!.Value, e);
        };

        try
        {
            // get request type for method
            if (!JRpcMethods.Instance.TryGetValue(rpcRequest.Method, out var requestType))
            {
                return failure(new JRpcNotFoundException("Method not found"));
            }

            // authenticate
            if (!await authentication.Handle(context, requestType))
            {
                return failure(new JRpcUnauthorizedException("Unauthorized"));
            }

            // authorize
            if (!await authorization.Handle(context, requestType))
            {
                return failure(new JRpcUnauthorizedException("Forbidden"));
            }

            // deserialize params to request
            var request =rpcRequest.Params.Deserialize(requestType, options.JsonOptions) ;

            if (request is null)
            {
                return failure(new JRpcBadRequestException("Invalid Request"));
            }

            // send request
            var response = await bus.InvokeAsync<object>(request);

            // check if request is a Result Type
            if (response is IResult result)
            {
                if (result.IsSuccess)
                {
                    return success(result.Value);
                }
                else 
                {
                    return failure(result.Exception!);
                }
            }
            else
            {
                return success(response);
            }
        }
        // handle exceptions
        catch (Exception e)
        {
            return failure(e);
        }
    }
}
