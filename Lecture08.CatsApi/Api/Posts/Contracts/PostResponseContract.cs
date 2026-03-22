namespace Lecture08.CatsApi.Api.Posts.Contracts
{
	public sealed class PostResponseContract
	{
		public required Guid Uid { get; init; }

		public required Guid UserUid { get; init; }

		public required string Title { get; init; }

		public string? Description { get; init; }

		public required string PhotoUrl { get; init; }

		public required IReadOnlyCollection<Guid> CatUids { get; init; }

		public required DateTimeOffset CreatedAt { get; init; }
	}
}
