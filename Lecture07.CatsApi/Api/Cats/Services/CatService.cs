using Lecture07.CatsApi.Api.Cats.Contracts;

namespace Lecture07.CatsApi.Api.Cats.Services
{
	public class CatService : ICatService
	{
		public Task<CatResponseContract> CreateAsync(CreateCatRequestContract request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyCollection<CatResponseContract>> GetAllAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<CatResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyCollection<CatResponseContract>> SearchByNameAsync(string name, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task<CatResponseContract?> UpdateAsync(Guid uid, UpdateCatRequestContract request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
