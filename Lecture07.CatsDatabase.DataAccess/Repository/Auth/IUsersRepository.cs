using Lecture07.CatsDatabase.DataAccess.Models.Auth;

namespace Lecture07.CatsDatabase.DataAccess.Repository.Auth
{
    public interface IUsersRepository
    {
        Task<User[]> GetAll(CancellationToken token);

        Task<User?> GetByUid(Guid uid, CancellationToken token);

        Task<User> Create(string username, string email, string password, CancellationToken token);

        Task<User?> Update(Guid uid, string username, string email, CancellationToken token);

        Task<bool> Delete(Guid uid, CancellationToken token);
    }
}
