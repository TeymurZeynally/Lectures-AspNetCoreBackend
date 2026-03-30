using Lecture09.CatsApi.Api.Cats.Contracts;
using Lecture09.CatsApi.Api.Cats.Services;
using Lecture09.CatsApi.Api.Cats.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Lecture09.CatsApi.Api.Cats.Controllers
{
	[ApiController]
	[Route("api/cats")]
	public sealed class CatsController(
		ICatService catService,
		ILogger<CatsController> logger) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<CatResponseContract[]>> GetAll(CancellationToken cancellationToken)
		{
			logger.LogInformation("Getting all cats.");

			var cats = await catService.GetAllAsync(cancellationToken);

			logger.LogInformation("Successfully retrieved {CatsCount} cats.", cats.Count);

			return Ok(cats);
		}

		[HttpGet("{uid:guid}")]
		public async Task<ActionResult<CatResponseContract>> GetByUid([FromRoute] Guid uid, CancellationToken cancellationToken)
		{
			logger.LogInformation("Getting cat by uid: {CatUid}.", uid);

			var cat = await catService.GetByUidAsync(uid, cancellationToken);

			if (cat is null)
			{
				logger.LogWarning("Cat with uid {CatUid} was not found.", uid);
				return NotFound();
			}

			logger.LogInformation("Successfully retrieved cat with uid: {CatUid}.", uid);

			return Ok(cat);
		}

		[HttpGet("search")]
		public async Task<ActionResult<CatResponseContract[]>> SearchByName(
			[FromQuery][Required] string name,
			CancellationToken cancellationToken)
		{
			logger.LogInformation("Searching cats by name: {CatName}.", name);

			var cats = await catService.SearchByNameAsync(name, cancellationToken);

			logger.LogInformation("Search by name '{CatName}' returned {CatsCount} cats.", name, cats.Count);

			return Ok(cats);
		}

		[HttpPost]
		public async Task<ActionResult<CatResponseContract>> Create(
			[FromBody] CreateCatRequestContract request,
			CancellationToken cancellationToken)
		{
			logger.LogInformation("Creating cat with name: {CatName}, breed: {Breed}, age: {Age}, user uid: {UserUid}.", request.Name, request.Breed, request.Age, request.UserUid);

			var createdCat = await catService.CreateAsync(request, cancellationToken);

			logger.LogInformation("Successfully created cat with uid: {CatUid}, name: {CatName}.", createdCat.Uid, createdCat.Name);

			return CreatedAtAction(nameof(GetByUid), new { uid = createdCat.Uid }, createdCat);
		}

		[HttpPut("{uid:guid}")]
		public async Task<ActionResult<CatResponseContract>> Update(
			[FromRoute] Guid uid,
			[FromBody] UpdateCatRequestContract request,
			CancellationToken cancellationToken)
		{
			logger.LogInformation("Updating cat with uid: {CatUid}. New name: {CatName}, new breed: {Breed}, new age: {Age}.", uid, request.Name, request.Breed, request.Age);

			var updatedCat = await catService.UpdateAsync(uid, request, cancellationToken);

			if (updatedCat is null)
			{
				logger.LogWarning("Cat with uid {CatUid} was not found for update.", uid);
				return NotFound();
			}

			logger.LogInformation("Successfully updated cat with uid: {CatUid}.", uid);

			return Ok(updatedCat);
		}

		[HttpDelete("{uid:guid}")]
		public async Task<IActionResult> Delete([FromRoute] Guid uid, CancellationToken cancellationToken)
		{
			logger.LogInformation("Deleting cat with uid: {CatUid}.", uid);

			var deleted = await catService.DeleteAsync(uid, cancellationToken);

			if (!deleted)
			{
				logger.LogWarning("Cat with uid {CatUid} was not found for deletion.", uid);
				return NotFound();
			}

			logger.LogInformation("Successfully deleted cat with uid: {CatUid}.", uid);

			return NoContent();
		}
	}
}