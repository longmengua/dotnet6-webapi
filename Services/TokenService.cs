
using System.Text;

namespace dotnet6_webapi.Services;

public class TokenService
{
    private readonly ILogger<TokenService> _logger;
    public TokenService(ILogger<TokenService> logger)
    {
        _logger = logger;
    }

    public static string GenerateToken(string username)
    {
        var key = Encoding.UTF8.GetBytes("your-256-bit-secret");

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = "yourapp.com",
            Audience = "yourapp.com",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}