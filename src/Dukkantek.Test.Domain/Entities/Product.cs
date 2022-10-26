#nullable disable

using Dukkantek.Test.Domain.Common;

namespace Dukkantek.Test.Domain.Entities;

public class Product : AuditableEntity, IHasDomainEvent
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Barcode { get; set; }
    public string Decription { get; set; }
    public bool Weighted { get; set; }
    public short StatusId { get; set; }

    public ProductCategory Category { get; set; }
    public ProductStatus Status { get; set; }
    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}
