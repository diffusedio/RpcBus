using RpcBus.Test.Contract;
using SlimMessageBus;

namespace RpcBus.Test.Api.Handlers;

public class ErrorRequestHandler : IRequestHandler<ErrorRequest, string>
{
    
    public Task<string> OnHandle(ErrorRequest request)
    {
        throw new Exception(request.Message);
    }
}