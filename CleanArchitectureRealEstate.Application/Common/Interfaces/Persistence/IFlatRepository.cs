using CleanArchitectureRealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Common.Interfaces.Persistence
{
    public interface IFlatRepository
    {
        Task AddAsync(Flat flat, CancellationToken cancellationToken);
        Task<Flat?> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}
