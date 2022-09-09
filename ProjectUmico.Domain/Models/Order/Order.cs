using ProjectUmico.Domain.Common;
using ProjectUmico.Domain.Enums;

namespace umico.Models.Order;

public class Order : BaseAuditableEntity
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    
    
    public CompanyProductSaleEntry SaleEntry{ get; set; }
    public int SaleEntryId { get; set; }
    
    public List<Case> Cases { get; set; }

    public string ShippingAdress { get; set; } // TODO move to different table

    public string Carrier { get; set; }
    
    public string CarrierReceivedPackageEvidencePictureUrl { get; set; }
    public string CarrierDeliveredPackageEvidencePictureUrl { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public DateTime ShippedDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    
}