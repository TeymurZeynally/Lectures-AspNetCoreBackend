using Lecture09.CatsApi.Api.Users.Contracts;

namespace Lecture09.CatsApi.Api.Users.Services
{
	public interface IUserService
	{
		Task<IReadOnlyCollection<UserResponseContract>> GetAllAsync(CancellationToken cancellationToken);

		Task<UserResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken);

		Task<UserResponseContract> CreateAsync(CreateUserRequestContract request, CancellationToken cancellationToken);

		Task<UserResponseContract?> UpdateAsync(Guid uid, UpdateUserRequestContract request, CancellationToken cancellationToken);

		Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken);
	}
}
