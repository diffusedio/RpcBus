using MediatR;

namespace RpcBus.Test.Contract
{
    [JRpcMethod("delete/todo")]
    [JRpcAuthorize(Role = "writer")]
    public class DeleteTodoRequest : IRequest
    {
        public int Id { get; set; }

        public DeleteTodoRequest(int id)
        {
            Id = id;
        }
    }
}