using ProjectUmico.Domain.Common;
using umico.Enums;

namespace umico.Models;

public class CompanyProductSaleEntry : BaseAuditableEntity
{
    public Company Company { get; set; }
    public string CompanyId { get; set; }
    
    public Product Product { get; set; }
    
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int StockCount { get; set; }
    public SaleEntryTypes SaleEntryType { get; set; } = SaleEntryTypes.Normal;
    public Promotion? Promotion { get; set; }
    public int? PromotionId { get; set; }
}