namespace Lecture14.Auth.JWT.Full.Options
{
    public class JwtOptions
    {
        public required string Issuer { get; init; }

        public required string Audience { get; init; }

        public required string Key { get; init; }

        public TimeSpan AccessTokenExpiration { get; init; }

        public TimeSpan RefreshTokenExpiration { get; init; }
    }
}
