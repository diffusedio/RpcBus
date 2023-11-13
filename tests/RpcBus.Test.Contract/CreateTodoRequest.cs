using RpcBus.Test.Contract.Models;
using SlimMessageBus;

namespace RpcBus.Test.Contract
{
    [JRpcMethod("create/todo")]
    [JRpcAuthorize(Role = "writer")]
    public class CreateTodoRequest : IRequest<TodoModel>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public CreateTodoRequest(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}