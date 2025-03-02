using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace dotnet6_webapi.Utils;

/// <summary>
/// 產生 JWT token的地方。(大約設置15分鐘左右，被盜取最多15分鐘有效，也可減輕 server 壓力。)
/// 關於 Fresh token 部分，直接放在資料庫管理，故不用要特別符合jwt格式，直接產一個16進位即可。(後端可以即時控管有效期)
/// 如有資安疑慮，可用 RSA 非對稱於前後端處理加解密(public key, private key)方式，另一方面如遇到攻擊，及時更換key也好處理跟追蹤。
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
        // 確認 appsetting 有無設定正確。
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
    ///  產出 JWT token
    /// </summary>
    /// <param name="account"></param>
    /// <param name="expiryHours"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GenerateToken<T>(T data, int expiryHours = 1)
    {
        if (string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidOperationException("Required configuration is missing");
        }

        // 透過反射將 T 類型的屬性轉為 claims
        var claims = data?.GetType()
                         .GetProperties()
                         .Select(p => new Claim(p.Name, p.GetValue(data)?.ToString() ?? ""))
                         .ToList();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(expiryHours), // Set expiration time as needed
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenDescriptor);
    }

    /// <summary>
    ///  驗證 JWT token
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
