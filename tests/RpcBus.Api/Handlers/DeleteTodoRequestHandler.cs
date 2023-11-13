using RpcBus.Exceptions;
using RpcBus.Test.Api.Data;
using RpcBus.Test.Contract;
using SlimMessageBus;

namespace RpcBus.Test.Api.Handlers;

public class DeleteTodoRequestHandler : IRequestHandler<DeleteTodoRequest>
{
    private readonly TodoContext context;

    public DeleteTodoRequestHandler(TodoContext context)
    {
        this.context = context;
    }

    public async Task OnHandle(DeleteTodoRequest request)
    {
        var entity = await context.Todos.FindAsync(new object?[] { request.Id });

        if (entity == null)
            throw new JRpcNotFoundException("Todo not found");

        context.Todos.Remove(entity);

        await context.SaveChangesAsync();

        
    }
}
