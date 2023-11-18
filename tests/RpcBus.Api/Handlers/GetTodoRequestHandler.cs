using RpcBus.Exceptions;
using RpcBus.Test.Api.Data;
using RpcBus.Test.Contract;
using RpcBus.Test.Contract.Models;

using System.Threading;

namespace RpcBus.Test.Api.Handlers;

public class GetTodoRequestHandler {

    private readonly TodoContext context;

    public GetTodoRequestHandler(TodoContext context)
    {
        this.context = context;
    }



    public async Task<TodoModel> OnHandle(GetTodoRequest request)
    {
        var entity = await context.Todos.FindAsync(new object?[] { request.Id });

        if (entity == null)
            throw new JRpcNotFoundException("Todo not found");

        return entity;
    }
}