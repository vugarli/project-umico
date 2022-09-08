
using ProjectUmico.Domain.Common;
using umico.Models.Categories;
using umico.Models.Rating;

namespace umico.Models;

public class Product : BaseAuditableEntity
{
    public string SKU { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ThumbnailUrl { get; set; }

    // Ratings
    public List<ProductRating> ProductRatings { get; set; }
    
    // SaleEntries
    public List<CompanyProductSaleEntry> SaleEntriesList { get; set; } // Satis
    //public List<CompanyProductPromotionSaleEntries> PromotionSaleEntriesList { get; set; } // Satis aksiya
    
    public List<Promotion> ProductInPromotions { get; set; }
    
    public List<Atribute> Atributes { get; set; }

     public Category Category { get; set; }
     public int CategoryId { get; set; }
    
}