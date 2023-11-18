using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RpcBus.Exceptions;
using RpcBus.Models;
using Wolverine;

namespace RpcBus.Server.Handlers;

public class JRpcNotificationHandler
{
    private readonly JRpcServerOptions options;
    private readonly IMessageBus bus;
    private readonly JRpcAuthorizationHandler authorization;
    private readonly JRpcAuthenticationHandler authentication;

    public JRpcNotificationHandler(IOptionsSnapshot<JRpcServerOptions> options, IMessageBus bus, JRpcAuthorizationHandler authorization, JRpcAuthenticationHandler authentication)
    {
        this.options = options.Value;
        this.bus = bus;
        this.authorization = authorization;
        this.authentication = authentication;
    }

    public async Task Handle(HttpContext context, JRpcRequest rpcRequest)
    {
        try
        {
            // get request type for method
            if (!JRpcMethods.Instance.TryGetValue(rpcRequest.Method, out var requestType))
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            // authenticate
            if (!await authentication.Handle(context, requestType))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            // authorize
            if (!await authorization.Handle(context, requestType))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            // deserialize params to request
            var notification = rpcRequest.Params.Deserialize(requestType, options.JsonOptions);

            if (notification == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            // publish notification
            await bus.PublishAsync(notification);
            context.Response.StatusCode = (int)HttpStatusCode.NoContent;
        }
        // handle exceptions
        catch (JRpcBadRequestException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        catch (JRpcNotFoundException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        catch (JRpcUnauthorizedException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        catch (Exception e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Console.Error.WriteLine("@{0}: {1}\n{2}", e.GetType().Name, e.Message, e.StackTrace);
        }
    }
}