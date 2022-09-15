using ProjectUmico.Domain.Models.Attributes;

namespace ProjectUmico.Application.Dtos;

public class AttributeDto
{
    public int Id { get; set; }
    
    public AttributeType AttributeType { get; set; }
    
    public string Value { get; set; }
    
    public int? ParentAttributeId { get; set; }
}