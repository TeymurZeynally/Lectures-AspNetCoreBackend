using Lecture13.Auth.JWT.Full.Api.Account.Contract;
using Lecture13.Auth.JWT.Full.Api.Account.Services.Models;

namespace Lecture13.Auth.JWT.Full.Api.Account.Services
{
    public interface IUserService
    {
        Task<AuthResult> Register(CredentialsRequest credentialsRequest, CancellationToken cancellationToken);

        Task<AuthResult> Login(CredentialsRequest credentialsRequest, CancellationToken cancellationToken);
    }
}
