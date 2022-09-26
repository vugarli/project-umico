using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectUmico.Infrastructure.Persistance;

public static class ApplySeedingExtensions
{
    // public static ModelBuilder SeedUserData(this ModelBuilder modelBuilder,IPasswordHasher<ApplicationUser> passwordHasher)
    // {
    //     
    //     var superAdminUser = new ApplicationUser()
    //     {
    //         Id = Guid.NewGuid().ToString(),
    //         UserName = "s@s.com",
    //         Email = "s@s.com"
    //     };
    //     var phash = passwordHasher.HashPassword(superAdminUser, "vugar2003V$03");
    //     superAdminUser.PasswordHash = phash;
    //     
    //     var identityUserClaim = new IdentityUserClaim<string>()
    //     {
    //         Id = 1,
    //         UserId = superAdminUser.Id,
    //         ClaimType = "Role",
    //         ClaimValue = "SuperAdmin"
    //     };
    //     
    //     modelBuilder.Entity<ApplicationUser>()
    //         .HasData(superAdminUser);
    //
    //     modelBuilder.Entity<IdentityUserClaim<string>>()
    //         .HasData(identityUserClaim);
    //     
    //     
    //     return modelBuilder;
    // }

    public static async void SeedUserData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (await userManager.FindByEmailAsync("s@s.com") is null)
        {
            return;
        }

        var superAdminUser = new ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "s@s.com",
            Email = "s@s.com"
        };

        await userManager.CreateAsync(superAdminUser, "vugar2003V$03");
        await userManager.AddClaimAsync(superAdminUser, new Claim("Role", "SuperAdmin"));
    }
}