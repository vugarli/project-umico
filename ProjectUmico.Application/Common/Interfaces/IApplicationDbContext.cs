using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectUmico.Domain.Models.Attributes;
using umico.Models;
using umico.Models.Categories;
using umico.Models.Order;
using umico.Models.Rating;

namespace ProjectUmico.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    EntityEntry Entry(object a);

    public DbSet<ApplicationUser> Users { get; set; }
    
    // Order related
    public DbSet<Order> Orders { get; set; }    
    public DbSet<Case> Cases { get; set; }

    // Product Related
    public DbSet<RatingBase> Ratings { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductAttribute> Attributes { get; set; }
    public DbSet<CompanyProductSaleEntry> CompanyProductSaleEntries { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}