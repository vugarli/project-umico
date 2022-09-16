using ProjectUmico.Domain.Models.Attributes;

namespace ProjectUmico.Application.Dtos;

public class AttributeDto : ICachable
{
    public int Id { get; set; }
    
    public AttributeType AttributeType { get; set; }
    
    public string Value { get; set; }
    
    public int? ParentAttributeId { get; set; }
    public DateTime? LastModified { get; set; }
}