using Lecture07.CatsApi.Api.Users.Contracts;
using Lecture07.CatsDatabase.DataAccess.Models.Auth;
using Lecture07.CatsDatabase.DataAccess.Repository.Auth;

namespace Lecture07.CatsApi.Api.Users.Services
{
    public sealed class UserService(IUsersRepository repository, ILogger<UserService> logger) : IUserService
    {
        public async Task<UserResponseContract> CreateAsync(CreateUserRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating user in service. Username: {Username}, Email: {Email}.", request.Username, request.Email);

            var user = await repository.Create(request.Username, request.Email, request.Password, cancellationToken);

            logger.LogInformation("Successfully created user in service with uid: {UserUid}.", user.Uid);

            return Map(user);
        }

        public async Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting user in service by uid: {UserUid}.", uid);

            var deleted = await repository.Delete(uid, cancellationToken);

            logger.LogInformation(deleted ? "Successfully deleted user in service by uid: {UserUid}." : "User with uid {UserUid} was not found for deletion in service.", uid);

            return deleted;
        }

        public async Task<IReadOnlyCollection<UserResponseContract>> GetAllAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all users in service.");

            var users = await repository.GetAll(cancellationToken);

            var result = users.Select(Map).ToArray();

            logger.LogInformation("Successfully retrieved {UsersCount} users in service.", result.Length);

            return result;
        }

        public async Task<UserResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting user by uid in service: {UserUid}.", uid);

            var user = await repository.GetByUid(uid, cancellationToken);

            if (user is null)
            {
                logger.LogWarning("User with uid {UserUid} was not found in service.", uid);
                return null;
            }

            logger.LogInformation("Successfully retrieved user by uid in service: {UserUid}.", uid);

            return Map(user);
        }

        public async Task<UserResponseContract?> UpdateAsync(Guid uid, UpdateUserRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating user in service. UserUid: {UserUid}, Username: {Username}, Email: {Email}.", uid, request.Username, request.Email);

            var user = await repository.Update(uid, request.Username, request.Email, cancellationToken);

            if (user is null)
            {
                logger.LogWarning("User with uid {UserUid} was not found for update in service.", uid);
                return null;
            }

            logger.LogInformation("Successfully updated user in service with uid: {UserUid}.", uid);

            return Map(user);
        }

        private static UserResponseContract Map(User user)
        {
            return new UserResponseContract
            {
                Uid = user.Uid,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
            };
        }
    }
}