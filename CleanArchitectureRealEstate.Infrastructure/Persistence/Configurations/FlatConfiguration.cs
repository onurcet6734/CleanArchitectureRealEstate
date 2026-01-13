using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FlatConfiguration : IEntityTypeConfiguration<Flat>
{
    public void Configure(EntityTypeBuilder<Flat> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Price).HasPrecision(18, 2).IsRequired();


        builder.OwnsOne(x => x.Status, status =>
        {
            status.Property(s => s.Value)
                  .HasColumnName("Status")
                  .IsRequired();
        });

        builder.OwnsOne(x => x.Type, type =>
        {
            type.Property(t => t.Value)
                .HasColumnName("Type")
                .IsRequired();
        });

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
