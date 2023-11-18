using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RpcBus.Exceptions;
using RpcBus.Test.Api.Models;
using RpcBus.Test.Contract;

namespace RpcBus.Test.Api.Handlers;

public class LoginRequestHandler 
{
    private const string secret = "super secret sentence";

    private readonly static User[] users = new[]
    {
        new User("user", "1234", "reader"),
        new User("admin", "root", "reader", "writer"),
    };

    public static SymmetricSecurityKey GetTokenSigningKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret sentence super secret sentence"));

    public Task<string> Handle(LoginRequest request)
    {
        var user = users.FirstOrDefault(x => x.Name == request.Username && x.Password == request.Password);

        if (user is null)
            throw new JRpcUnauthorizedException("unauthorized");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, request.Username),
        };
        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(GetTokenSigningKey(), SecurityAlgorithms.HmacSha256),
            Audience = "me",
            Issuer = "me"
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        // Create Token
        var token = tokenHandler.CreateToken(tokenDescriptor);
        // Convert Token to string
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}