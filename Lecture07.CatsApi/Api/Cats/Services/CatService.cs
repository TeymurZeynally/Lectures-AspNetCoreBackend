using Lecture07.CatsApi.Api.Cats.Contracts;
using Lecture07.CatsDatabase.DataAccess.Models.Cats;
using Lecture07.CatsDatabase.DataAccess.Repository.Cats;

namespace Lecture07.CatsApi.Api.Cats.Services
{
    public sealed class CatService(ICatsRepository repository, ILogger<CatService> logger) : ICatService
    {
        public async Task<CatResponseContract> CreateAsync(CreateCatRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating cat in service. UserUid: {UserUid}, Name: {CatName}, Breed: {Breed}, Age: {Age}.", request.UserUid, request.Name, request.Breed, request.Age);

            var cat = await repository.Create(request.UserUid, request.Name, request.Breed, request.Age, cancellationToken);

            logger.LogInformation("Successfully created cat in service with uid: {CatUid}.", cat.Uid);

            return Map(cat);
        }

        public async Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting cat in service by uid: {CatUid}.", uid);

            var deleted = await repository.Delete(uid, cancellationToken);

            logger.LogInformation(deleted ? "Successfully deleted cat in service by uid: {CatUid}." : "Cat with uid {CatUid} was not found for deletion in service.", uid);

            return deleted;
        }

        public async Task<IReadOnlyCollection<CatResponseContract>> GetAllAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all cats in service.");

            var cats = await repository.GetAll(cancellationToken);

            var result = cats.Select(Map).ToArray();

            logger.LogInformation("Successfully retrieved {CatsCount} cats in service.", result.Length);

            return result;
        }

        public async Task<CatResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting cat by uid in service: {CatUid}.", uid);

            var cat = await repository.GetByUid(uid, cancellationToken);

            if (cat is null)
            {
                logger.LogWarning("Cat with uid {CatUid} was not found in service.", uid);
                return null;
            }

            logger.LogInformation("Successfully retrieved cat by uid in service: {CatUid}.", uid);

            return Map(cat);
        }

        public async Task<IReadOnlyCollection<CatResponseContract>> SearchByNameAsync(string name, CancellationToken cancellationToken)
        {
            logger.LogInformation("Searching cats by name in service: {CatName}.", name);

            var cats = await repository.SearchByName(name, cancellationToken);

            var result = cats.Select(Map).ToArray();

            logger.LogInformation("Search by name {CatName} returned {CatsCount} cats in service.", name, result.Length);

            return result;
        }

        public async Task<CatResponseContract?> UpdateAsync(Guid uid, UpdateCatRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating cat in service. CatUid: {CatUid}, Name: {CatName}, Breed: {Breed}, Age: {Age}.", uid, request.Name, request.Breed, request.Age);

            var cat = await repository.Update(uid, request.Name, request.Breed, request.Age, cancellationToken);

            if (cat is null)
            {
                logger.LogWarning("Cat with uid {CatUid} was not found for update in service.", uid);
                return null;
            }

            logger.LogInformation("Successfully updated cat in service with uid: {CatUid}.", uid);

            return Map(cat);
        }

        private static CatResponseContract Map(Cat cat)
        {
            return new CatResponseContract
            {
                Uid = cat.Uid,
                UserUid = cat.UserUid,
                Name = cat.Name,
                Breed = cat.Breed,
                Age = cat.Age,
                CreatedAt = cat.CreatedAt,
            };
        }
    }
}
