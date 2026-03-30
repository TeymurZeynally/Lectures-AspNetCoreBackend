using Lecture09.CatsApi.Api.Users.Contracts;
using Lecture09.CatsDatabase.DataAccess.EF;
using Lecture09.CatsDatabase.DataAccess.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lecture09.CatsApi.Api.Users.Services
{
    public sealed class UserService(CatsDbContext dbContext, ILogger<UserService> logger) : IUserService
    {
        public async Task<UserResponseContract> CreateAsync(CreateUserRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting user creation. Username: {Username}, Email: {Email}.", request.Username, request.Email);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            };

            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("User entity persisted successfully. UserId: {UserId}, UserUid: {UserUid}, Username: {Username}, Email: {Email}.", user.Id, user.Uid, user.Username, user.Email);

            var response = new UserResponseContract
            {
                Uid = user.Uid,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };

            logger.LogInformation("User creation completed. UserUid: {UserUid}, Username: {Username}, Email: {Email}, CreatedAt: {CreatedAt}.", response.Uid, response.Username, response.Email, response.CreatedAt);

            return response;
        }

        public async Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting user deletion. UserUid: {UserUid}.", uid);

           
            var user = await dbContext.Users.Include(x => x.Posts).ThenInclude(x => x.Cats).Include(x => x.Cats).SingleOrDefaultAsync(x => x.Uid == uid, cancellationToken);

            if (user is null)
            {
                logger.LogWarning("User deletion skipped because user was not found. UserUid: {UserUid}.", uid);
                return false;
            }

            logger.LogInformation("User found for deletion. UserUid: {UserUid}, Username: {Username}, PostsCount: {PostsCount}, CatsCount: {CatsCount}.", user.Uid, user.Username, user.Posts.Count, user.Cats.Count);

            foreach (var post in user.Posts)
            {
                post.Cats.Clear();
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("User post relations cleared successfully. UserUid: {UserUid}.", user.Uid);

            dbContext.Posts.RemoveRange(user.Posts);
            dbContext.Cats.RemoveRange(user.Cats);
            dbContext.Users.Remove(user);

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("User deleted successfully with related entities. UserId: {UserId}, UserUid: {UserUid}, Username: {Username}.", user.Id, user.Uid, user.Username);

            return true;
        }

        public async Task<IReadOnlyCollection<UserResponseContract>> GetAllAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Loading all users.");

            var users = await dbContext.Users
                .AsNoTracking()
                .OrderBy(x => x.Username)
                .Select(x => new UserResponseContract
                {
                    Uid = x.Uid,
                    Username = x.Username,
                    Email = x.Email,
                    CreatedAt = x.CreatedAt
                })
                .ToArrayAsync(cancellationToken);

            logger.LogInformation("All users loaded successfully. Count: {Count}.", users.Length);

            return users;
        }

        public async Task<UserResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken)
        {
            logger.LogInformation("Loading user by uid. UserUid: {UserUid}.", uid);

            var user = await dbContext.Users
                .AsNoTracking()
                .Where(x => x.Uid == uid)
                .Select(x => new UserResponseContract
                {
                    Uid = x.Uid,
                    Username = x.Username,
                    Email = x.Email,
                    CreatedAt = x.CreatedAt
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                logger.LogWarning("User was not found by uid. UserUid: {UserUid}.", uid);
                return null;
            }

            logger.LogInformation("User loaded successfully by uid. UserUid: {UserUid}, Username: {Username}, Email: {Email}.", user.Uid, user.Username, user.Email);

            return user;
        }

        public async Task<UserResponseContract?> UpdateAsync(Guid uid, UpdateUserRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting user update. UserUid: {UserUid}, Username: {Username}, Email: {Email}.", uid, request.Username, request.Email);

            var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Uid == uid, cancellationToken);

            if (user is null)
            {
                logger.LogWarning("User update skipped because user was not found. UserUid: {UserUid}.", uid);
                return null;
            }

            user.Username = request.Username;
            user.Email = request.Email;

            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            var response = new UserResponseContract
            {
                Uid = user.Uid,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };

            logger.LogInformation("User updated successfully. UserUid: {UserUid}, Username: {Username}, Email: {Email}.", response.Uid, response.Username, response.Email);

            return response;
        }
    }
}