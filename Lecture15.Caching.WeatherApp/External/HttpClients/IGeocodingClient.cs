using Lecture15.WebState.WeatherApp.External.HttpClients.Models;

namespace Lecture15.WebState.WeatherApp.External.HttpClients
{
	internal interface IGeocodingClient
	{
		Task<GeocodedCity?> FindAsync(string city);
	}
}
