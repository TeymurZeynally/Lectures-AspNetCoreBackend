using Lecture07.CatsApi.Api.Posts.Contracts;

namespace Lecture07.CatsApi.Api.Posts.Services
{
	public sealed class PostService : IPostService
	{
		public Task<PostResponseContract> CreateAsync(CreatePostRequestContract request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<GetPostsResponseContract> GetAsync(GetPostsRequestContract request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<PostResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<PostResponseContract?> UpdateAsync(Guid uid, UpdatePostRequestContract request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}