using Lecture08.CatsApi.Api.Users.Contracts;

namespace Lecture08.CatsApi.Api.Users.Services
{
    public sealed class UserService(ILogger<UserService> logger) : IUserService
    {
        public async Task<UserResponseContract> CreateAsync(CreateUserRequestContract request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<UserResponseContract>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponseContract?> UpdateAsync(Guid uid, UpdateUserRequestContract request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}