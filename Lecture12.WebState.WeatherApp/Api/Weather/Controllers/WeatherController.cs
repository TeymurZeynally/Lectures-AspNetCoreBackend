using Lecture12.WebState.WeatherApp.Api.Weather.Contract;
using Lecture12.WebState.WeatherApp.Api.Weather.Exceptions;
using Lecture12.WebState.WeatherApp.Api.Weather.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Lecture12.WebState.WeatherApp.Api.Weather.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController(IWeatherService weatherService) : ControllerBase
{
	private const string CityCookie = "citySelected";
	private const string CityHistorySessionKey = "cityHistory";

	[HttpGet("current")]
	public async Task<ActionResult<WeatherResult>> GetCurrent([FromQuery] string? city)
	{
		var selctedCity = city ?? Request.Cookies[CityCookie] ?? "Moscow";

		WeatherResult weather;
		try
		{
			weather = await weatherService.GetWeatherAsync(selctedCity);
		}
		catch (CityNotFoundException ex)
		{
			return NotFound(new { error = ex.Message });
		}

		if (city != null)
		{
			Response.Cookies.Append(CityCookie, weather.City, new CookieOptions { HttpOnly = true, MaxAge = TimeSpan.FromDays(30) });

			var historyJson = HttpContext.Session.GetString(CityHistorySessionKey);
			var history = historyJson is null ? [] : (JsonSerializer.Deserialize<string[]>(historyJson) ?? []);
			HttpContext.Session.SetString(CityHistorySessionKey, JsonSerializer.Serialize(history.Append(city)));
		}

		return Ok(weather);
	}

	[HttpGet("state")]
	public IActionResult GetState()
	{
		var historyJson = HttpContext.Session.GetString(CityHistorySessionKey);
		var history = historyJson is null ? [] : (JsonSerializer.Deserialize<string[]>(historyJson) ?? []);

		return Ok(new
		{
			CurrentCityFromCookie = Request.Cookies[CityCookie],
			RecentCitiesFromSession = history
		});
	}
}