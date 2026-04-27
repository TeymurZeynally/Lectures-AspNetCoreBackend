using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lecture13.Auth.JWT.Full.Api.Something.Controllers;

[Authorize]
[ApiController]
[Route("api/some")]
public class SomethingController() : ControllerBase
{
    [HttpGet("data")]
    public IActionResult GetSomeData()
    {
        var name = User.Identity?.Name;
        var role = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new
        {
            SomeData = "Hello, this is response from SomethingController",
            Name = name,
            Role = role
        });
    }
}