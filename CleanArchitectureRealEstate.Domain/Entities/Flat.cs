using CleanArchitectureRealEstate.Domain.Entities;
using CleanArchitectureRealEstate.Domain.Entities.Common;
using CleanArchitectureRealEstate.Domain.ValueObjects;

public class Flat : BaseEntity<int>, IAuditEntity
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public decimal Price { get; set; }
    public string Currency { get; set; } = default!;

    public string City { get; set; } = default!;
    public string District { get; set; } = default!;
    public string AddressLine { get; set; } = default!;

    public FlatType Type { get; set; } = null!;
    public FlatStatus Status { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public ICollection<FlatImage> Images { get; set; } = new List<FlatImage>();
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public bool IsDeleted { get; set; } = false;

    // EF + Application için açık
    public Flat()
    {
        Created = DateTime.UtcNow;
        IsDeleted = false;
    }
}
