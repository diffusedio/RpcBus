using RpcBus.Exceptions;
using RpcBus.Test.Api.Data;
using RpcBus.Test.Contract;


namespace RpcBus.Test.Api.Handlers;

public class DeleteTodoRequestHandler
{
    private readonly TodoContext context;

    public DeleteTodoRequestHandler(TodoContext context)
    {
        this.context = context;
    }

    public async Task Handle(DeleteTodoRequest request)
    {
        var entity = await context.Todos.FindAsync(new object?[] { request.Id });

        if (entity == null)
            throw new JRpcNotFoundException("Todo not found");

        context.Todos.Remove(entity);

        await context.SaveChangesAsync();

        
    }
}
