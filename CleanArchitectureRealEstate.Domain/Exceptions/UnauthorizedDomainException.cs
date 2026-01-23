namespace CleanArchitectureRealEstate.Domain.Exceptions
{
    public class UnauthorizedDomainException : DomainException
    {
        public UnauthorizedDomainException()
            : base("Unauthorized operation") { }

        public UnauthorizedDomainException(string message)
            : base(message) { }
    }
}
