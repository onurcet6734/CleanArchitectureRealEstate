using CleanArchitectureRealEstate.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Application.Features.Flats.Commands.UpdateFlat
{
    public class UpdateFlatCommand : IRequest<Result>
    {
        public int FlatId { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = null!;
    }
}