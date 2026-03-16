using Lecture07.CatsApi.Api.Users.Contracts;

namespace Lecture07.CatsApi.Api.Users.Services
{
	internal class UserService : IUserService
	{
		public Task<UserResponseContract> CreateAsync(CreateUserRequestContract request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyCollection<UserResponseContract>> GetAllAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<UserResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<UserResponseContract?> UpdateAsync(Guid uid, UpdateUserRequestContract request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
