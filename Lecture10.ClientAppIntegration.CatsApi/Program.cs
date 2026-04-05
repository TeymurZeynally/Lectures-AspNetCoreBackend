using System.Text;
using Lecture10.ClientAppIntegration.CatsApi.Api.Cats.Services;
using Lecture10.ClientAppIntegration.CatsApi.Api.Posts.Filters;
using Lecture10.ClientAppIntegration.CatsApi.Api.Posts.Services;
using Lecture10.ClientAppIntegration.CatsApi.Api.Users.Services;
using Lecture10.ClientAppIntegration.CatsDatabase.DataAccess.EF;
using Microsoft.EntityFrameworkCore;
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

builder.Services
    .AddDbContext<CatsDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("CatsDb")))
    .AddTransient<ICatService, CatService>()
    .AddTransient<IUserService, UserService>()
    .AddTransient<IPostService, PostService>();


builder.Services
    .AddCors(options => options
        .AddPolicy("AllowFrontend", policy => policy
            .WithOrigins("http://localhost:54583")
            .AllowAnyHeader()
            .AllowAnyMethod()));


var app = builder.Build();

app.UseSwagger().UseSwaggerUI();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();

