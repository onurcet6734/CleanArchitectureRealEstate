using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Features.FlatImages.Queries.GetList;
using CleanArchitectureRealEstate.Domain.Entities;
using CleanArchitectureRealEstate.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureRealEstate.Infrastructure.Persistence.Repositories
{
    public class FlatImageRepository : IFlatImageRepository
    {
        private readonly ApplicationDbContext _context;

        public FlatImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FlatImage?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.FlatImages
                .Include(x => x.Flat)
                .ThenInclude(x=>x.User)// Frontend Flat detaylarını istiyor
                .FirstOrDefaultAsync(
                    x => x.Id == id && !x.IsDeleted,
                    cancellationToken);
        }

        public async Task<List<FlatImage>> GetByFlatIdAsync(
            int flatId,
            CancellationToken cancellationToken)
        {
            return await _context.FlatImages
                .Include(x => x.Flat)
                .ThenInclude(x=>x.User)
                .Where(x => x.FlatId == flatId && !x.IsDeleted)
                .OrderByDescending(x => x.Created)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(FlatImage image, CancellationToken cancellationToken)
        {
            await _context.FlatImages.AddAsync(image, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(FlatImage image, CancellationToken cancellationToken)
        {
            _context.FlatImages.Update(image);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(FlatImage image, CancellationToken cancellationToken)
        {
            // Soft delete
            image.IsDeleted = true;
            image.Updated = DateTime.UtcNow;

            _context.FlatImages.Update(image);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<FlatImage?> GetByIdWithFlatAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.FlatImages
                .Include(x => x.Flat)
                .ThenInclude(x=>x.User)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<FlatImage>> GetFlatImagesWithFlatAsync(GetFlatImageListQuery request,CancellationToken cancellationToken)
        {
            IQueryable<FlatImage> query = _context.FlatImages
                .Include(x => x.Flat).
                 ThenInclude(x => x.User)
                .Where(x => !x.IsDeleted);

            if (request.IsCover.HasValue)
            {
                query = query.Where(x => x.IsCover == request.IsCover.Value);
            }
            if (request.MinPrice.HasValue)
            {
                query = query.Where(x => x.Flat.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(x => x.Flat.Price <= request.MaxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.City))
            {
                query = query.Where(x => x.Flat.City == request.City);
            }

            if (!string.IsNullOrWhiteSpace(request.District))
            {
                query = query.Where(x => x.Flat.District == request.District);
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(x => x.Flat.Title.Contains(request.Title));
            }

            if (!string.IsNullOrWhiteSpace(request.AddressLine))
            {
                query = query.Where(x => x.Flat.AddressLine.Contains(request.AddressLine));
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                query = query.Where(x => x.Flat.Description.Contains(request.Description));
            }

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                query = query.Where(x => x.Flat.Status.Value == request.Status);
            }

            if (!string.IsNullOrWhiteSpace(request.Type))
            {
                query = query.Where(x => x.Flat.Type.Value == request.Type);
            }

            // 🔹 Pagination
            int skip = (request.Page - 1) * request.Limit;

            query = query
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(request.Limit);

            return await query.ToListAsync(cancellationToken);
        }

    }
}
