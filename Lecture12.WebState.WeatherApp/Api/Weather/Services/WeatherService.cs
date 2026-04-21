using Lecture12.WebState.WeatherApp.Api.Weather.Contract;
using Lecture12.WebState.WeatherApp.External.HttpClients;
using WebApplication14.Api.Weather.Exceptions;

namespace Lecture12.WebState.WeatherApp.Api.Weather.Services;


internal class WeatherService(IGeocodingClient geocodingClient,IForecastClient forecastClient) : IWeatherService
{
	public async Task<WeatherResult> GetWeatherAsync(string city)
	{
		var place = await geocodingClient.FindAsync(city);

		if (place == null)
		{
			throw new CityNotFoundException(city);
		}

		var forecast = await forecastClient.GetCurrentAsync(place.Latitude, place.Longitude);

		return new WeatherResult
		{
			City = place.Name,
			Temperature = forecast.Temperature,
			WeatherCode = forecast.WeatherCode,
			WindSpeed = forecast.WindSpeed
		};
	}
}
