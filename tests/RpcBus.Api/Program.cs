using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RpcBus.Server;
using RpcBus.Test.Api.Data;
using RpcBus.Test.Api.Handlers;
using RpcBus.Test.Contract;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);


// add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "me",
            ValidAudience = "me",
            IssuerSigningKey = LoginRequestHandler.GetTokenSigningKey()
        };
    });

// add authorization
builder.Services.AddAuthorization();

// add db context
builder.Services.AddDbContext<TodoContext>(options => options.UseInMemoryDatabase("local"));


var assemblies = new[] { typeof(CreateTodoRequest).Assembly, typeof(CreateTodoRequestHandler).Assembly };


builder.Host.UseWolverine();

//// add SlimMessageBus
//builder.Services.AddSlimMessageBus(mbb =>
//{
//    mbb
//        .WithProviderMemory()
//        .AutoDeclareFrom(assemblies);

//    foreach (var ass in assemblies)
//    {
//        mbb.AddServicesFromAssembly(ass);
//    }
//});

// add jrpc mediator
builder.Services
    .AddJRpcMediator(assemblies, options =>
    {
        options.Route = "/execute";
        options.JsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseJRpcMediator();

app.Run();