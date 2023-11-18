using Microsoft.EntityFrameworkCore;
using RpcBus.Exceptions;
using RpcBus.Test.Api.Data;
using RpcBus.Test.Contract;
using RpcBus.Test.Contract.Models;


namespace RpcBus.Test.Api.Handlers;

public class UpdateTodoRequestHandler 
{
    private readonly TodoContext context;

    public UpdateTodoRequestHandler(TodoContext context)
    {
        this.context = context;
    }

    public async Task<TodoModel> Handle(UpdateTodoRequest request)
    {
        if (await context.Todos.AnyAsync(x => x.Id == request.Model.Id) is false)
            throw new JRpcNotFoundException("Todo not found");

        context.Todos.Update(request.Model);

        await context.SaveChangesAsync();

        return request.Model;
    }
}
