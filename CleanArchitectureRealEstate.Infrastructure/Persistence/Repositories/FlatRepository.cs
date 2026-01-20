using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Application.Features.Flats.Queries.GetFlatList;
using CleanArchitectureRealEstate.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureRealEstate.Infrastructure.Persistence.Repositories
{
    public class FlatRepository : IFlatRepository
    {
        private readonly ApplicationDbContext _context;

        public FlatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Flat?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Flats
                .Include(x=>x.User)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        //public async Task<List<Flat>> GetListAsync(CancellationToken cancellationToken)
        //{
        //    return await _context.Flats
        //        .AsNoTracking()
        //        .ToListAsync(cancellationToken);
        //}

        public async Task AddAsync(Flat flat, CancellationToken cancellationToken)
        {
            await _context.Flats.AddAsync(flat, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Flat flat, CancellationToken cancellationToken)
        {
            //_context.Flats.Update(flat);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Flat flat, CancellationToken cancellationToken)
        {
            _context.Flats.Remove(flat);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Flat>> GetAllAsync(
            GetFlatListQuery request,
            CancellationToken cancellationToken)
        {
            var query = _context.Flats
                .AsNoTracking()
                .Include(x => x.User)
                .Where(x => !x.IsDeleted);

            // Range Filters
            if (request.MinPrice is not null)
                query = query.Where(x => x.Price >= request.MinPrice);

            if (request.MaxPrice is not null)
                query = query.Where(x => x.Price <= request.MaxPrice);

            // String Filters
            if (!string.IsNullOrWhiteSpace(request.City))
                query = query.Where(x => x.City == request.City);

            if (!string.IsNullOrWhiteSpace(request.District))
                query = query.Where(x => x.District == request.District);

            if (!string.IsNullOrWhiteSpace(request.Title))
                query = query.Where(x => x.Title.Contains(request.Title));

            if (!string.IsNullOrWhiteSpace(request.AddressLine))
                query = query.Where(x => x.AddressLine.Contains(request.AddressLine));

            if (!string.IsNullOrWhiteSpace(request.Description))
                query = query.Where(x => x.Description.Contains(request.Description));

            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(x => x.Status.Value == request.Status);

            if (!string.IsNullOrWhiteSpace(request.Type))
                query = query.Where(x => x.Type.Value == request.Type);


            // Pagination
            query = query
                .OrderByDescending(x => x.Created)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Flat?> GetByIdWithImagesAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Flats
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }


    }
}
