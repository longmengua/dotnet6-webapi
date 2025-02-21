using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet6_webapi.Controller.external;

[Route("api/external/data")]
[ApiController]
[Authorize]
public class DataController : ControllerBase
{
    private readonly ILogger<DataController> _logger;
    public DataController(ILogger<DataController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetData()
    {
        return Ok(new { message = "This is a protected endpoint!" });
    }
}
