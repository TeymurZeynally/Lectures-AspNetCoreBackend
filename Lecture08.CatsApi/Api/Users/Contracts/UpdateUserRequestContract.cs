using System.ComponentModel.DataAnnotations;

namespace Lecture08.CatsApi.Api.Users.Contracts
{
	public sealed class UpdateUserRequestContract
	{
		[Required]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
		[RegularExpression(@"^[A-Za-z][A-Za-z0-9_]{2,49}$", ErrorMessage = "Username must start with a letter and contain only letters, numbers, and underscores.")]
		public required string Username { get; init; }

		[Required]
		[EmailAddress]
		[StringLength(100)]
		public required string Email { get; init; }
	}
}
