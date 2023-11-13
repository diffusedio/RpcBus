using MediatR;
using RpcBus.Exceptions;
using RpcBus.Test.Api.Data;
using RpcBus.Test.Contract;

namespace RpcBus.Test.Api.Handlers;

public class DeleteTodoRequestHandler : IRequestHandler<DeleteTodoRequest>
{
    private readonly TodoContext context;

    public DeleteTodoRequestHandler(TodoContext context)
    {
        this.context = context;
    }

    public async Task Handle(DeleteTodoRequest request, CancellationToken cancellationToken)
    {
        var entity = await context.Todos.FindAsync(new object?[] { request.Id }, cancellationToken);

        if (entity == null)
            throw new JRpcNotFoundException("Todo not found");

        context.Todos.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        
    }
}
