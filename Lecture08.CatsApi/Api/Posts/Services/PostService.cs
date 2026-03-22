using Lecture08.CatsApi.Api.Posts.Contracts;

namespace Lecture08.CatsApi.Api.Posts.Services
{
    public sealed class PostService(ILogger<PostService> logger) : IPostService
    {
        public async Task<PostResponseContract> CreateAsync(CreatePostRequestContract request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<GetPostsResponseContract> GetAsync(GetPostsRequestContract request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<PostResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<PostResponseContract?> UpdateAsync(Guid uid, UpdatePostRequestContract request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}