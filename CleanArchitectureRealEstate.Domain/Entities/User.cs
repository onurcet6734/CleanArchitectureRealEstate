using CleanArchitectureRealEstate.Domain.Entities.Common;

namespace CleanArchitectureRealEstate.Domain.Entities
{
    public class User : BaseEntity<int>, IAuditEntity
    {
        // Identity
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        // Profile
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName ?? string.Empty} {LastName ?? string.Empty}".Trim();

        // Tokens
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }

        // Relations
        public ICollection<Flat> Flats { get; set; } = new List<Flat>();

        // Audit
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
