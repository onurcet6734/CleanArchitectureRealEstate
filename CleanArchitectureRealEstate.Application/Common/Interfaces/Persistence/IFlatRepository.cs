using CleanArchitectureRealEstate.Application.Features.Flats.Queries.GetFlatList;
using CleanArchitectureRealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence
{
    public interface IFlatRepository
    {
        Task<List<Flat>> GetAllAsync(GetFlatListQuery request,  CancellationToken cancellationToken);
        Task<Flat?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(Flat flat, CancellationToken cancellationToken);
        Task UpdateAsync(Flat flat, CancellationToken cancellationToken);
        Task DeleteAsync(Flat flat, CancellationToken cancellationToken);
        Task<Flat?> GetByIdWithImagesAsync(int id, CancellationToken cancellationToken);

    }
}
