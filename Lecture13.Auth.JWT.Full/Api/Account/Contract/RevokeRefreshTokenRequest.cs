namespace Lecture13.Auth.JWT.Full.Api.Account.Contract
{
    public class RevokeRefreshTokenRequest
    {
        public required string RefreshToken { get; init; }
    }
}
