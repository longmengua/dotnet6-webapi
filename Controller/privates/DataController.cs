using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet6_webapi.Controller.privates;

[Route("api/privates/Data")]
[ApiController]
public class DataController : ControllerBase
{
    public DataController() { }

    [HttpGet("")]
    public IActionResult GetData()
    {
        return Ok(new { message = "This is a protected endpoint!" });
    }
}
