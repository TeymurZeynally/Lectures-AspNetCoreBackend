namespace Lecture12.WebState.WeatherApp.External.HttpClients.Models
{
	internal sealed class GeocodedCity
	{
		public required string Name { get; init; }

		public required double Latitude { get; init; }

		public required double Longitude { get; init; }
	}
}
