namespace Lecture13.Auth.JWT.Full.Api.Account.Services.Models
{
    public sealed record TokenPair(
        string AccessToken,
        string RefreshToken,
        DateTimeOffset AccessTokenExpiresAt,
        DateTimeOffset RefreshTokenExpiresAt);
}
