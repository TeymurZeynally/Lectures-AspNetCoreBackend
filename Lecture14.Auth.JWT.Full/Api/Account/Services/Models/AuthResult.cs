namespace Lecture14.Auth.JWT.Full.Api.Account.Services.Models
{
    public sealed record AuthResult(
        bool Succeeded,
        TokenPair? Tokens,
        string? Error)
    {
        public static AuthResult Success(TokenPair tokens)
        {
            return new AuthResult(true, tokens, null);
        }

        public static AuthResult Failure(string error)
        {
            return new AuthResult(false, null, error);
        }
    }
}
