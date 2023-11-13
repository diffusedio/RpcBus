using Microsoft.EntityFrameworkCore;
using RpcBus.Test.Contract.Models;

namespace RpcBus.Test.Api.Data;

public class TodoContext : DbContext
{
    public DbSet<TodoModel> Todos { get; set; }

    public TodoContext(DbContextOptions options) : base(options) { }
}