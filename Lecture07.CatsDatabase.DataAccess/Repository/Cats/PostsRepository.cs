using Dapper;
using Lecture07.CatsDatabase.DataAccess.Models.Cats;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Lecture07.CatsDatabase.DataAccess.Repository.Cats
{
    public sealed class PostsRepository(string connectionString, ILogger<PostsRepository> logger) : IPostsRepository
    {
        public async Task<(Post[] Items, int TotalCount)> Get((int Page, int PageSize, Guid? UserUid, Guid? CatUid, string? Title) filter, CancellationToken token)
        {
            const string sql = """
                WITH filtered_posts AS
                (
                    SELECT
                        p.id,
                        p.uid,
                        p.user_id,
                        u.uid AS useruid,
                        p.title,
                        p.description,
                        p.photo_url AS photourl,
                        p.created_at AS createdat
                    FROM cats.posts p
                    INNER JOIN auth.users u ON u.id = p.user_id
                    WHERE (@UserUid IS NULL OR u.uid = @UserUid)
                        AND (@Title IS NULL OR p.title ILIKE @TitlePattern)
                        AND (
                                @CatUid IS NULL OR EXISTS
                                (
                                    SELECT 1
                                    FROM cats.posts_cats pc
                                    INNER JOIN cats.cats c ON c.id = pc.cat_id
                                    WHERE pc.post_id = p.id
                                        AND c.uid = @CatUid
                                )
                            )
                )
                SELECT COUNT(*)
                FROM filtered_posts;

                WITH filtered_posts AS
                (
                    SELECT
                        p.id,
                        p.uid,
                        p.user_id,
                        u.uid AS useruid,
                        p.title,
                        p.description,
                        p.photo_url AS photourl,
                        p.created_at AS createdat
                    FROM cats.posts p
                    INNER JOIN auth.users u ON u.id = p.user_id
                    WHERE (@UserUid IS NULL OR u.uid = @UserUid)
                        AND (@Title IS NULL OR p.title ILIKE @TitlePattern)
                        AND (
                                @CatUid IS NULL OR EXISTS
                                (
                                    SELECT 1
                                    FROM cats.posts_cats pc
                                    INNER JOIN cats.cats c ON c.id = pc.cat_id
                                    WHERE pc.post_id = p.id
                                        AND c.uid = @CatUid
                                )
                            )
                ),
                paged_posts AS
                (
                    SELECT *
                    FROM filtered_posts
                    ORDER BY createdat DESC, id DESC
                    LIMIT @PageSize
                    OFFSET @Offset
                )
                SELECT
                    pp.id,
                    pp.uid,
                    pp.user_id,
                    pp.useruid,
                    pp.title,
                    pp.description,
                    pp.photourl,
                    pp.createdat,
                    c.uid AS catuid
                FROM paged_posts pp
                LEFT JOIN cats.posts_cats pc ON pc.post_id = pp.id
                LEFT JOIN cats.cats c ON c.id = pc.cat_id
                ORDER BY pp.createdat DESC, pp.id DESC;
                """;

            logger.LogInformation("Executing query to get posts. Page: {Page}, PageSize: {PageSize}, UserUid: {UserUid}, CatUid: {CatUid}, Title: {Title}.", filter.Page, filter.PageSize, filter.UserUid, filter.CatUid, filter.Title);

            await using var connection = new NpgsqlConnection(connectionString);

            var command = new CommandDefinition(
                sql,
                new
                {
                    filter.UserUid,
                    filter.CatUid,
                    filter.Title,
                    TitlePattern = filter.Title is null ? null : $"%{filter.Title}%",
                    Offset = (filter.Page - 1) * filter.PageSize,
                    filter.PageSize
                },
                cancellationToken: token);

            using var multi = await connection.QueryMultipleAsync(command);

            var totalCount = await multi.ReadSingleAsync<int>();
            var rows = (await multi.ReadAsync<(long Id, Guid Uid, long UserId, Guid UserUid, string Title, string? Description, string PhotoUrl, DateTimeOffset CreatedAt, Guid? CatUid)>()).ToArray();

            var posts = rows
                .GroupBy(x => new { x.Id, x.Uid, x.UserId, x.UserUid, x.Title, x.Description, x.PhotoUrl, x.CreatedAt })
                .Select(g => new Post
                {
                    Id = g.Key.Id,
                    Uid = g.Key.Uid,
                    UserId = g.Key.UserId,
                    UserUid = g.Key.UserUid,
                    Title = g.Key.Title,
                    Description = g.Key.Description,
                    PhotoUrl = g.Key.PhotoUrl,
                    CreatedAt = g.Key.CreatedAt,
                    CatUids = g
                        .Where(x => x.CatUid.HasValue)
                        .Select(x => x.CatUid!.Value)
                        .Distinct()
                        .ToArray()
                })
                .ToArray();

            logger.LogInformation("Get posts query returned {PostsCount} rows with total count {TotalCount}.", posts.Length, totalCount);

            return (posts, totalCount);
        }

        public async Task<Post?> GetByUid(Guid uid, CancellationToken token)
        {
            const string sql = """
                SELECT
                    p.id,
                    p.uid,
                    p.user_id,
                    u.uid AS useruid,
                    p.title,
                    p.description,
                    p.photo_url AS photourl,
                    p.created_at AS createdat,
                    c.uid AS catuid
                FROM cats.posts p
                INNER JOIN auth.users u ON u.id = p.user_id
                LEFT JOIN cats.posts_cats pc ON pc.post_id = p.id
                LEFT JOIN cats.cats c ON c.id = pc.cat_id
                WHERE p.uid = @Uid
                ORDER BY p.id;
                """;

            logger.LogInformation("Executing query to get post by uid: {PostUid}.", uid);

            await using var connection = new NpgsqlConnection(connectionString);

            var command = new CommandDefinition(sql, new { Uid = uid }, cancellationToken: token);

            var rows = (await connection.QueryAsync<(long Id, Guid Uid, long UserId, Guid UserUid, string Title, string? Description, string PhotoUrl, DateTimeOffset CreatedAt, Guid? CatUid)>(command)).ToArray();

            if (rows.Length == 0)
            {
                logger.LogInformation("Post with uid {PostUid} was not found in repository.", uid);
                return null;
            }

            var first = rows[0];

            var post = new Post
            {
                Id = first.Id,
                Uid = first.Uid,
                UserId = first.UserId,
                UserUid = first.UserUid,
                Title = first.Title,
                Description = first.Description,
                PhotoUrl = first.PhotoUrl,
                CreatedAt = first.CreatedAt,
                CatUids = rows
                    .Where(x => x.CatUid.HasValue)
                    .Select(x => x.CatUid!.Value)
                    .Distinct()
                    .ToArray()
            };

            logger.LogInformation("Post with uid {PostUid} was found in repository.", uid);

            return post;
        }

        public async Task<Post> Create(Guid userUid, string title, string? description, string photoUrl, IReadOnlyCollection<Guid> catUids, CancellationToken token)
        {
            logger.LogInformation("Executing transaction to create post. UserUid: {UserUid}, Title: {Title}, CatsCount: {CatsCount}.", userUid, title, catUids.Count);

            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync(token);
            await using var transaction = await connection.BeginTransactionAsync(token);

            const string insertPostSql = """
                INSERT INTO cats.posts (user_id, title, description, photo_url)
                SELECT u.id, @Title, @Description, @PhotoUrl
                FROM auth.users u
                WHERE u.uid = @UserUid
                RETURNING id, uid, user_id;
                """;

            var insertPostCommand = new CommandDefinition(
                insertPostSql,
                new { UserUid = userUid, Title = title, Description = description, PhotoUrl = photoUrl },
                transaction: transaction,
                cancellationToken: token);

            var createdPostRow = await connection.QuerySingleOrDefaultAsync<(long Id, Guid Uid, long UserId)?>(insertPostCommand);

            if (createdPostRow is null)
            {
                logger.LogWarning("Failed to create post because user with uid {UserUid} was not found.", userUid);
                throw new InvalidOperationException($"User with uid '{userUid}' was not found.");
            }

            const string insertPostCatSql = """
                INSERT INTO cats.posts_cats (post_id, cat_id)
                SELECT @PostId, c.id
                FROM cats.cats c
                WHERE c.uid = ANY(@CatUids);
                """;

            var insertPostCatCommand = new CommandDefinition(
                insertPostCatSql,
                new { PostId = createdPostRow.Value.Id, CatUids = catUids.ToArray() },
                transaction: transaction,
                cancellationToken: token);

            var insertedRelations = await connection.ExecuteAsync(insertPostCatCommand);

            if (insertedRelations != catUids.Count)
            {
                await transaction.RollbackAsync(token);
                logger.LogWarning("Failed to create post {PostUid} because not all cat uids were found. Expected: {Expected}, Actual: {Actual}.", createdPostRow.Value.Uid, catUids.Count, insertedRelations);

                throw new InvalidOperationException("One or more cat uids were not found.");
            }

            await transaction.CommitAsync(token);

            var post = await GetByUid(createdPostRow.Value.Uid, token);

            logger.LogInformation("Successfully created post with uid: {PostUid}.", createdPostRow.Value.Uid);

            return post!;
        }

        public async Task<Post?> Update(Guid uid, string title, string? description, string photoUrl, IReadOnlyCollection<Guid> catUids, CancellationToken token)
        {
            logger.LogInformation("Executing transaction to update post. PostUid: {PostUid}, Title: {Title}, CatsCount: {CatsCount}.", uid, title, catUids.Count);

            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync(token);
            await using var transaction = await connection.BeginTransactionAsync(token);

            const string updatePostSql = """
                UPDATE cats.posts
                SET title = @Title, description = @Description, photo_url = @PhotoUrl
                WHERE uid = @Uid
                RETURNING id;
                """;

            var updatePostCommand = new CommandDefinition(
                updatePostSql,
                new { Uid = uid, Title = title, Description = description, PhotoUrl = photoUrl },
                transaction: transaction,
                cancellationToken: token);

            var postId = await connection.QuerySingleOrDefaultAsync<long?>(updatePostCommand);

            if (postId is null)
            {
                await transaction.RollbackAsync(token);
                logger.LogInformation("Post with uid {PostUid} was not found for update in repository.", uid);
                return null;
            }

            const string deleteRelationsSql = """
                DELETE FROM cats.posts_cats
                WHERE post_id = @PostId;
                """;

            var deleteRelationsCommand = new CommandDefinition(
                deleteRelationsSql,
                new { PostId = postId.Value },
                transaction: transaction,
                cancellationToken: token);

            await connection.ExecuteAsync(deleteRelationsCommand);

            const string insertRelationsSql = """
                INSERT INTO cats.posts_cats (post_id, cat_id)
                SELECT @PostId, c.id
                FROM cats.cats c
                WHERE c.uid = ANY(@CatUids);
                """;

            var insertRelationsCommand = new CommandDefinition(
                insertRelationsSql,
                new
                { PostId = postId.Value, CatUids = catUids.ToArray() },
                transaction: transaction,
                cancellationToken: token);

            var insertedRelations = await connection.ExecuteAsync(insertRelationsCommand);

            if (insertedRelations != catUids.Count)
            {
                await transaction.RollbackAsync(token);
                logger.LogWarning("Failed to update post {PostUid} because not all cat uids were found. Expected: {Expected}, Actual: {Actual}.", uid, catUids.Count, insertedRelations);

                throw new InvalidOperationException("One or more cat uids were not found.");
            }

            await transaction.CommitAsync(token);

            var post = await GetByUid(uid, token);

            logger.LogInformation("Successfully updated post with uid: {PostUid}.", uid);

            return post;
        }

        public async Task<bool> Delete(Guid uid, CancellationToken token)
        {
            logger.LogInformation("Executing transaction to delete post by uid: {PostUid}.", uid);

            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync(token);
            await using var transaction = await connection.BeginTransactionAsync(token);

            const string getPostIdSql = """
                SELECT id
                FROM cats.posts
                WHERE uid = @Uid
                LIMIT 1;
                """;

            var getPostIdCommand = new CommandDefinition(
                getPostIdSql,
                new { Uid = uid },
                transaction: transaction,
                cancellationToken: token);

            var postId = await connection.QuerySingleOrDefaultAsync<long?>(getPostIdCommand);

            if (postId is null)
            {
                await transaction.RollbackAsync(token);
                logger.LogInformation("Post with uid {PostUid} was not found for deletion in repository.", uid);
                return false;
            }

            const string deleteRelationsSql = """
                DELETE FROM cats.posts_cats
                WHERE post_id = @PostId;
                """;

            var deleteRelationsCommand = new CommandDefinition(
                deleteRelationsSql,
                new { PostId = postId.Value },
                transaction: transaction,
                cancellationToken: token);

            await connection.ExecuteAsync(deleteRelationsCommand);

            const string deletePostSql = """
                DELETE FROM cats.posts
                WHERE id = @PostId;
                """;

            var deletePostCommand = new CommandDefinition(
                deletePostSql,
                new { PostId = postId.Value },
                transaction: transaction,
                cancellationToken: token);

            var affectedRows = await connection.ExecuteAsync(deletePostCommand);

            await transaction.CommitAsync(token);

            logger.LogInformation("Delete post by uid {PostUid} affected {AffectedRows} rows.", uid, affectedRows);

            return affectedRows > 0;
        }
    }
}
