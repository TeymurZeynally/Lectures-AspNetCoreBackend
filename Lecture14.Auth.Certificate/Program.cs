using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Server.Kestrel.Https;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(https =>
    {
        https.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
    });
});

builder.Services
    .AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options =>
    {
        options.AllowedCertificateTypes = CertificateTypes.All;

        options.Events = new CertificateAuthenticationEvents
        {
            OnCertificateValidated = context =>
            {
                // Тут можно проверить CN и Issuer у context.ClientCertificate
                // Например так
                // Только это вот из конфига              ↓↓↓↓↓↓↓↓↓↓↓↓↓↓
                if (context.ClientCertificate.Subject != "CN=test-client")
                {
                    context.Fail("Invalid client Subject");
                    return Task.CompletedTask;
                }

                // Чтобы выписать серт:
                // openssl req -x509 -newkey rsa:4096 -sha256 -days 365 -nodes -keyout client.key -out client.crt -subj "/CN=test-client" -addext "extendedKeyUsage=clientAuth"
                // openssl pkcs12 -export -out client.pfx -inkey client.key -in client.crt -password pass:12345
                // ↑ нужно чтобы ОС ему доверяла

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, context.ClientCertificate.Subject),
                    new Claim(ClaimTypes.Name, context.ClientCertificate.Subject)
                };

                context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));

                context.Success();

                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();