using CleanArchitectureRealEstate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FlatImageConfiguration : IEntityTypeConfiguration<FlatImage>
{
    public void Configure(EntityTypeBuilder<FlatImage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Url)
               .IsRequired()
               .HasMaxLength(500);

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasOne(x => x.Flat)
               .WithMany(f => f.Images)
               .HasForeignKey(x => x.FlatId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
