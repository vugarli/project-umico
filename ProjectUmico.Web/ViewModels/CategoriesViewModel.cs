using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Web.Models;

public class CategoriesViewModel
{
    public ICollection<CategoryDto> Categories { get; set; }

    public string CategoryName { get; set; }
    
    public string CategoryParentName { get; set; }
    
}