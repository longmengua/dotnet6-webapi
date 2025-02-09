
using Microsoft.AspNetCore.Mvc;

namespace dotnet6_webapi.Controllers.external;

[Route("api/external/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Username == "test" && request.Password == "password")
        {
            var token = TokenService.GenerateToken(request.Username);
            return Ok(new { Token = token });
        }
        return Unauthorized();
    }
}

public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}