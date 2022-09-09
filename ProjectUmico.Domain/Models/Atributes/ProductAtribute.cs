using ProjectUmico.Domain.Common;

namespace umico.Models;

public class ProductAtribute : BaseAuditableEntity
{
    public string Value { get; set; }

    public List<Product>  Products { get; set; }

    public ICollection<ProductAtribute>? Children { get; set; }

    public int? ParentAttributeId { get; set; }
    public ProductAtribute? ParentAttribute { get; set; }
    
}