using RpcBus.Client;
using RpcBus.Test.Contract.Models;

namespace RpcBus.Test.Contract
{
    [JRpcMethod("get/todo")]
    [JRpcAuthorize(Role = "reader")]
    public class GetTodoRequest : IRequest<TodoModel>
    {
        public int Id { get; set; }

        public GetTodoRequest(int id)
        {
            Id = id;
        }
    }
}