using dotnet6_webapi.Models;
using dotnet6_webapi.Utils;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace dotnet6_webapi.Controllers.external;

[Route("api/external/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        Log.Information("Received request: {account}, {password}", request.Account, request.Password);
        // 這邊應該連接 SSO 進行用戶驗證
        var auth = AuthenticateUser(request.Account ?? "", request.Password ?? "");
        Log.Information("Received AuthenticateUser: {auth}", auth.Account);
        if (auth != null)
        {
            var token = AuthHelper.GenerateToken(request.Account ?? "", 1);
            var jwt = $"Bearer {token}";
            return Ok(new { jwt });
        }

        return Unauthorized("Invalid credentials");
    }

    // 模擬 SSO 認證 (這裡你應該實現與 SSO 伺服器的連接)
    private Auth AuthenticateUser(string account, string password)
    {
        var toReturn = new Auth();
        Log.Information("AuthenticateUser Result: {result}", account == "admin" && password == "123");
        if (account == "admin" && password == "123") // 這是模擬邏輯，應該與 SSO 驗證接口對接
        {
            toReturn.Account = account;
        }
        return toReturn;
    }
}

public class LoginRequest
{
    public string? Account { get; set; }
    public string? Password { get; set; }
}
