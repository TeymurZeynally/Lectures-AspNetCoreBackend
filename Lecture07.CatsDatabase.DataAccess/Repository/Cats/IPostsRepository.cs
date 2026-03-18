using Lecture07.CatsDatabase.DataAccess.Models.Cats;

namespace Lecture07.CatsDatabase.DataAccess.Repository.Cats
{
    public interface IPostsRepository
    {
        Task<(Post[] Items, int TotalCount)> Get((int Page, int PageSize, Guid? UserUid, Guid? CatUid, string? Title) filter, CancellationToken token);

        Task<Post?> GetByUid(Guid uid, CancellationToken token);

        Task<Post> Create(Guid userUid, string title, string? description, string photoUrl, IReadOnlyCollection<Guid> catUids, CancellationToken token);

        Task<Post?> Update(Guid uid, string title, string? description, string photoUrl, IReadOnlyCollection<Guid> catUids, CancellationToken token);

        Task<bool> Delete(Guid uid, CancellationToken token);
    }
}
