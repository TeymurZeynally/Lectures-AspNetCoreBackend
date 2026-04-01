using System.Text;
using Lecture09.ErrorHandling.CatsApi.Api.Cats.Services;
using Lecture09.ErrorHandling.CatsApi.Api.Posts.Filters;
using Lecture09.ErrorHandling.CatsApi.Api.Posts.Services;
using Lecture09.ErrorHandling.CatsApi.Api.Users.Services;
using Lecture09.ErrorHandling.CatsDatabase.DataAccess.EF;
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
/// builder.Services.AddControllers(o => o.Filters.Add<PostExceptionFilterAttribute>());

builder.Services
    .AddDbContext<CatsDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("CatsDb")))
    .AddTransient<ICatService, CatService>()
    .AddTransient<IUserService, UserService>()
    .AddTransient<IPostService, PostService>();

var app = builder.Build();

app.UseSwagger().UseSwaggerUI();

app.MapControllers();


app.Use(async (context, next) => {
    try
    {
        await next();
    }
    catch(Exception e)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { error = $"An error during request execution: {e.Message}" });
    }
});

/*
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { error = $"An error during request execution" });
    });
});

app.UseExceptionHandler("./error.html");
*/

app.Run();

