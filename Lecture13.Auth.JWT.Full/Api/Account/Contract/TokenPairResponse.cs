namespace Lecture13.Auth.JWT.Full.Api.Account.Contract
{
    public class TokenPairResponse
    {
        public required string AccessToken { get; init; }

        public required string RefreshToken { get; init; }

        public required DateTimeOffset AccessTokenExpiresAt { get; init; }

        public required DateTimeOffset RefreshTokenExpiresAt { get; init; }
    }
}
