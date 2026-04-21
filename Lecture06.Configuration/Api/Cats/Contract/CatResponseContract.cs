namespace Lecture06.Configuration.Api.Cats.Contract
{
    public class CatResponseContract
    {
        public required int Id { get; init; }

        public required string Name { get; init; }

        public required int AgeYears { get; init; }

        public required double WeightKg { get; init; }
    }
}
