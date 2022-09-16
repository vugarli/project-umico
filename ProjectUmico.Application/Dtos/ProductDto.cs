using umico.Models;
using umico.Models.Categories;
using umico.Models.Rating;

namespace ProjectUmico.Application.Dtos;

public class ProductDto : ICachable
{
    public int Id { get; set; }
    
    public string SKU { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ThumbnailUrl { get; set; }
    
    // public ICollection<ProductRatingDto>? ProductRatings { get; set; }
    
   // public ICollection<CompanyProductSaleEntry>? SaleEntriesList { get; set; } // Satis
    
   // public List<PromotionDto>? PromotionsForProduct { get; set; }
    
    public List<AttributeDto>? Atributes { get; set; }
    
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime? LastModified { get; set; }
}