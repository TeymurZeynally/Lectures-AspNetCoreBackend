using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lecture13.Auth.OAuth.Api.Something.Controllers;

[Authorize]
[ApiController]
[Route("api/some")]
public class SomethingController() : ControllerBase
{
    [HttpGet("data")]
    public IActionResult GetSomeData()
    {
        var name = User.Identity!.Name;

        var role = User.Claims.First(x => x.Type == ClaimTypes.Role).Value;


        return Ok(new
        {
            SomeData = "Hello, this is response from SomethingController"
        });
    }
}