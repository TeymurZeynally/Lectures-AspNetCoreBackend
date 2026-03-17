using System.Text;
using Serilog;

Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(l =>
{
    l.ClearProviders();
    l.AddSerilog(new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger());
});

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
var app = builder.Build();

app.UseSwagger().UseSwaggerUI();

app.MapControllers();

app.Run();

