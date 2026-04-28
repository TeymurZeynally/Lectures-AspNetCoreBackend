using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lecture13.Auth.Cookie.Api.Something.Controllers;

[ApiController]
[Route("api/some")]
public class SomethingController() : ControllerBase
{
    [HttpGet("data")]
    public IActionResult GetSomeData()
    {
        return Ok(new
        {
            SomeData = "Hello, this is response from SomethingController"
        });
    }
}