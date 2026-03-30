namespace Lecture09.CatsApi.Api.Users.Contracts
{
	public class UserResponseContract
	{
		public required Guid Uid { get; init; }

		public required string Username { get; init; }

		public required string Email { get; init; }

		public required DateTimeOffset CreatedAt { get; init; }
	}
}
