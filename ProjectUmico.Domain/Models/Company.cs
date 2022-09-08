using System.Security.Principal;
using umico.Models.Rating;

namespace umico.Models;

public class Company : User
{
    public string CompanyLogo { get; set; }
    public List<CompanyRating> CompanyRatings { get; set; } // ratings that users gave
    
    public List<CompanyProductSaleEntry> SaleEntriesList { get; set; } // looks fishy
    //public List<CompanyProductPromotionSaleEntries> PromotionSaleEntriesList { get; set; } // looks fishy 
    
}