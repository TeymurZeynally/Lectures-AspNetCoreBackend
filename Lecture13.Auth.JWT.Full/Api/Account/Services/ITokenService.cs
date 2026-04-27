using Lecture13.Auth.JWT.Full.Api.Account.Services.Models;

namespace Lecture13.Auth.JWT.Full.Api.Account.Services
{
    public interface ITokenService
    {
        Task<TokenPair> CreateTokenPairAsync(Guid userUid, CancellationToken cancellationToken);

        Task<TokenPair?> RefreshAsync(string refreshToken, CancellationToken cancellationToken);

        Task<bool> RevokeAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
