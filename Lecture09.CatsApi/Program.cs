using System.Text;
using Lecture09.CatsApi.Api.Cats.Services;
using Lecture09.CatsApi.Api.Posts.Services;
using Lecture09.CatsApi.Api.Users.Services;
using Lecture09.CatsDatabase.DataAccess.EF;
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

