using Lecture15.WebState.WeatherApp.Api.Weather.Contract;

namespace Lecture15.WebState.WeatherApp.Api.Weather.Services
{
	public interface IWeatherService
	{
		Task<WeatherResult> GetWeatherAsync(string city);
	}
}