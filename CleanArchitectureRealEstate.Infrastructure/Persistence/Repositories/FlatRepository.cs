using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Domain.Entities;
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
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<Flat>> GetListAsync(CancellationToken cancellationToken)
        {
            return await _context.Flats
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Flat flat, CancellationToken cancellationToken)
        {
            await _context.Flats.AddAsync(flat, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Flat flat, CancellationToken cancellationToken)
        {
            _context.Flats.Update(flat);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Flat flat, CancellationToken cancellationToken)
        {
            flat.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
