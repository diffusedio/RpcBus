using RpcBus.Test.Contract.Models;
using SlimMessageBus;

namespace RpcBus.Test.Contract
{
    [JRpcMethod("update/todo")]
    [JRpcAuthorize(Role = "writer")]
    public class UpdateTodoRequest : IRequest<TodoModel>
    {
        public TodoModel Model { get; set; }

        public UpdateTodoRequest(TodoModel model)
        {
            Model = model;
        }
    }
}