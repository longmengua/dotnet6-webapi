using dotnet6_webapi.DTO.Req;
using dotnet6_webapi.Service;
using dotnet6_webapi.Utils;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace dotnet6_webapi.Controller.publics;

[Route("api")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService authService;

    public AuthController(AuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("publics/auth/Login")]
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

    [HttpPost("publics/auth/Register")]
    public IActionResult Register([FromBody] Register request)
    {
        if (string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Account and Password are required");
        }

        var (auth, jwtToken, refreshToken) = authService.RegisterUser(
            request.Account,
            request.Password,
            request.FirstName ?? "",
            request.MiddleName ?? "",
            request.LastName ?? "",
            request.Email ?? "",
            request.Phone ?? ""
        );
        return Ok(new { auth.Account, jwtToken, refreshToken });
    }

    [HttpPost("private/auth/UpdatePassword")]
    public IActionResult UpdatePassword([FromBody] UpdatePassword request)
    {
        var auth = authService.UpdateUserPassword(request.Account ?? "", request.NewPassword ?? "");
        return Ok(new { auth.Account });
    }

    [HttpPost("internal/auth/RenewRefreshToken")]
    public IActionResult RenewRefreshToken([FromBody] RenewToken request)
    {
        var auth = authService.RenewUserRefreshToken(request.Account ?? "");
        return Ok(new { auth.Account });
    }
}