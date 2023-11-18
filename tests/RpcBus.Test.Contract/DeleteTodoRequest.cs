namespace RpcBus.Test.Contract
{
    [JRpcMethod("delete/todo")]
    [JRpcAuthorize(Role = "writer")]
    public class DeleteTodoRequest 
    {
        public int Id { get; set; }

        public DeleteTodoRequest(int id)
        {
            Id = id;
        }
    }
}