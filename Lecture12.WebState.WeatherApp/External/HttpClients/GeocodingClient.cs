using System.Text.Json;
using Lecture12.WebState.WeatherApp.External.HttpClients.Models;

namespace Lecture12.WebState.WeatherApp.External.HttpClients;

internal class GeocodingClient(HttpClient http) : IGeocodingClient
{
	public async Task<GeocodedCity?> FindAsync(string city)
	{
		var json = await http.GetFromJsonAsync<JsonElement>($"v1/search?name={Uri.EscapeDataString(city)}&count=1&language=ru&format=json");

		if (!json.TryGetProperty("results", out var results) || results.GetArrayLength() == 0)
			return null;

		var place = results[0];

		return new GeocodedCity
		{
			Name = place.GetProperty("name").GetString()!,
			Latitude = place.GetProperty("latitude").GetDouble()!,
			Longitude = place.GetProperty("longitude").GetDouble()!,
		};
	}
}