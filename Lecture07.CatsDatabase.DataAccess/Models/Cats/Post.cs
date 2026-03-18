namespace Lecture07.CatsDatabase.DataAccess.Models.Cats
{
    public sealed class Post
    {
        public required long Id { get; init; }

        public required Guid Uid { get; init; }

        public required long UserId { get; init; }

        public required Guid UserUid { get; init; }

        public required string Title { get; init; }

        public string? Description { get; init; }

        public required string PhotoUrl { get; init; }

        public required IReadOnlyCollection<Guid> CatUids { get; init; }

        public required DateTimeOffset CreatedAt { get; init; }
    }
}
