using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using RpcBus.Server;
using RpcBus.Test.Api.Data;
using RpcBus.Test.Api.Handlers;
using RpcBus.Test.Contract;

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
            IssuerSigningKey = LoginRequestHandler.GetTokenSigningKey(),
        };
    });

// add authorization
builder.Services.AddAuthorization();

// add db context
builder.Services.AddDbContext<TodoContext>(options => options.UseInMemoryDatabase("local"));

// add jrpc mediator
builder.Services
    .AddJRpcMediator(new[] { typeof(CreateTodoRequest).Assembly, typeof(CreateTodoRequestHandler).Assembly }, options =>
    {
        options.Route = "/execute";
        options.JsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddHttpContextAccessor();

//builder.Services.AddSpaStaticFiles(cfg => cfg.RootPath = "wwwroot");

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
//app.UseSpaStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseJRpcMediator();
//app.UseJRpcDashboard();

//app.UseSpa(spa =>
//{
//    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000/");
//});


app.Run();


app.Run();
