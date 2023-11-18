using RpcBus.Client;
using RpcBus.Test.Contract.Models;

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