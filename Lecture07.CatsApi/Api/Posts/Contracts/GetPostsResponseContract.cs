namespace Lecture07.CatsApi.Api.Posts.Contracts
{
	public class GetPostsResponseContract
	{
		public required IReadOnlyCollection<PostResponseContract> Items { get; init; }

		public required int Page { get; init; }

		public required int PageSize { get; init; }

		public required int TotalCount { get; init; }
	}
}
