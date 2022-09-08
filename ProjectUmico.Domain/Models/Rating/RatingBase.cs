using ProjectUmico.Domain.Common;

namespace umico.Models.Rating;

public class RatingBase : BaseAuditableEntity
{
    public string Comment { get; set; }
    public int Rate { get; set; }
    
    public User RatedUser { get; set; } // remove ? and error
    public int RatedUserId { get; set; }
    
    // TODO: Add different categories of rating
}
