using Azure;
using CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence;
using CleanArchitectureRealEstate.Domain.Entities;
using CleanArchitectureRealEstate.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        public async Task<List<Flat>> GetAllAsync(int page , int limit,  CancellationToken cancellationToken)
        {

            var query = _context.Flats.AsNoTracking(); // Query olduğu için tracking kapalı (performans)

            if (page > 0 && limit > 0)
            {
                query = query
                    .Where(x => !x.IsDeleted)
                    .OrderByDescending(x => x.Created)
                    .Skip((page - 1) * limit)
                    .Take(limit);
            }

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
