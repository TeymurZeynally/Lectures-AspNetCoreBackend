using Lecture15.WebState.WeatherApp.Api.Weather.Services;
using Lecture15.WebState.WeatherApp.External.HttpClients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IGeocodingClient, GeocodingClient>(client =>
	client.BaseAddress = builder.Configuration.GetSection("WeatherApi:GeocodingUrl").Get<Uri>());

builder.Services.AddHttpClient<IForecastClient, ForecastClient>(client =>
	client.BaseAddress = builder.Configuration.GetSection("WeatherApi:ForecastUrl").Get<Uri>());

builder.Services.AddScoped<IWeatherService, WeatherService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();