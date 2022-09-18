using ProjectUmico.Domain.Common;
using umico.Models;

namespace ProjectUmico.Domain.Models.Attributes;

public class ProductAttribute : BaseAuditableEntity
{
    public string Value { get; set; }
    public List<Product>  Products { get; set; }
    
    public AttributeType AttributeType { get; set; } //TODO Maybe map AttributeType to owned entity type?
    public ICollection<ProductAttribute> Children { get; set; }
    
    public int? ParentAttributeId { get; set; }
    public ProductAttribute? ParentAttribute { get; set; }
}

public enum AttributeType
{
Attribute,
AttributeGroup
}


