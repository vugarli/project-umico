namespace ProjectUmico.Application.Dtos;

public class PromotionDto
{
    //TODO : ICachable
    public string PromotionDescription { get; set; }
    public string PromotionName { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public int PromotionDiscountRate { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}