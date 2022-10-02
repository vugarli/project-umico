namespace ProjectUmico.Application.Dtos;

public class ProductRatingDto
{
    //TODO : ICachable
    
    public string RatedUserId { get; set; } //todo change to email
    
    public string Comment { get; set; }
    
    public int Rate { get; set; }
    
    public int ProductId { get; set; }
    
}