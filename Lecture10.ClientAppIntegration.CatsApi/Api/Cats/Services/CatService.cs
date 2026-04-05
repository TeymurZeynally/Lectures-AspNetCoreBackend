using System.Linq.Expressions;
using Lecture10.ClientAppIntegration.CatsApi.Api.Cats.Contracts;
using Lecture10.ClientAppIntegration.CatsDatabase.DataAccess.EF;
using Lecture10.ClientAppIntegration.CatsDatabase.DataAccess.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lecture10.ClientAppIntegration.CatsApi.Api.Cats.Services
{
    public sealed class CatService(CatsDbContext dbContext, ILogger<CatService> logger) : ICatService
    {
        public async Task<CatResponseContract> CreateAsync(CreateCatRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting cat creation. UserUid: {UserUid}, Name: {Name}, Breed: {Breed}, Age: {Age}.", request.UserUid, request.Name, request.Breed, request.Age);

            var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Uid == request.UserUid, cancellationToken);

            if (user is null)
            {
                logger.LogWarning("Cat creation failed because user was not found. UserUid: {UserUid}, Name: {Name}.", request.UserUid, request.Name);
                throw new InvalidOperationException($"User with uid '{request.UserUid}' was not found.");
            }

            var cat = new Cat
            {
                UserId = user.Id,
                Name = request.Name,
                Breed = request.Breed,
                Age = request.Age
            };

            await dbContext.Cats.AddAsync(cat, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Cat entity persisted successfully. CatId: {CatId}, CatUid: {CatUid}, UserId: {UserId}, UserUid: {UserUid}.", cat.Id, cat.Uid, user.Id, user.Uid);

            var response = new CatResponseContract
            {
                Uid = cat.Uid,
                UserUid = user.Uid,
                Name = cat.Name,
                Breed = cat.Breed,
                Age = cat.Age,
                CreatedAt = cat.CreatedAt
            };

            logger.LogInformation("Cat creation completed. CatUid: {CatUid}, Name: {Name}, Breed: {Breed}, Age: {Age}, CreatedAt: {CreatedAt}.", response.Uid, response.Name, response.Breed, response.Age, response.CreatedAt);

            return response;
        }

        public async Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting cat deletion. CatUid: {CatUid}.", uid);

            var cat = await dbContext.Cats.Include(x => x.Posts).SingleOrDefaultAsync(x => x.Uid == uid, cancellationToken);

            if (cat is null)
            {
                logger.LogWarning("Cat deletion skipped because cat was not found. CatUid: {CatUid}.", uid);
                return false;
            }

            logger.LogInformation("Cat found for deletion. CatUid: {CatUid}, Name: {Name}, RelatedPostsCount: {RelatedPostsCount}.", cat.Uid, cat.Name, cat.Posts.Count);

            cat.Posts.Clear();
            dbContext.Cats.Remove(cat);

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Cat deleted successfully. CatId: {CatId}, CatUid: {CatUid}, Name: {Name}.", cat.Id, cat.Uid, cat.Name);

            return true;
        }

        public async Task<IReadOnlyCollection<CatResponseContract>> GetAllAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Loading all cats.");

            var cats = await dbContext.Cats.AsNoTracking().Select(MapToContract()).ToArrayAsync(cancellationToken);

            logger.LogInformation("All cats loaded successfully. Count: {Count}.", cats.Length);

            return cats;
        }

        public async Task<CatResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken)
        {
            logger.LogInformation("Loading cat by uid. CatUid: {CatUid}.", uid);

            var cat = await dbContext.Cats.AsNoTracking().Where(x => x.Uid == uid).Select(MapToContract()).SingleOrDefaultAsync(cancellationToken);

            if (cat is null)
            {
                logger.LogWarning("Cat was not found by uid. CatUid: {CatUid}.", uid);
                return null;
            }

            logger.LogInformation("Cat loaded successfully by uid. CatUid: {CatUid}, UserUid: {UserUid}, Name: {Name}.", cat.Uid, cat.UserUid, cat.Name);

            return cat;
        }

        public async Task<IReadOnlyCollection<CatResponseContract>> SearchByNameAsync(string name, CancellationToken cancellationToken)
        {
            logger.LogInformation("Searching cats by name. SearchTerm: {SearchTerm}.", name);

            var normalizedName = name.Trim();

            var cats = await dbContext.Cats
                .AsNoTracking()
                .Where(x => EF.Functions.ILike(x.Name, $"%{normalizedName}%"))
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .Select(MapToContract())
                .ToArrayAsync(cancellationToken);

            logger.LogInformation("Cat search completed. SearchTerm: {SearchTerm}, ResultCount: {ResultCount}.", normalizedName, cats.Length);

            return cats;
        }

        public async Task<CatResponseContract?> UpdateAsync(Guid uid, UpdateCatRequestContract request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting cat update. CatUid: {CatUid}, Name: {Name}, Breed: {Breed}, Age: {Age}.", uid, request.Name, request.Breed, request.Age);

            var cat = await dbContext.Cats.Include(x => x.User).SingleOrDefaultAsync(x => x.Uid == uid, cancellationToken);

            if (cat is null)
            {
                logger.LogWarning("Cat update skipped because cat was not found. CatUid: {CatUid}.", uid);
                return null;
            }

            logger.LogInformation("Current cat state before update. CatUid: {CatUid}, CurrentName: {CurrentName}, CurrentBreed: {CurrentBreed}, CurrentAge: {CurrentAge}.", cat.Uid, cat.Name, cat.Breed, cat.Age);

            cat.Name = request.Name;
            cat.Breed = request.Breed;
            cat.Age = request.Age;

            dbContext.Cats.Update(cat);
            await dbContext.SaveChangesAsync(cancellationToken);

            var response = new CatResponseContract
            {
                Uid = cat.Uid,
                UserUid = cat.User.Uid,
                Name = cat.Name,
                Breed = cat.Breed,
                Age = cat.Age,
                CreatedAt = cat.CreatedAt
            };

            logger.LogInformation("Cat updated successfully. CatUid: {CatUid}, Name: {Name}, Breed: {Breed}, Age: {Age}.", response.Uid, response.Name, response.Breed, response.Age);

            return response;
        }

        private Expression<Func<Cat, CatResponseContract>> MapToContract()
        {
            return cat => new CatResponseContract
            {
                Uid = cat.Uid,
                UserUid = cat.User.Uid,
                Name = cat.Name,
                Breed = cat.Breed,
                Age = cat.Age,
                CreatedAt = cat.CreatedAt
            };
        }
    }
}