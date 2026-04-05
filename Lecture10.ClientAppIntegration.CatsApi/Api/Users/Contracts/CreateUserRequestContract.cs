using System.ComponentModel.DataAnnotations;

namespace Lecture10.ClientAppIntegration.CatsApi.Api.Users.Contracts
{
	public sealed class CreateUserRequestContract
	{
		[Required]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
		[RegularExpression(@"^[A-Za-z][A-Za-z0-9_]{2,49}$", ErrorMessage = "Username must start with a letter and contain only letters, numbers, and underscores.")]
		public required string Username { get; init; }

		[Required]
		[EmailAddress]
		[StringLength(100)]
		public required string Email { get; init; }

		[Required]
		[StringLength(255, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
		public required string Password { get; init; }
	}
}
