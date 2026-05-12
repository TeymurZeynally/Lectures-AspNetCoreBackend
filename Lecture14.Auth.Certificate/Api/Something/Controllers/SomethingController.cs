using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lecture14.Auth.Certificate.Api.Something.Controllers;

[Authorize]
[ApiController]
[Route("api/some")]
public class SomethingController() : ControllerBase
{
    [HttpGet("data")]
    public IActionResult GetSomeData()
    {
        var name = User.Identity!.Name;

        return Ok(new
        {
            SomeData = "Hello, this is response from SomethingController",
            Name = name,
        });
    }
}