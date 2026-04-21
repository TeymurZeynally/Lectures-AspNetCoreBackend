using Lecture12.WebState.WeatherApp.External.HttpClients.Models;

namespace Lecture12.WebState.WeatherApp.External.HttpClients
{
	internal interface IForecastClient
	{
		Task<Forecast> GetCurrentAsync(double latitude, double longitude);
	}
}
