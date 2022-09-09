﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Infrastructure.Identity;
using umico.Models;
using umico.Models.Categories;
using umico.Models.Order;
using umico.Models.Rating;
// using umico.Models.UserPersistance;

namespace ProjectUmico.Infrastructure.Persistance;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IConfiguration _configuration;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IConfiguration configuration):base(options)
    {
        _configuration = configuration;
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer(_configuration["DbConnect"]);
    //     base.OnConfiguring(optionsBuilder);
    // }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<CompanyProductPromotionSaleEntries>()
        //          .HasBaseType<CompanyProductSaleEntriesBase>();
        // Sale Types
        
        // // Promo
        // modelBuilder.Entity<Company>()
        //     .HasMany<CompanyProductPromotionSaleEntries>(o=>o.PromotionSaleEntriesList)
        //     .WithOne(o => o.Company)
        //     .HasForeignKey(o => o.CompanyId)
        //     .HasPrincipalKey(o=>o.Id)
        //     .OnDelete(DeleteBehavior.NoAction);;
        //
        // modelBuilder.Entity<CompanyProductPromotionSaleEntries>()
        //     .HasOne<Company>(o=>o.Company)
        //     .WithMany(o => o.PromotionSaleEntriesList)
        //     .HasForeignKey(o => o.CompanyId)
        //     .HasPrincipalKey(o=>o.Id)
        //     .OnDelete(DeleteBehavior.NoAction);;
        //
        
        
        
        // Without Promo
        modelBuilder.Entity<Company>()
            .HasMany<CompanyProductSaleEntry>(o=>o.SaleEntriesList)
            .WithOne(o => o.Company)
            .HasForeignKey(o => o.CompanyId)
            .HasPrincipalKey(o=>o.Id)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<CompanyProductSaleEntry>()
            .HasOne<Company>(o=>o.Company)
            .WithMany(o => o.SaleEntriesList)
            .HasForeignKey(o => o.CompanyId)
            .HasPrincipalKey(o=>o.Id)
            .OnDelete(DeleteBehavior.NoAction);

        // Rating Types
        modelBuilder.Entity<CompanyRating>()
            .HasBaseType<RatingBase>();
        modelBuilder.Entity<ProductRating>()
            .HasBaseType<RatingBase>();
        
        modelBuilder.Entity<RatingBase>()
            .HasDiscriminator<string>("RatingType")
            .HasValue<CompanyRating>("CompanyRating")
            .HasValue<ProductRating>("ProductRating");
        
        // User Types
        modelBuilder.Entity<Company>()
            .HasBaseType<ApplicationUser>();
        modelBuilder.Entity<ApplicationUser>()
            .HasDiscriminator<string>("UserType")
            .HasValue<Company>("Company")
            .HasValue<ApplicationUser>("User");

        // Rating stuff / FRAGILE \
        
        modelBuilder.Entity<CompanyRating>()
            .HasOne<Company>(o=>o.Company)
            .WithMany(o => o.CompanyRatings)
            .HasPrincipalKey(o=>o.Id)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Company>()
            .HasMany<CompanyRating>(o=>o.CompanyRatings)
            .WithOne(o => o.Company)
            .HasPrincipalKey(o=>o.Id)
            .HasForeignKey(o=>o.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);
        
        // User's rated ratings
        modelBuilder.Entity<RatingBase>()
            .HasOne(o => o.RatedUser)
            .WithMany(o => o.Ratings)
            .HasPrincipalKey(o=>o.Id)
            .HasForeignKey(o => o.RatedUserId);

        //Category
        modelBuilder.Entity<Category>()
            .HasOne(s => s.Parent)
            .WithMany(m => m.Children)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.NoAction);
        // Attribute

        modelBuilder.Entity<ProductAtribute>()
            .HasOne<ProductAtribute>(a => a.ParentAttribute)
            .WithMany(a => a.Children)
            .HasForeignKey(a => a.ParentAttributeId);
        
        
        // Case OrderDetails
        modelBuilder.Entity<Order>()
            .HasMany<Case>(o => o.Cases)
            .WithOne(o => o.Order)
            .HasForeignKey(o => o.OrderId)
            .HasPrincipalKey(o => o.Id);

        // modelBuilder.Entity<ApplicationUser>()
        //     .HasOne<UserPersistance>(o => o.UserPersistance)
        //     .WithOne(o => o.User)
        //     .HasForeignKey<ApplicationUser>(o => o.UserPersistanceId);

        // Console.WriteLine(modelBuilder.Model.ToDebugString());
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<ApplicationUser> Users { get; set; }
    
    // Order related
    public DbSet<Order> Orders { get; set; }    
    public DbSet<Case> Cases { get; set; }

    // Product Related
    public DbSet<RatingBase> Ratings { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductAtribute> ProductAtributes { get; set; }
    public DbSet<CompanyProductSaleEntry> CompanyProductSaleEntries { get; set; }
}