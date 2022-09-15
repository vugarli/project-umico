namespace ProjectUmico.Application.Dtos;

public class CategoryDto
{
    public int? CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string? CategoryParentName { get; set; }
    public int? ParentId { get; set; }
    
    // public DateTime? CreatedAt { get; set; }
    // public string? CreatedBy { get; set; }
    //
    // public DateTime? LastModified { get; set; }
    // public string? LastModifiedBy { get; set; }

}