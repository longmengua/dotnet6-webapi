using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace dotnet6_webapi.Controller
{
    [Route("/")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        /// <summary>
        /// 取得server 版本號碼
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public IActionResult GetServerVersion()
        {
            try
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Version not found";
                return Ok(new { Version = version });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
