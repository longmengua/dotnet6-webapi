using Microsoft.AspNetCore.Mvc;
using dotnet6_webapi.DTO;
using dotnet6_webapi.DTO.Res;

namespace dotnet6_webapi.Controller.internals;

[ApiController]
[Route("api/internals/WeatherForecast")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetInfo")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        await Task.Delay(15000); // 15秒，如果用 System.Threading.Thread.Sleep，不會觸發。
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
