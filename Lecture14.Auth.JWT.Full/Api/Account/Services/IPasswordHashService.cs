namespace Lecture14.Auth.JWT.Full.Api.Account.Services
{
    internal interface IPasswordHashService
    {
        (byte[] HashBytes, byte[] SaltBytes) HashPassword(string password);

        bool VerifyPassword(string password, byte[] hashBytes, byte[] saltBytes);
    }
}
