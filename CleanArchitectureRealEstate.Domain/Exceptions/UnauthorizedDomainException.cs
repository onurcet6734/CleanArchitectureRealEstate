using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Domain.Exceptions
{
    public class UnauthorizedDomainException : DomainException
    {
        public UnauthorizedDomainException()
            : base("Unauthorized operation") { }
    }
}
