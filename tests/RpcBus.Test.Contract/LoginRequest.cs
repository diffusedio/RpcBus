﻿using RpcBus.Client;

namespace RpcBus.Test.Contract
{
    [JRpcMethod("login")]
    public class LoginRequest : IRequest<string>
    {
        public LoginRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }


}