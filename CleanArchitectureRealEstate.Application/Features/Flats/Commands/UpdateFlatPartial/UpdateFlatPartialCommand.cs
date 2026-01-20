using CleanArchitectureRealEstate.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Commands.UpdateFlatPartial
{
    public class UpdateFlatPartialCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string? Title { get; init; }
        public string? Description { get; init; }
        public decimal? Price { get; init; }
        public string? Currency { get; init; }
        public string? City { get; init; }
        public string? District { get; init; }
        public string? AddressLine { get; init; }
        public string? Status { get; init; }
        public string? Type { get; init; }
    }
}
