using ProjectUmico.Domain.Common;

namespace umico.Models.Order;

public class Case : BaseAuditableEntity
{

    public Order Order { get; set; }
    public int OrderId { get; set; }

    // user yada company ola biler
    // public string CreatedBy { get; set; }
    
    public ApplicationUser? User { get; set; }
    public string? UserId { get; set; }
    
    public Company? Company { get; set; }
    public string? CompanyId { get; set; }

    public string CaseDescription { get; set; }
    public string Comment { get; set; }
    
    public bool IsClosed  { get; set; }

    public DateTime ClosedDate { get; set; }
    public string ClosedReason { get; set; }

}