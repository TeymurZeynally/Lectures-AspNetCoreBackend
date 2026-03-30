using Lecture09.CatsApi.Api.Cats.Contracts;
using Lecture09.CatsApi.Api.Cats.Contracts;

namespace Lecture09.CatsApi.Api.Cats.Services
{
	public interface ICatService
	{
		Task<IReadOnlyCollection<CatResponseContract>> GetAllAsync(CancellationToken cancellationToken);

		Task<CatResponseContract?> GetByUidAsync(Guid uid, CancellationToken cancellationToken);

		Task<IReadOnlyCollection<CatResponseContract>> SearchByNameAsync(string name, CancellationToken cancellationToken);

		Task<CatResponseContract> CreateAsync(CreateCatRequestContract request, CancellationToken cancellationToken);

		Task<CatResponseContract?> UpdateAsync(Guid uid, UpdateCatRequestContract request, CancellationToken cancellationToken);

		Task<bool> DeleteAsync(Guid uid, CancellationToken cancellationToken);
	}
}
