using MediatR;
using Microsoft.EntityFrameworkCore;
using RpcBus.Test.Api.Data;
using RpcBus.Test.Contract;
using RpcBus.Test.Contract.Models;

namespace RpcBus.Test.Api.Handlers;

public class QueryTodosRequestHandler : IRequestHandler<QueryTodosRequest, TodoModel[]>
{
    private readonly TodoContext context;

    public QueryTodosRequestHandler(TodoContext context)
    {
        this.context = context;
    }

    public async Task<TodoModel[]> Handle(QueryTodosRequest request, CancellationToken cancellationToken)
    {
        return await context
            .Todos
            .AsNoTracking()
            .Skip(request.Skip)
            .Take(request.Take)
            .ToArrayAsync(cancellationToken);
    }
}
