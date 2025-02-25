using dotnet6_webapi.DTOs;
using dotnet6_webapi.Service;
using dotnet6_webapi.Utils;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace dotnet6_webapi.Controller.publics;

[Route("api/publics/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService authService;

    public AuthController(AuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginReq request)
    {
        Log.Information("Received request: {account}, {password}", request.Account, request.Password);
        // 這邊應該連接 SSO 進行用戶驗證
        var auth = authService.AuthenticateUser(request.Account ?? "", request.Password ?? "");
        Log.Information("Received AuthenticateUser: {auth}", auth.Account);
        if (auth != null)
        {
            var token = AuthHelper.GenerateToken(request.Account ?? "", 1);
            var jwt = $"Bearer {token}";
            return Ok(new { jwt });
        }

        return Unauthorized("Invalid credentials");
    }
}
