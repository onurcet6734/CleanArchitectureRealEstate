using CleanArchitectureRealEstate.Domain.Entities.Common;

namespace CleanArchitectureRealEstate.Domain.Entities
{
    public class User :  BaseEntity<int>, IAuditEntity
    {

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => string.Join(" ", FirstName, LastName);
        public string Email { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }

        public ICollection<Flat> Flats { get; set; } = new List<Flat>();

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public bool IsDeleted { get; set; } = false;
        
    }
}
