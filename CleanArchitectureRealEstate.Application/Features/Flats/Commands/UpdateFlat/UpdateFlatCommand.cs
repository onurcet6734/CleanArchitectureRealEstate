using CleanArchitectureRealEstate.Application.Common.Models;
using CleanArchitectureRealEstate.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Commands.UpdateFlat
{
    public class UpdateFlatCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string? Title { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string Currency { get; set; } = default!;
        public string City { get; set; }
        public string District { get; set; } = default!;
        public string AddressLine { get; set; } = default!;
        public string Type { get; set; } = default!;
    }
}