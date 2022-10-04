using Microsoft.EntityFrameworkCore;
using ProjectUmico.Domain.Models.Attributes;

namespace ProjectUmico.Infrastructure.Persistance;

public static class ApplyDbSeedingExtensions
{

    public static void ApplySeeding(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyAttributeSeeding();
    }
    
    
    public static void ApplyAttributeSeeding(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<ProductAttribute>()
            .HasData(GetProductAttributes());
    }

    private static ICollection<ProductAttribute> GetProductAttributes()
    {
        var attributes = new List<ProductAttribute>
        {
            new ProductAttribute()
            {
                Id = 1,
                Value = "Color",
                ParentAttributeId = null
            },
            new ProductAttribute()
            {
                Id = 2,
                Value = "Red",
                ParentAttributeId = 1,
            },
            new ProductAttribute()
            {
                Id = 3,
                Value = "Green",
                ParentAttributeId = 1
            },
            new ProductAttribute()
            {
                Id = 4,
                Value = "Blue",
                ParentAttributeId = 1
            },
            new ProductAttribute()
            {
                Id = 5,
                Value = "Pink",
                ParentAttributeId = 1
            }
        };
        return attributes;
    }
    
    
}