namespace umico.Models.Rating;

public class ProductRating : RatingBase
{
    public Product Product { get; set; }
    public int ProductId { get; set; }
}