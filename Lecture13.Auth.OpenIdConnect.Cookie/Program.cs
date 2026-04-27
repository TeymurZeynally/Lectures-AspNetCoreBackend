using System.Data;
using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(options =>
    {
        options.Authority = "http://localhost:8080/realms/demo";

        options.ClientId = "aspnet-web"; // Надо читать из конгфига
        options.ClientSecret = "aspnet-web-secret"; // Надо читать из конгфига/env

        options.ResponseType = "code";

        options.RequireHttpsMetadata = false; // Так не делать - надо всегда использовать HTTPS

        options.SaveTokens = true;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");

        options.CallbackPath = "/signin-oidc";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () =>
{
    return Results.Content("""
    <h1>ASP.NET + Keycloak</h1>
    <p><a href="/secure">Go to secure page</a></p>
    <p><a href="/logout">Logout</a></p>
    """, MediaTypeNames.Text.Html);
});

app.MapGet("/secure", (HttpContext http) =>
{
    var name = http.User.FindFirstValue("name");
    var email = http.User.FindFirstValue(ClaimTypes.Email);

    return Results.Content($"""
    <h1>Secure page</h1>
    <p>You are logged in.</p>
    <p>Name: {name} ({email})</p>
    <p><a href="/">Home</a></p>
    """, MediaTypeNames.Text.Html);
}).RequireAuthorization();

app.MapGet("/logout", async (HttpContext http) =>
{
    await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    await http.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
});

app.Run();