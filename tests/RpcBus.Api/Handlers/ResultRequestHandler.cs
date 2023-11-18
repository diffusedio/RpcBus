using RpcBus.Test.Contract;


namespace RpcBus.Test.Api.Handlers;

public class ResultRequestHandler 
{
    public async Task<Result<Dictionary<string, string>>> Handle(ResultRequest request)
    {
        await Task.Delay(1);

        if (request.ShouldThrowError)
        {
            return new Result<Dictionary<string, string>>(new Exception("some error"));
        }

        return new Dictionary<string, string>
        {
            { request.Name, request.Value }
        };
    }
}
