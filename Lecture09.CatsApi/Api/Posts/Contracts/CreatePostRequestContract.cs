using System.ComponentModel.DataAnnotations;

namespace Lecture09.CatsApi.Api.Posts.Contracts
{
	public sealed class CreatePostRequestContract
	{
		[Required]
		public required Guid UserUid { get; init; }

		[Required]
		[StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
		public required string Title { get; init; }

		[StringLength(4000, ErrorMessage = "Description must not exceed 4000 characters.")]
		public string? Description { get; init; }

		[Required]
		[StringLength(255, MinimumLength = 3, ErrorMessage = "Photo url must be between 3 and 255 characters.")]
		public required string PhotoUrl { get; init; }

		[Required]
		[MinLength(1, ErrorMessage = "At least one cat uid is required.")]
		public required IReadOnlyCollection<Guid> CatUids { get; init; }
	}
}
