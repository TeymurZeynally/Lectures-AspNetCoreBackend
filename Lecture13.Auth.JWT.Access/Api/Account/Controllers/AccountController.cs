using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lecture13.Auth.JWT.Access.Api.Account.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Lecture13.Auth.JWT.Access.Api.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController() : ControllerBase
    {
        [HttpPost]
        [Route("token/access")]
        public IActionResult GetAccessToken(Credentials credentials)
        {
            return Ok();
        }
    }
}
