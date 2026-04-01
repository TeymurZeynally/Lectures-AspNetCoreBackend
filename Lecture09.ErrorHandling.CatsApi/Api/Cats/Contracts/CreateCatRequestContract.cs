using System.ComponentModel.DataAnnotations;

namespace Lecture09.ErrorHandling.CatsApi.Api.Cats.Contracts
{
	public sealed class CreateCatRequestContract
	{
		[Required]
		public required Guid UserUid { get; init; }

		[Required]
		[StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
		public required string Name { get; init; }

		[Required]
		[StringLength(50, MinimumLength = 2, ErrorMessage = "Breed must be between 2 and 50 characters.")]
		public required string Breed { get; init; }

		[Range(0, 50, ErrorMessage = "Age must be between 0 and 50.")]
		public required int Age { get; init; }
	}
}