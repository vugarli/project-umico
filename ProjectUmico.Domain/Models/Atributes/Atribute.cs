using ProjectUmico.Domain.Common;

namespace umico.Models;

public class Atribute : BaseAuditableEntity
{
    public string Value { get; set; }

    public List<Product>  Products { get; set; }
    
    public AtributeGroup AtributGroup { get; set; }
    public int AtributeGroupId { get; set; }
    
}