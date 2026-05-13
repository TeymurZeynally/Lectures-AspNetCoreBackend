using System.Globalization;
using System.Text.Json;
using Lecture15.WebState.WeatherApp.External.HttpClients.Models;

namespace Lecture15.WebState.WeatherApp.External.HttpClients;

internal class ForecastClient(HttpClient http) : IForecastClient
{
	public async Task<Forecast> GetCurrentAsync(double latitude, double longitude)
	{
		var json = await http.GetFromJsonAsync<JsonElement>(
			$"v1/forecast?latitude={latitude.ToString(CultureInfo.InvariantCulture)}&longitude={longitude.ToString(CultureInfo.InvariantCulture)}&current=temperature_2m,weather_code,wind_speed_10m&timezone=auto");

		var current = json.GetProperty("current");

		await Task.Delay(TimeSpan.FromSeconds(2));

		return new Forecast
		{
			Temperature = current.GetProperty("temperature_2m").GetDouble(),
			WeatherCode = (WeatherCode)current.GetProperty("weather_code").GetInt32(),
			WindSpeed = current.GetProperty("wind_speed_10m").GetDouble(),
		};
	}
}