using Lecture07.CatsApi.Api.Posts.Contracts;
using Lecture07.CatsDatabase.DataAccess.Models.Cats;
using Lecture07.CatsDatabase.DataAccess.Repository.Cats;

namespace Lecture07.CatsApi.Api.Posts.Services
{
    public sealed class PostService(IPostsRepository repository, ILogger<PostService> logger) : IPostService
    {
        public async Task<PostResponseContract> CreateAsync(CreatePostRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating post in service. UserUid: {UserUid}, Title: {Title}, CatsCount: {CatsCount}.", request.UserUid, request.Title, request.CatUids.Count);

            var post = await repository.Create(request.UserUid, request.Title, request.Description, request.PhotoUrl, request.CatUids, cancellationToken);

            logger.LogInformation("Successfully created post in service with uid: {PostUid}.", post.Uid);

            return Map(post);
        }

        public async Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting post in service by uid: {PostUid}.", uid);

            var deleted = await repository.Delete(uid, cancellationToken);

            logger.LogInformation(deleted ? "Successfully deleted post in service by uid: {PostUid}." : "Post with uid {PostUid} was not found for deletion in service.", uid);

            return deleted;
        }

        public async Task<GetPostsResponseContract> GetAsync(GetPostsRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting posts in service. Page: {Page}, PageSize: {PageSize}, UserUid: {UserUid}, CatUid: {CatUid}, Title: {Title}.", request.Page, request.PageSize, request.UserUid, request.CatUid, request.Title);

            var result = await repository.Get((request.Page, request.PageSize, request.UserUid, request.CatUid, request.Title), cancellationToken);

            var response = new GetPostsResponseContract
            {
                Items = result.Items.Select(Map).ToArray(),
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = result.TotalCount
            };

            logger.LogInformation("Successfully retrieved posts in service. Returned: {ReturnedCount}, TotalCount: {TotalCount}.", response.Items.Count, response.TotalCount);

            return response;
        }

        public async Task<PostResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting post by uid in service: {PostUid}.", uid);

            var post = await repository.GetByUid(uid, cancellationToken);

            if (post is null)
            {
                logger.LogWarning("Post with uid {PostUid} was not found in service.", uid);
                return null;
            }

            logger.LogInformation("Successfully retrieved post by uid in service: {PostUid}.", uid);

            return Map(post);
        }

        public async Task<PostResponseContract?> UpdateAsync(Guid uid, UpdatePostRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating post in service. PostUid: {PostUid}, Title: {Title}, CatsCount: {CatsCount}.", uid, request.Title, request.CatUids.Count);

            var post = await repository.Update(uid, request.Title, request.Description, request.PhotoUrl, request.CatUids, cancellationToken);

            if (post is null)
            {
                logger.LogWarning("Post with uid {PostUid} was not found for update in service.", uid);
                return null;
            }

            logger.LogInformation("Successfully updated post in service with uid: {PostUid}.", uid);

            return Map(post);
        }

        private static PostResponseContract Map(Post post)
        {
            return new PostResponseContract
            {
                Uid = post.Uid,
                UserUid = post.UserUid,
                Title = post.Title,
                Description = post.Description,
                PhotoUrl = post.PhotoUrl,
                CatUids = post.CatUids,
                CreatedAt = post.CreatedAt,
            };
        }
    }
}