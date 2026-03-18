namespace Lecture07.CatsDatabase.DataAccess.Models.Auth
{
    public sealed class User
    {
        public required long Id { get; init; }

        public required Guid Uid { get; init; }

        public required string Username { get; init; }

        public required string Email { get; init; }

        public required string Password { get; init; }

        public required DateTimeOffset CreatedAt { get; init; }
    }
}
