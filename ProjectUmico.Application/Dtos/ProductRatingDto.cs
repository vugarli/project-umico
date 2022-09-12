namespace ProjectUmico.Application.Dtos;

public class ProductRatingDto
{
    public string RatedUserEmail { get; set; }
    
    public string Comment { get; set; }
    public int Rate { get; set; }
    public int ProductId { get; set; }
    
}