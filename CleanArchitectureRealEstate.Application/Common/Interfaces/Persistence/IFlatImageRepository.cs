using CleanArchitectureRealEstate.Application.Features.FlatImages.Queries.GetList;
using CleanArchitectureRealEstate.Domain.Entities;

namespace CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence
{
    public interface IFlatImageRepository
    {
        Task<FlatImage?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<FlatImage>> GetByFlatIdAsync(int flatId, CancellationToken cancellationToken);
        Task<List<FlatImage>> GetAllAsync(CancellationToken cancellationToken);

        Task<FlatImage?> GetByIdWithFlatAsync(int id, CancellationToken cancellationToken);

        Task<List<FlatImage>> GetFlatImagesWithFlatAsync(GetFlatImageListQuery request , CancellationToken cancellationToken);
        Task AddAsync(FlatImage image, CancellationToken cancellationToken);
        Task UpdateAsync(FlatImage image, CancellationToken cancellationToken);
        Task DeleteAsync(FlatImage image, CancellationToken cancellationToken);
    }
}