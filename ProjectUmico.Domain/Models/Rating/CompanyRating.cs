namespace umico.Models.Rating;

public class CompanyRating : RatingBase
{
    public Company? Company { get; set; }
    public string? CompanyId { get; set; }
}