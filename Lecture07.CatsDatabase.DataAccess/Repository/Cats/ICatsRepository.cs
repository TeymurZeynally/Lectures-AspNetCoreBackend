using Lecture07.CatsDatabase.DataAccess.Models.Cats;

namespace Lecture07.CatsDatabase.DataAccess.Repository.Cats
{
    public interface ICatsRepository
    {
        Task<Cat[]> GetAll(CancellationToken token);

        Task<Cat?> GetByUid(Guid uid, CancellationToken token);

        Task<Cat[]> SearchByName(string name, CancellationToken token);

        Task<Cat> Create(Guid userUid, string name, string breed, int age, CancellationToken token);

        Task<Cat?> Update(Guid uid, string name, string breed, int age, CancellationToken token);

        Task<bool> Delete(Guid uid, CancellationToken token);
    }
}