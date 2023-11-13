using MediatR;
using RpcBus.Test.Api.Data;
using RpcBus.Test.Contract;
using RpcBus.Test.Contract.Models;

namespace RpcBus.Test.Api.Handlers;

public class CreateTodoRequestHandler : IRequestHandler<CreateTodoRequest, TodoModel>
{
    private readonly TodoContext context;

    public CreateTodoRequestHandler(TodoContext context)
    {
        this.context = context;
    }

    public async Task<TodoModel> Handle(CreateTodoRequest request, CancellationToken cancellationToken)
    {
        var entity = new TodoModel
        {
            Name = request.Name,
            Description = request.Description
        };

        context.Todos.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
