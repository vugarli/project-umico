using System.Reflection.Metadata.Ecma335;
using umico.Models.Rating;

namespace umico.Models;

//TODO Identity user here
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<RatingBase> Ratings { get; set; }

    public int UserPersistanceId { get; set; }
    public UserPersistance.UserPersistance UserPersistance { get; set; }
    
    // Sebet itemleri
    // Muqayise
    // Beyenilenler
    
}