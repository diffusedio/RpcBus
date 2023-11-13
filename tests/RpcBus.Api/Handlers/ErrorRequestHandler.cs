using MediatR;
using RpcBus.Test.Contract;

namespace RpcBus.Test.Api.Handlers;

public class ErrorRequestHandler : IRequestHandler<ErrorRequest, string>
{
    public Task<string> Handle(ErrorRequest request, CancellationToken cancellationToken)
    {
        throw new Exception(request.Message);
    }
}