using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lecture13.Auth.JWT.Access.Api.Account.Contract;
using Lecture13.Auth.JWT.Access.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Lecture13.Auth.JWT.Access.Api.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IOptions<JwtOptions> jwtOptions) : ControllerBase
    {
        [HttpPost]
        [Route("token/access")]
        public IActionResult GetAccessToken(Credentials credentials)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, credentials.Login),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Value.Issuer,
                audience: jwtOptions.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
                signingCredentials: creds);

            var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { Token = stringToken });
        }
    }
}
