using Microsoft.EntityFrameworkCore;
using RpcBus.Test.Api.Data;
using RpcBus.Test.Contract;
using RpcBus.Test.Contract.Models;
using SlimMessageBus;

namespace RpcBus.Test.Api.Handlers;

public class QueryTodosRequestHandler : IRequestHandler<QueryTodosRequest, TodoModel[]>
{
    private readonly TodoContext context;

    public QueryTodosRequestHandler(TodoContext context)
    {
        this.context = context;
    }

    public async Task<TodoModel[]> OnHandle(QueryTodosRequest request)
    {
        return await context
            .Todos
            .AsNoTracking()
            .Skip(request.Skip)
            .Take(request.Take)
            .ToArrayAsync();
    }
}
