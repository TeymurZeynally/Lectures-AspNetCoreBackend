using Lecture12.WebState.WeatherApp.External.HttpClients.Models;

namespace Lecture12.WebState.WeatherApp.External.HttpClients
{
	internal interface IGeocodingClient
	{
		Task<GeocodedCity?> FindAsync(string city);
	}
}
