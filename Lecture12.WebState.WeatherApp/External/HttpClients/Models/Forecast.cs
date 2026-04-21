using Lecture12.WebState.WeatherApp.Api.Weather.Contract;

namespace Lecture12.WebState.WeatherApp.External.HttpClients.Models
{
	internal sealed class Forecast
	{
		public required double Temperature { get; init; }

		public required WeatherCode WeatherCode { get; init; }

		public required double WindSpeed { get; init; }
	}
}
