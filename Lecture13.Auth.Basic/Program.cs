using System.Security.Claims;
using AspNetCore.Authentication.Basic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication(BasicDefaults.AuthenticationScheme)
    .AddBasic(o =>
    {
        o.Realm = "My app";
        o.Events.OnValidateCredentials = (ctx) =>
        {
            if (!(ctx.Username == "admin" && ctx.Password == "passwd"))
            {
                ctx.ValidationFailed();
                return Task.CompletedTask;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, ctx.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            ctx.ValidationSucceeded(claims);
            return Task.CompletedTask;
        };
    });


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();