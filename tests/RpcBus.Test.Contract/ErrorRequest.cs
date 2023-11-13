using SlimMessageBus;

namespace RpcBus.Test.Contract
{
    [JRpcMethod("error")]
    public class ErrorRequest : IRequest<string>
    {
        public ErrorRequest(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}