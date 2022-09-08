using System.Reflection.Metadata.Ecma335;
using ProjectUmico.Domain.Common;

namespace umico.Models;

public class Promotion : BaseAuditableEntity
{

    public List<Product> ProductsInPromotion { get; set; }
    
    public string PromotionDescription { get; set; }
    public string PromotionName { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public int PromotionDiscountRate { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

}