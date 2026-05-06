using System.Security.Claims;
using AspNetCore.Authentication.Basic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication(BasicDefaults.AuthenticationScheme)
    .AddBasic(o =>
    {
        o.Realm = "My application";
        o.Events.OnValidateCredentials = (ctx) =>
        {
            // сходить в базу по username, password
            if (!((ctx.Username == "admin" || ctx.Username == "user") && ctx.Password == "passwd"))
            {
                ctx.ValidationFailed();
                return Task.CompletedTask;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, ctx.Username),
                ctx.Username == "admin" 
                    ? new Claim(ClaimTypes.Role, "Administrator") 
                    : new Claim(ClaimTypes.Role, "User"),
            };

            ctx.ValidationSucceeded(claims);
            return Task.CompletedTask;
        };
    });



var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();