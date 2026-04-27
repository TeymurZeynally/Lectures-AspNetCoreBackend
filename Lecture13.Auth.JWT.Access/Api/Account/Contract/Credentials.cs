namespace Lecture13.Auth.JWT.Access.Api.Account.Contract
{
    public class Credentials
    {
        public required string Login { get; set; }

        public required string Password { get; set; }
    }
}
