using MediatR;
using RpcBus.Exceptions;
using RpcBus.Test.Api.Data;
using RpcBus.Test.Contract;
using RpcBus.Test.Contract.Models;

namespace RpcBus.Test.Api.Handlers;

public class GetTodoRequestHandler : IRequestHandler<GetTodoRequest, TodoModel>
{
    private readonly TodoContext context;

    public GetTodoRequestHandler(TodoContext context)
    {
        this.context = context;
    }

    public async Task<TodoModel> Handle(GetTodoRequest request, CancellationToken cancellationToken)
    {
        var entity = await context.Todos.FindAsync(new object?[] { request.Id }, cancellationToken);

        if (entity == null)
            throw new JRpcNotFoundException("Todo not found");

        return entity;
    }
}