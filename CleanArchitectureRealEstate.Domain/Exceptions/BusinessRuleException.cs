using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureRealEstate.Domain.Exceptions
{
    public class BusinessRuleException : DomainException
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}
