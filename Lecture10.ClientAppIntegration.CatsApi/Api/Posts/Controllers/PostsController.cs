using Lecture10.ClientAppIntegration.CatsApi.Api.Posts.Contracts;
using Lecture10.ClientAppIntegration.CatsApi.Api.Posts.Exceptions;
using Lecture10.ClientAppIntegration.CatsApi.Api.Posts.Filters;
using Lecture10.ClientAppIntegration.CatsApi.Api.Posts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lecture10.ClientAppIntegration.CatsApi.Api.Posts.Controllers
{
	[ApiController]
	[Route("api/posts")]
    [PostExceptionFilter]
    public sealed class PostsController(
		IPostService postService,
		ILogger<PostsController> logger) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<GetPostsResponseContract>> Get(
			[FromQuery] GetPostsRequestContract request,
			CancellationToken cancellationToken)
		{
			logger.LogInformation("Getting posts. Page: {Page}, PageSize: {PageSize}, UserUid: {UserUid}, CatUid: {CatUid}, Title: {Title}.", request.Page, request.PageSize, request.UserUid, request.CatUid, request.Title);

			var response = await postService.GetAsync(request, cancellationToken);

			logger.LogInformation("Successfully retrieved posts. Returned: {ReturnedCount}, TotalCount: {TotalCount}, Page: {Page}, PageSize: {PageSize}.", response.Items.Count, response.TotalCount, response.Page, response.PageSize);

			return Ok(response);
		}

		[HttpGet("{uid:guid}")]
		public async Task<ActionResult<PostResponseContract>> GetByUid([FromRoute] Guid uid, CancellationToken cancellationToken)
		{
			logger.LogInformation("Getting post by uid: {PostUid}.", uid);

			var post = await postService.GetByUidAsync(uid, cancellationToken);

			if (post is null)
			{
				logger.LogWarning("Post with uid {PostUid} was not found.", uid);
				return NotFound();
			}

			logger.LogInformation("Successfully retrieved post with uid: {PostUid}.", uid);

			return Ok(post);
		}

		[HttpPost]
		[PostExceptionFilter]
		public async Task<ActionResult<PostResponseContract>> Create(
			[FromBody] CreatePostRequestContract request,
			CancellationToken cancellationToken)
		{
			logger.LogInformation("Creating post with title: {Title}, user uid: {UserUid}, cats count: {CatsCount}.", request.Title, request.UserUid, request.CatUids.Count);

			var createdPost = await postService.CreateAsync(request, cancellationToken);

			logger.LogInformation("Successfully created post with uid: {PostUid}, title: {Title}.", createdPost.Uid, createdPost.Title);

			return CreatedAtAction(nameof(GetByUid), new { uid = createdPost.Uid }, createdPost);
		}

		[HttpPut("{uid:guid}")]
        [PostExceptionFilter]
        public async Task<ActionResult<PostResponseContract>> Update(
			[FromRoute] Guid uid,
			[FromBody] UpdatePostRequestContract request,
			CancellationToken cancellationToken)
		{
			logger.LogInformation("Updating post with uid: {PostUid}. New title: {Title}, cats count: {CatsCount}.", uid, request.Title, request.CatUids.Count);

			var updatedPost = await postService.UpdateAsync(uid, request, cancellationToken);

			if (updatedPost is null)
			{
				logger.LogWarning("Post with uid {PostUid} was not found for update.", uid);
				return NotFound();
			}

			logger.LogInformation("Successfully updated post with uid: {PostUid}.", uid);

			return Ok(updatedPost);
		}

		[HttpDelete("{uid:guid}")]
		public async Task<IActionResult> Delete([FromRoute] Guid uid, CancellationToken cancellationToken)
		{
			logger.LogInformation("Deleting post with uid: {PostUid}.", uid);

			var deleted = await postService.DeleteAsync(uid, cancellationToken);

			if (!deleted)
			{
				logger.LogWarning("Post with uid {PostUid} was not found for deletion.", uid);
				return NotFound();
			}

			logger.LogInformation("Successfully deleted post with uid: {PostUid}.", uid);

			return NoContent();
		}
	}
}