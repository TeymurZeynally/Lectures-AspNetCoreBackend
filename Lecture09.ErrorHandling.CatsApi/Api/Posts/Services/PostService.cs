using Lecture09.ErrorHandling.CatsApi.Api.Posts.Contracts;
using Lecture09.ErrorHandling.CatsApi.Api.Posts.Exceptions;
using Lecture09.ErrorHandling.CatsDatabase.DataAccess.EF;
using Lecture09.ErrorHandling.CatsDatabase.DataAccess.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lecture09.ErrorHandling.CatsApi.Api.Posts.Services
{
    public sealed class PostService(CatsDbContext dbContext, ILogger<PostService> logger) : IPostService
    {
        public async Task<PostResponseContract> CreateAsync(CreatePostRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting post creation. UserUid: {UserUid}, Title: {Title}, CatsCount: {CatsCount}, PhotoUrl: {PhotoUrl}.", request.UserUid, request.Title, request.CatUids.Count, request.PhotoUrl);

            var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Uid == request.UserUid, cancellationToken);
            if (user is null)
            {
                logger.LogWarning("Post creation failed because user was not found. UserUid: {UserUid}, Title: {Title}.", request.UserUid, request.Title);
                throw new PostUserNotFoundException($"User with uid '{request.UserUid}' was not found.");
            }

            var distinctCatUids = request.CatUids.Distinct().ToArray();

            var cats = await dbContext.Cats.Where(x => distinctCatUids.Contains(x.Uid)).ToListAsync(cancellationToken);

            if (cats.Count != distinctCatUids.Length)
            {
                var foundCatUids = cats.Select(x => x.Uid).ToHashSet();
                var missingCatUids = distinctCatUids.Where(x => !foundCatUids.Contains(x)).ToArray();

                logger.LogWarning("Post creation failed because some cats were not found. Title: {Title}, MissingCatUids: {MissingCatUids}.", request.Title, missingCatUids);
                throw new PostCatsNotFoundException($"Some cats were not found: {string.Join(", ", missingCatUids)}");
            }

            var post = new Post
            {
                UserId = user.Id,
                Title = request.Title,
                Description = request.Description,
                PhotoUrl = request.PhotoUrl,
                Cats = cats
            };

            await dbContext.Posts.AddAsync(post, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Post entity persisted successfully. PostId: {PostId}, PostUid: {PostUid}, UserId: {UserId}, CatsCount: {CatsCount}.", post.Id, post.Uid, user.Id, cats.Count);

            var response = new PostResponseContract
            {
                Uid = post.Uid,
                UserUid = user.Uid,
                Title = post.Title,
                Description = post.Description,
                PhotoUrl = post.PhotoUrl,
                CatUids = cats.Select(x => x.Uid).ToArray(),
                CreatedAt = post.CreatedAt
            };

            logger.LogInformation("Post creation completed. PostUid: {PostUid}, Title: {Title}, UserUid: {UserUid}, CatsCount: {CatsCount}, CreatedAt: {CreatedAt}.", response.Uid, response.Title, response.UserUid, response.CatUids.Count, response.CreatedAt);

            return response;
        }

        public async Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting post deletion. PostUid: {PostUid}.", uid);

            var post = await dbContext.Posts.Include(x => x.Cats).SingleOrDefaultAsync(x => x.Uid == uid, cancellationToken);

            if (post is null)
            {
                logger.LogWarning("Post deletion skipped because post was not found. PostUid: {PostUid}.", uid);
                return false;
            }

            logger.LogInformation("Post found for deletion. PostUid: {PostUid}, PostId: {PostId}, Title: {Title}, RelatedCatsCount: {RelatedCatsCount}.", post.Uid, post.Id, post.Title, post.Cats.Count);

            post.Cats.Clear();
            dbContext.Posts.Remove(post);

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Post deleted successfully. PostUid: {PostUid}, PostId: {PostId}, Title: {Title}.", post.Uid, post.Id, post.Title);

            return true;
        }

        public async Task<GetPostsResponseContract> GetAsync(GetPostsRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Loading posts with filters. Page: {Page}, PageSize: {PageSize}, UserUid: {UserUid}, CatUid: {CatUid}, Title: {Title}.", request.Page, request.PageSize, request.UserUid, request.CatUid, request.Title);

            var query = dbContext.Posts
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Cats)
                .AsQueryable();

            if (request.UserUid.HasValue)
            {
                query = query.Where(x => x.User.Uid == request.UserUid.Value);
            }

            if (request.CatUid.HasValue)
            {
                query = query.Where(x => x.Cats.Any(c => c.Uid == request.CatUid.Value));
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                var normalizedTitle = request.Title.Trim();
                query = query.Where(x => EF.Functions.ILike(x.Title, $"%{normalizedTitle}%"));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PostResponseContract
                {
                    Uid = x.Uid,
                    UserUid = x.User.Uid,
                    Title = x.Title,
                    Description = x.Description,
                    PhotoUrl = x.PhotoUrl,
                    CatUids = x.Cats.Select(c => c.Uid).ToArray(),
                    CreatedAt = x.CreatedAt
                })
                .ToArrayAsync(cancellationToken);

            logger.LogInformation("Posts loaded successfully. ReturnedCount: {ReturnedCount}, TotalCount: {TotalCount}, Page: {Page}, PageSize: {PageSize}.", items.Length, totalCount, request.Page, request.PageSize);

            return new GetPostsResponseContract
            {
                Items = items,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PostResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken)
        {
            logger.LogInformation("Loading post by uid. PostUid: {PostUid}.", uid);

            var post = await dbContext.Posts
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Cats)
                .Where(x => x.Uid == uid)
                .Select(x => new PostResponseContract
                {
                    Uid = x.Uid,
                    UserUid = x.User.Uid,
                    Title = x.Title,
                    Description = x.Description,
                    PhotoUrl = x.PhotoUrl,
                    CatUids = x.Cats.Select(c => c.Uid).ToArray(),
                    CreatedAt = x.CreatedAt
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (post is null)
            {
                logger.LogWarning("Post was not found by uid. PostUid: {PostUid}.", uid);
                return null;
            }

            logger.LogInformation("Post loaded successfully by uid. PostUid: {PostUid}, UserUid: {UserUid}, Title: {Title}, CatsCount: {CatsCount}.", post.Uid, post.UserUid, post.Title, post.CatUids.Count);

            return post;
        }

        public async Task<PostResponseContract?> UpdateAsync(Guid uid, UpdatePostRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting post update. PostUid: {PostUid}, Title: {Title}, CatsCount: {CatsCount}, PhotoUrl: {PhotoUrl}.", uid, request.Title, request.CatUids.Count, request.PhotoUrl);

            var post = await dbContext.Posts.Include(x => x.User).Include(x => x.Cats).SingleOrDefaultAsync(x => x.Uid == uid, cancellationToken);

            if (post is null)
            {
                logger.LogWarning("Post update skipped because post was not found. PostUid: {PostUid}.", uid);
                return null;
            }

            var distinctCatUids = request.CatUids.Distinct().ToArray();

            var cats = await dbContext.Cats.Where(x => distinctCatUids.Contains(x.Uid)).ToListAsync(cancellationToken);

            if (cats.Count != distinctCatUids.Length)
            {
                var foundCatUids = cats.Select(x => x.Uid).ToHashSet();
                var missingCatUids = distinctCatUids.Where(x => !foundCatUids.Contains(x)).ToArray();

                logger.LogWarning("Post update failed because some cats were not found. PostUid: {PostUid}, MissingCatUids: {MissingCatUids}.", uid, missingCatUids);
                throw new InvalidOperationException($"Some cats were not found: {string.Join(", ", missingCatUids)}");
            }

            logger.LogInformation("Current post state before update. PostUid: {PostUid}, CurrentTitle: {CurrentTitle}, CurrentCatsCount: {CurrentCatsCount}.", post.Uid, post.Title, post.Cats.Count);

            post.Title = request.Title;
            post.Description = request.Description;
            post.PhotoUrl = request.PhotoUrl;
            post.Cats.Clear();

            foreach (var cat in cats)
            {
                post.Cats.Add(cat);
            }

            dbContext.Posts.Update(post);
            await dbContext.SaveChangesAsync(cancellationToken);

            var response = new PostResponseContract
            {
                Uid = post.Uid,
                UserUid = post.User.Uid,
                Title = post.Title,
                Description = post.Description,
                PhotoUrl = post.PhotoUrl,
                CatUids = post.Cats.Select(x => x.Uid).ToArray(),
                CreatedAt = post.CreatedAt
            };

            logger.LogInformation("Post updated successfully. PostUid: {PostUid}, Title: {Title}, UserUid: {UserUid}, CatsCount: {CatsCount}.", response.Uid, response.Title, response.UserUid, response.CatUids.Count);

            return response;
        }
    }
}