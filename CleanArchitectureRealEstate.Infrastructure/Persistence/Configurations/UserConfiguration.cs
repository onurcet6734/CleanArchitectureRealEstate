using CleanArchitectureRealEstate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureRealEstate.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Username)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Username).IsUnique();

            // Relations
            builder.HasMany(x => x.Flats)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Soft delete
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
