using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace dotnet6_webapi.Utils;

public class JwtHelper
{
    private static string? secretKey;
    private static string? issuer;
    private static string? audience;

    public static void SetConfiguration(string? _secretKey, string? _issuer, string? _audience)
    {
        secretKey = _secretKey;
        issuer = _issuer;
        audience = _audience;
    }
    public static void Init(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    }

    public static string GenerateToken(string account)
    {
        Log.Information("GenerateToken - 1: {secretKey}", secretKey);
        var claims = new List<Claim>
            {
                new(ClaimTypes.Name, account),
                // You can add more claims as needed
            };

        Log.Information("GenerateToken - 2: {account}, {secretKey}", account, secretKey);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1), // Set expiration time as needed
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenDescriptor);
    }
}
