using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Domain.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException(string entity, object key)
            : base($"{key} not found. Key: {key}") { }
    }
}
