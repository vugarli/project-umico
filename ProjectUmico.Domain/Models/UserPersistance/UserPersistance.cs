namespace umico.Models.UserPersistance;

public class UserPersistance
{
    public int Id { get; set; }

    public ApplicationUser User { get; set; }
    public int UserId { get; set; }

    public string Content { get; set; }
}