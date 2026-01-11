using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Domain.Exceptions
{
    public class ValidationDomainException : DomainException
    {
        public ValidationDomainException(string message)
            : base(message) { }
    }
}
