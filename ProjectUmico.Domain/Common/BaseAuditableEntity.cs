namespace ProjectUmico.Domain.Common;


//TODO implement this to auditable classes
public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

}