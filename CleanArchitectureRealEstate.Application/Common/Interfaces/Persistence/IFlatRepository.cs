using CleanArchitectureRealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence
{
    public interface IFlatRepository
    {
        Task<List<Flat>> GetAllAsync(int page,  int limit , CancellationToken cancellationToken);
        Task<Flat?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(Flat flat, CancellationToken cancellationToken);
        Task UpdateAsync(Flat flat, CancellationToken cancellationToken);
        Task DeleteAsync(Flat flat, CancellationToken cancellationToken);
        Task<Flat?> GetByIdWithImagesAsync(int id, CancellationToken cancellationToken);

    }
}
