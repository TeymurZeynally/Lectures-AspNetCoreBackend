using System.Text.Json;
using Lecture15.WebState.WeatherApp.Api.Weather.Contract;
using Lecture15.WebState.WeatherApp.Api.Weather.Exceptions;
using Lecture15.WebState.WeatherApp.External.HttpClients;
using Lecture15.WebState.WeatherApp.External.HttpClients.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Lecture15.WebState.WeatherApp.Api.Weather.Services;

internal class WeatherService(
    IDistributedCache distributedCache,
	IGeocodingClient geocodingClient,
	IForecastClient forecastClient) : IWeatherService
{
    public async Task<WeatherResult> GetWeatherAsync(string city)
    {
        var cacheKey = $"{nameof(WeatherService)}:{nameof(GetWeatherAsync)}:{city}";

        var place = await GetCached(cacheKey, () => geocodingClient.FindAsync(city));

        if (place == null)
        {
            throw new CityNotFoundException(city);
        }

        var forecast = await forecastClient.GetCurrentAsync(place.Latitude, place.Longitude);

        return new WeatherResult
        {
            City = place.Name,
            Temperature = forecast.Temperature,
            WeatherCode = (Contract.WeatherCode)forecast.WeatherCode,
            WindSpeed = forecast.WindSpeed
        };
    }

    private async Task<T> GetCached<T>(string cacheKey, Func<Task<T>> actionFn)
    {
        var cachedDataJson = await distributedCache.GetStringAsync(cacheKey);

        if (cachedDataJson != null)
        {
            return JsonSerializer.Deserialize<T>(cachedDataJson)!;
        }

        var data = await actionFn();

        await distributedCache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(data),
            new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(25) });

        return data;
    }
}
