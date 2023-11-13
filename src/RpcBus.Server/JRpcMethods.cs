﻿using System.Collections.Concurrent;

namespace RpcBus.Server;

public class JRpcMethods : ConcurrentDictionary<string, Type>
{
    private static readonly JRpcMethods instance = new JRpcMethods();
    public static JRpcMethods Instance => instance;
}
