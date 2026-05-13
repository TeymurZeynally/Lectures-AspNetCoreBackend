using Lecture15.WebState.WeatherApp.Api.Weather.Contract;
using Lecture15.WebState.WeatherApp.Api.Weather.Exceptions;
using Lecture15.WebState.WeatherApp.Api.Weather.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Lecture15.WebState.WeatherApp.Api.Weather.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController(IWeatherService weatherService) : ControllerBase
{
	[HttpGet("get")]
    // Кэш на стороне клиента:
    // [ResponseCache(Duration = 1800 /*30min*/, Location = ResponseCacheLocation.Client)]

    // Кэш на стороне сервера:
    // [OutputCache(Duration = 1800)]
    public async Task<ActionResult<WeatherResult>> GetCurrent([FromQuery] string? city)
	{
		try
		{
			var weather = await weatherService.GetWeatherAsync(city ?? "Moscow");
			return Ok(weather);
		}
		catch (CityNotFoundException ex)
		{
			return NotFound(new { error = ex.Message });
		}
	}
}