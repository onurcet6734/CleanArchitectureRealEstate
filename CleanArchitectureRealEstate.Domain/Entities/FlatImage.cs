using CleanArchitectureRealEstate.Domain.Entities.Common;

namespace CleanArchitectureRealEstate.Domain.Entities
{
    public class FlatImage : BaseEntity<int> , IAuditEntity 
    {
        public string Url { get; set; } = null!;
        public bool IsCover { get; set; }

        public int FlatId { get; set; }
        public Flat Flat { get; set; } = default!;

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool IsDeleted { get; set; } = false;
        public FlatImage()
        {
            Created = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
