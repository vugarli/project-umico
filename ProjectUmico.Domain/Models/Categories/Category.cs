using ProjectUmico.Domain.Common;

namespace umico.Models.Categories;

public class Category : BaseAuditableEntity
{
    public string Name { get; set; }

    public Category? Parent { get; set; }
    public int? ParentId { get; set; }
    public ICollection<Category>? Children { get; set; }

}