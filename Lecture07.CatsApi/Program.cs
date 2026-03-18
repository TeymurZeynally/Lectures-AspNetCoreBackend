using System.Text;
using Lecture07.CatsApi.Api.Cats.Services;
using Lecture07.CatsApi.Api.Posts.Services;
using Lecture07.CatsApi.Api.Users.Services;
using Lecture07.CatsDatabase.DataAccess.Repository.Auth;
using Lecture07.CatsDatabase.DataAccess.Repository.Cats;
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

var connectionString = builder.Configuration.GetConnectionString("CatsDb")!;
builder.Services
    .AddTransient<ICatsRepository, CatsRepository>(r => new CatsRepository(connectionString, r.GetRequiredService<ILogger<CatsRepository>>()))
    .AddTransient<IUsersRepository, UsersRepository>(r => new UsersRepository(connectionString, r.GetRequiredService<ILogger<UsersRepository>>()))
    .AddTransient<IPostsRepository, PostsRepository>(r => new PostsRepository(connectionString, r.GetRequiredService<ILogger<PostsRepository>>()))
    .AddTransient<ICatService, CatService>()
    .AddTransient<IUserService, UserService>()
    .AddTransient<IPostService, PostService>();

var app = builder.Build();

app.UseSwagger().UseSwaggerUI();

app.MapControllers();

app.Run();

