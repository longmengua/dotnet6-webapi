using dotnet6_webapi.DTO.Req;
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
    public IActionResult Login([FromBody] Login request)
    {
        var auth = authService.AuthenticateUser(request.Account ?? "", request.Password ?? "");
        // Log.Information("Received AuthenticateUser: {auth}", auth.Account);
        if (auth != null)
        {
            var token = AuthHelper.GenerateToken(request.Account ?? "", 1);
            var jwt = $"Bearer {token}";
            return Ok(new { jwt });
        }

        return Unauthorized("Invalid credentials");
    }

    [HttpPost("Register")]
    public IActionResult Register([FromBody] Register request)
    {
        var auth = authService.RegisterUser(request.Account ?? "", request.Password ?? "");
        return Ok(new { auth.Account });
    }

    [HttpPost("UpdatePassword")]
    public IActionResult UpdatePassword([FromBody] UpdatePassword request)
    {
        var auth = authService.UpdateUserPassword(request.Account ?? "", request.NewPassword ?? "");
        return Ok(new { auth.Account });
    }

    [HttpPost("RenewRefreshToken")]
    public IActionResult RenewRefreshToken([FromBody] RenewToken request)
    {
        var auth = authService.RenewUserRefreshToken(request.Account ?? "");
        return Ok(new { auth.Account });
    }
}
