namespace Lecture07.CatsDatabase.DataAccess.Models.Cats
{
    public class Cat
    {
        public required long Id { get; init; }

        public required Guid Uid { get; init; }

        public required long UserId { get; init; }

        public required Guid UserUid { get; init; }

        public required string Name { get; init; }

        public required string Breed { get; init; }

        public required int Age { get; init; }

        public required DateTimeOffset CreatedAt { get; init; }
    }
}
