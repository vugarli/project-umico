using ProjectUmico.Domain.Common;

namespace umico.Models;

public class AtributeGroup : BaseAuditableEntity
{
    public string Name { get; set; }

    public List<Atribute> Atributes { get; set; }
}