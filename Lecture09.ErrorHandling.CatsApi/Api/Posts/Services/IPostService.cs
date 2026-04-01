using Lecture09.ErrorHandling.CatsApi.Api.Posts.Contracts;

namespace Lecture09.ErrorHandling.CatsApi.Api.Posts.Services
{
	public interface IPostService
	{
		Task<GetPostsResponseContract> GetAsync(GetPostsRequestContract request, CancellationToken cancellationToken);

		Task<PostResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken);

		Task<PostResponseContract> CreateAsync(CreatePostRequestContract request, CancellationToken cancellationToken);

		Task<PostResponseContract?> UpdateAsync(Guid uid, UpdatePostRequestContract request, CancellationToken cancellationToken);

		Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken);
	}
}