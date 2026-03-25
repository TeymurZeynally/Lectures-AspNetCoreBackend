using System.Text;
using Lecture08.CatsApi.Api.Cats.Services;
using Lecture08.CatsApi.Api.Posts.Services;
using Lecture08.CatsApi.Api.Users.Services;
using Lecture08.CatsDatabase.DataAccess.EF;
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

var app = builder.Build();

app.UseSwagger().UseSwaggerUI();

app.MapControllers();

app.Run();

