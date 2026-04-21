namespace Lecture12.WebState.WeatherApp.Api.Weather.Contract
{
	public class WeatherResult
	{
		public required string City { get; init; }

		public required double Temperature {get; init; }

		public required WeatherCode WeatherCode {get; init; }

		public required double WindSpeed { get; init; }
	}
}
