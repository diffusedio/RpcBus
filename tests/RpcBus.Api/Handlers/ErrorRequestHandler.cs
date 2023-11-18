using RpcBus.Test.Contract;

namespace RpcBus.Test.Api.Handlers;

public class ErrorRequestHandler 
{
    
    public Task<string> Handle(ErrorRequest request)
    {
        throw new Exception(request.Message);
    }
}