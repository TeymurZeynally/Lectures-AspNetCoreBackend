using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lecture13.Auth.Basic.Api.Something.Controllers;

[ApiController]
[Route("api/some")]
public class SomethingController() : ControllerBase
{
	[HttpGet("data")]
    [Authorize(Roles = "Administrator")]
	public IActionResult GetSomeData()
	{
        var name = this.User.Identity?.Name;
        var role = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

        return Ok(new
		{
            SuperSecureDataForAdminsOnly = "hi",
			SomeData = "Hello, this is response from SomethingController.GetSomeData",
            Name = name,
            Role = role,
        });
	}


    [HttpGet("data2")]
    [Authorize(Roles = "Administrator,User")]
    public IActionResult GetSomeOtherData()
    {
        var name = this.User.Identity?.Name;
        var role = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

        return Ok(new
        {
            SomeUserData = "userData",
            SomeData = "Hello, this is response from SomethingController.GetSomeOtherData",
            Name = name,
            Role = role,
        });
    }
}