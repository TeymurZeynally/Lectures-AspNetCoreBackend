using Lecture08.CatsApi.Api.Cats.Contracts;

namespace Lecture08.CatsApi.Api.Cats.Services
{
    public sealed class CatService(ILogger<CatService> logger) : ICatService
    {
        public async Task<CatResponseContract> CreateAsync(CreateCatRequestContract request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<CatResponseContract>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<CatResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<CatResponseContract>> SearchByNameAsync(string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<CatResponseContract?> UpdateAsync(Guid uid, UpdateCatRequestContract request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
