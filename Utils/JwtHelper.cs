using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace dotnet6_webapi.Utils;

public class JwtHelper
{
    private static IConfiguration? configuration;

    public static void SetConfiguration(IConfiguration _configuration)
    {
        configuration = _configuration;
    }

    private static IConfiguration GetConfiguration()
    {
        if (configuration == null)
        {
            throw new InvalidOperationException("Missing Builder Configuration In JwtHelper !");
        }
        return configuration;
    }
    public static void Init(JwtBearerOptions options)
    {
        var JwtSetting = JwtHelper.GetConfiguration();
        string SecretKey = JwtSetting["SecretKey"] ?? "";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = JwtSetting["Issuer"],
            ValidAudience = JwtSetting["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
        };
    }

    public static string GenerateToken(IConfiguration configuration, string username)
    {
        var claims = new List<Claim>
            {
                new(ClaimTypes.Name, username),
                // You can add more claims as needed
            };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1), // Set expiration time as needed
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenDescriptor);
    }
}
