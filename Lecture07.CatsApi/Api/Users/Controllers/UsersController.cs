using Lecture07.CatsApi.Api.Users.Contracts;
using Lecture07.CatsApi.Api.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lecture07.CatsApi.Api.Users.Controllers
{
	[ApiController]
	[Route("api/users")]
	public sealed partial class UsersController(
		IUserService userService,
		ILogger<UsersController> logger) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<UserResponseContract[]>> GetAll(CancellationToken cancellationToken)
		{
			logger.LogInformation("Getting all users.");

			var users = await userService.GetAllAsync(cancellationToken);

			logger.LogInformation("Successfully retrieved {UsersCount} users.", users.Count);

			return Ok(users);
		}

		[HttpGet("{uid:guid}")]
		public async Task<ActionResult<UserResponseContract>> GetByUid([FromRoute] Guid uid, CancellationToken cancellationToken)
		{
			logger.LogInformation("Getting user by uid: {UserUid}.", uid);

			var user = await userService.GetByUidAsync(uid, cancellationToken);

			if (user is null)
			{
				logger.LogWarning("User with uid {UserUid} was not found.", uid);
				return NotFound();
			}

			logger.LogInformation("Successfully retrieved user with uid: {UserUid}.", uid);

			return Ok(user);
		}

		[HttpPost]
		public async Task<ActionResult<UserResponseContract>> Create(
			[FromBody] CreateUserRequestContract request,
			CancellationToken cancellationToken)
		{
			logger.LogInformation("Creating user with username: {Username}, email: {Email}.", request.Username, request.Email);

			var createdUser = await userService.CreateAsync(request, cancellationToken);

			logger.LogInformation("Successfully created user with uid: {UserUid}, username: {Username}.", createdUser.Uid, createdUser.Username);

			return CreatedAtAction(nameof(GetByUid), new { uid = createdUser.Uid }, createdUser);
		}

		[HttpPut("{uid:guid}")]
		public async Task<ActionResult<UserResponseContract>> Update(
			[FromRoute] Guid uid,
			[FromBody] UpdateUserRequestContract request,
			CancellationToken cancellationToken)
		{
			logger.LogInformation("Updating user with uid: {UserUid}. New username: {Username}, new email: {Email}.", uid, request.Username, request.Email);

			var updatedUser = await userService.UpdateAsync(uid, request, cancellationToken);

			if (updatedUser is null)
			{
				logger.LogWarning("User with uid {UserUid} was not found for update.", uid);
				return NotFound();
			}

			logger.LogInformation("Successfully updated user with uid: {UserUid}.", uid);

			return Ok(updatedUser);
		}

		[HttpDelete("{uid:guid}")]
		public async Task<IActionResult> Delete([FromRoute] Guid uid, CancellationToken cancellationToken)
		{
			logger.LogInformation("Deleting user with uid: {UserUid}.", uid);

			var deleted = await userService.DeleteAsync(uid, cancellationToken);

			if (!deleted)
			{
				logger.LogWarning("User with uid {UserUid} was not found for deletion.", uid);
				return NotFound();
			}

			logger.LogInformation("Successfully deleted user with uid: {UserUid}.", uid);

			return NoContent();
		}
	}
}