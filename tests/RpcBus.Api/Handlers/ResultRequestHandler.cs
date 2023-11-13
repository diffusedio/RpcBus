using RpcBus.Test.Contract;
using SlimMessageBus;

namespace RpcBus.Test.Api.Handlers;

public class ResultRequestHandler : IRequestHandler<ResultRequest, Result<Dictionary<string, string>>>
{
    public async Task<Result<Dictionary<string, string>>> OnHandle(ResultRequest request)
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
