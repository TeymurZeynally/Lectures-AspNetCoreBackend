namespace Lecture13.Auth.JWT.Full.Api.Account.Contract
{
    public class CredentialsRequest
    {
        public required string Login { get; init; }

        public required string Password { get; init; }
    }
}
