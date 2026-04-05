namespace Lecture10.ClientAppIntegration.CatsApi.Api.Cats.Contracts
{
	public sealed class CatResponseContract
	{
		public required Guid Uid { get; init; }

		public required Guid UserUid { get; init; }

		public required string Name { get; init; }

		public required string Breed { get; init; }

		public required int Age { get; init; }

		public required DateTimeOffset CreatedAt { get; init; }
	}
}
