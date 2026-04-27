using Lecture13.Auth.JWT.Full.Api.Account.Contract;
using Lecture13.Auth.JWT.Full.Api.Account.Services.Models;
using Lecture13.Auth.JWT.Full.DataAccess;
using Lecture13.Auth.JWT.Full.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lecture13.Auth.JWT.Full.Api.Account.Services
{
    internal sealed class UserService(
        UsersDbContext usersDbContext,
        IPasswordHashService passwordHashService,
        ITokenService tokenService) : IUserService
    {
        public async Task<AuthResult> Register(CredentialsRequest credentialsRequest, CancellationToken cancellationToken)
        {
            var login = credentialsRequest.Login.Trim();

            if (string.IsNullOrWhiteSpace(login))
            {
                return AuthResult.Failure("Login is required.");
            }

            if (string.IsNullOrWhiteSpace(credentialsRequest.Password))
            {
                return AuthResult.Failure("Password is required.");
            }

            var alreadyExists = await usersDbContext.Users.AnyAsync(x => x.Login == login, cancellationToken);

            if (alreadyExists)
            {
                return AuthResult.Failure("User with this login already exists.");
            }

            var passwordHash = passwordHashService.HashPassword(credentialsRequest.Password);

            var user = new User
            {
                Uid = Guid.NewGuid(),
                Login = login,
                PasswordHash = passwordHash.HashBytes,
                PasswordSalt = passwordHash.SaltBytes,
                Role = "User",
                CreationTimestamp = DateTimeOffset.UtcNow
            };

            usersDbContext.Users.Add(user);

            await usersDbContext.SaveChangesAsync(cancellationToken);

            var tokens = await tokenService.CreateTokenPairAsync(user.Uid, cancellationToken);

            return AuthResult.Success(tokens);
        }

        public async Task<AuthResult> Login(CredentialsRequest credentialsRequest, CancellationToken cancellationToken)
        {
            var login = credentialsRequest.Login.Trim();

            var user = await usersDbContext.Users.SingleOrDefaultAsync(x => x.Login == login, cancellationToken);

            if (user is null)
            {
                return AuthResult.Failure("Invalid login or password.");
            }

            var passwordIsValid = passwordHashService.VerifyPassword(credentialsRequest.Password, user.PasswordHash, user.PasswordSalt);

            if (!passwordIsValid)
            {
                return AuthResult.Failure("Invalid login or password.");
            }

            var tokens = await tokenService.CreateTokenPairAsync(user.Uid, cancellationToken);

            return AuthResult.Success(tokens);
        }
    }
}
