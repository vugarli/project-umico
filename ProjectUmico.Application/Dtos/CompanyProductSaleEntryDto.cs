using umico.Enums;
using umico.Models;

namespace ProjectUmico.Application.Dtos;

public class CompanyProductSaleEntryDto : ICachable
{
    public string CompanyId { get; set; }
    public string CompanyName { get; set; }
    
    public int ProductId { get; set; }
    
    public decimal Price { get; set; }

    public int StockCount { get; set; }

    public SaleEntryTypes SaleEntryType { get; set; }

    public bool IsInPromotion { get; set; }
    public int? PromotionId { get; set; }

    public DateTime? LastModified { get; set; }
}