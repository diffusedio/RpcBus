using System.Text.Json;

namespace RpcBus.Server;

public class JRpcServerOptions
{
    public string Route { get; set; } = "/jrpc";

    public JsonSerializerOptions JsonOptions { get; set; } = new();
}