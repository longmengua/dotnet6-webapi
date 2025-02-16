using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace dotnet6_webapi.Utils;

/// <summary>
/// 
/// </summary>
public class AuthHelper
{
    private static string? secretKey;
    private static string? issuer;
    private static string? audience;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_secretKey"></param>
    /// <param name="_issuer"></param>
    /// <param name="_audience"></param>
    public static void SetConfiguration(string? _secretKey, string? _issuer, string? _audience)
    {
        secretKey = _secretKey;
        issuer = _issuer;
        audience = _audience;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void Init(JwtBearerOptions options)
    {
        if (string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
        {
            Log.Error("JWT setting is not set properly. System cannot initialize JWT authentication.");
            throw new InvalidOperationException("JWT setting is not set properly. System cannot initialize JWT authentication.");
        }
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

    /// <summary>
    /// todo: expiryHours
    /// </summary>
    /// <param name="account"></param>
    /// <param name="expiryHours"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GenerateToken(string account, int? expiryHours)
    {
        if (string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
        {
            Log.Error("Attempted to generate a JWT token but failed");
            throw new InvalidOperationException("Attempted to generate a JWT token but failed");
        }

        var claims = new List<Claim>
            {
                new(ClaimTypes.Name, account),
                // You can add more claims as needed
            };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(expiryHours ?? 1), // Set expiration time as needed
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenDescriptor);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="token"></param>
    /// <param name="principal"></param>
    /// <returns></returns>
    public static bool VerifyToken(string token, out ClaimsPrincipal? principal)
    {
        principal = null;
        if (string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
        {
            Log.Error("Attempted to verify a JWT token but JWT settings are not properly configured.");
            return false;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey);

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero // Reduce token expiration delay
            };

            principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error($"JWT validation failed: {ex.Message}");
            return false;
        }
    }
}
