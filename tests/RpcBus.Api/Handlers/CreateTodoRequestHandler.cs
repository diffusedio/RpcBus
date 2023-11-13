using RpcBus.Test.Api.Data;
using RpcBus.Test.Contract;
using RpcBus.Test.Contract.Models;
using SlimMessageBus;
using System.Threading;

namespace RpcBus.Test.Api.Handlers;

public class CreateTodoRequestHandler : IRequestHandler<CreateTodoRequest, TodoModel>
{
    private readonly TodoContext context;

    public CreateTodoRequestHandler(TodoContext context)
    {
        this.context = context;
    }

    public async Task<TodoModel> OnHandle(CreateTodoRequest request)
    {

        var entity = new TodoModel
        {
            Name = request.Name,
            Description = request.Description
        };

        context.Todos.Add(entity);

        await context.SaveChangesAsync();

        return entity;
    }
}
