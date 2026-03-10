    using CleanArchitectureRealEstate.Domain.Entities.Common;
    using Microsoft.AspNetCore.Identity;

    namespace CleanArchitectureRealEstate.Domain.Entities
    {
        public class User : IdentityUser<int>  ,  IAuditEntity
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string FullName => $"{FirstName ?? string.Empty} {LastName ?? string.Empty}".Trim();
            public string? PasswordResetToken { get; set; }
            public DateTime? PasswordResetTokenExpires { get; set; }

        // Verification
            public bool IsEDevletVerified { get; set; } = false;
            public DateTime? EDevletVerifiedAt { get; set; }

            // Tokens
            public string? RefreshToken { get; set; }
            public DateTime? RefreshTokenExpires { get; set; }

            public ICollection<Flat> Flats { get; set; } = new List<Flat>();

            // Audit
            public DateTime Created { get; set; }
            public DateTime? Updated { get; set; }
            public bool IsDeleted { get; set; } = false;
        }
    }
