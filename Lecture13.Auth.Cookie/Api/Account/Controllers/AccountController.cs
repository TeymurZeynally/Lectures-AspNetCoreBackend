using System.Security.Claims;
using Lecture13.Auth.Cookie.Api.Account.Contract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Lecture13.Auth.Cookie.Api.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController() : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginCookie(Credentials credentials)
        {
            return Ok();
        }
    }
}
