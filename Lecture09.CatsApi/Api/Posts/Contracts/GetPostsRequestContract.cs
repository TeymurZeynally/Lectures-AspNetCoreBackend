using System.ComponentModel.DataAnnotations;

namespace Lecture09.CatsApi.Api.Posts.Contracts
{
	public sealed class GetPostsRequestContract
	{
		[Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
		public int Page { get; init; } = 1;

		[Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
		public int PageSize { get; init; } = 20;

		public Guid? UserUid { get; init; }

		public Guid? CatUid { get; init; }

		[StringLength(100, ErrorMessage = "Title must not exceed 100 characters.")]
		public string? Title { get; init; }
	}
}
