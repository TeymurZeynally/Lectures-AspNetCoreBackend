namespace Lecture13.Auth.JWT.Access.Options
{
    public class JwtOptions
    {
        public required string Issuer { get; init; }

        public required string Audience { get; init; }

        public required string Key { get; init; }
    }
}
