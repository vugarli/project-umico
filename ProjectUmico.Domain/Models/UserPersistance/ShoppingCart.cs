namespace umico.Models.UserPersistance;

public class ShoppingCart
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public ApplicationUser User { get; set; }

    public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
}

public class ShoppingCartItem
{
    public int Id { get; set; }
    
    public int ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; }
    
    public int CompanyProductSaleEntryId { get; set; }
    public CompanyProductSaleEntry CompanyProductSaleEntry { get; set; }
}