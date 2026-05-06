using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Lecture14.Auth.JWT.Full.Api.Account.Services.Models;
using Lecture14.Auth.JWT.Full.DataAccess;
using Lecture14.Auth.JWT.Full.DataAccess.Entities;
using Lecture14.Auth.JWT.Full.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Lecture14.Auth.JWT.Full.Api.Account.Services
{
    internal sealed class TokenService(UsersDbContext usersDbContext, IOptions<JwtOptions> jwtOptions) : ITokenService
    {
        public async Task<TokenPair> CreateTokenPairAsync(Guid userUid, CancellationToken cancellationToken)
        {
            var user = usersDbContext.Users.Single(x => x.Uid == userUid);

            var now = DateTimeOffset.UtcNow;

            var accessTokenExpiresAt = now.Add(jwtOptions.Value.AccessTokenExpiration);
            var refreshTokenExpiresAt = now.Add(jwtOptions.Value.RefreshTokenExpiration);

            var accessToken = CreateAccessToken(user, accessTokenExpiresAt);
            var refreshToken = GenerateRefreshToken();

            usersDbContext.RefreshTokens.Add(new RefreshToken
            {
                Uid = Guid.NewGuid(),
                User = user,
                TokenHash = HashRefreshToken(refreshToken),
                CreationTimestamp = now,
                ExpirationTimestamp = refreshTokenExpiresAt
            });

            await usersDbContext.SaveChangesAsync(cancellationToken);

            return new TokenPair(
                Encoding.UTF8.GetString(accessToken),
                Convert.ToBase64String(refreshToken),
                accessTokenExpiresAt,
                refreshTokenExpiresAt);
        }

        public async Task<TokenPair?> RefreshAsync(string refreshTokenBase64, CancellationToken cancellationToken)
        {
            var tokenHash = HashRefreshToken(Convert.FromBase64String(refreshTokenBase64));

            var storedRefreshToken = await usersDbContext.RefreshTokens
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

            if (storedRefreshToken is null)
            {
                return null;
            }

            var now = DateTimeOffset.UtcNow;

            if (storedRefreshToken.RevokedTimestamp is not null)
            {
                return null;
            }

            if (storedRefreshToken.ExpirationTimestamp <= now)
            {
                return null;
            }

            storedRefreshToken.RevokedTimestamp = now;

            var newRefreshToken = GenerateRefreshToken();

            var accessTokenExpiresAt = now.Add(jwtOptions.Value.AccessTokenExpiration);
            var refreshTokenExpiresAt = now.Add(jwtOptions.Value.RefreshTokenExpiration);

            var accessToken = CreateAccessToken(storedRefreshToken.User, accessTokenExpiresAt);

            usersDbContext.RefreshTokens.Add(new RefreshToken
            {
                Uid = Guid.NewGuid(),
                User = storedRefreshToken.User,
                TokenHash = HashRefreshToken(newRefreshToken),
                CreationTimestamp = now,
                ExpirationTimestamp = refreshTokenExpiresAt
            });

            await usersDbContext.SaveChangesAsync(cancellationToken);

            return new TokenPair(
                Encoding.UTF8.GetString(accessToken),
                Convert.ToBase64String(newRefreshToken),
                accessTokenExpiresAt,
                refreshTokenExpiresAt);
        }

        public async Task<bool> RevokeAsync(string refreshTokenBase64, CancellationToken cancellationToken)
        {
            var tokenHash = HashRefreshToken(Convert.FromBase64String(refreshTokenBase64));

            var storedRefreshToken = await usersDbContext.RefreshTokens
                .SingleOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

            if (storedRefreshToken is null)
            {
                return false;
            }

            if (storedRefreshToken.RevokedTimestamp is not null)
            {
                return true;
            }

            storedRefreshToken.RevokedTimestamp = DateTimeOffset.UtcNow;

            await usersDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        private byte[] CreateAccessToken(User user, DateTimeOffset expiresAt)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Uid.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.Value.Key));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Value.Issuer,
                audience: jwtOptions.Value.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt.UtcDateTime,
                signingCredentials: credentials);

            return Encoding.UTF8.GetBytes(new JwtSecurityTokenHandler().WriteToken(token));
        }

        private static byte[] GenerateRefreshToken() => RandomNumberGenerator.GetBytes(64);

        private static byte[] HashRefreshToken(byte[] refreshToken) => SHA256.HashData(refreshToken);
    }
}
