using CleanArchitectureRealEstate.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureRealEstate.Infrastructure.Persistence.Configurations
{
    public class FlatConfiguration : IEntityTypeConfiguration<Flat>
    {
        public void Configure(EntityTypeBuilder<Flat> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Price).HasPrecision(18, 2);
            builder.Property(x => x.Currency).IsRequired().HasMaxLength(10);

            builder.Property(x => x.City).IsRequired().HasMaxLength(100);
            builder.Property(x => x.District).IsRequired().HasMaxLength(100);
            builder.Property(x => x.AddressLine).IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId);

            builder.OwnsOne(x => x.Type, t =>
            {
                t.Property(p => p.Value)
                 .HasColumnName("Type")
                 .IsRequired()
                 .HasMaxLength(50);
            });

            builder.OwnsOne(x => x.Status, s =>
            {
                s.Property(p => p.Value)
                 .HasColumnName("Status")
                 .IsRequired()
                 .HasMaxLength(50);
            });

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
