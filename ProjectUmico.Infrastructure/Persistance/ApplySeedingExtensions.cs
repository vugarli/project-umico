using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectUmico.Infrastructure.Persistance;

public static class ApplySeedingExtensions
{
    public static async void SeedUserData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (await userManager.FindByEmailAsync("s@s.com") is not null)
        {
         return;
        }

        var superAdminUser = new ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "s@s.com",
            Email = "s@s.com",
            NormalizedEmail = "S@S.COM",
            NormalizedUserName = "S@S.COM",
        };

        var resultcreation = await userManager.CreateAsync(superAdminUser, "vugar2003V$03");
        var resultClaims = await userManager.AddClaimAsync(superAdminUser, new Claim("Role", "SuperAdmin"));
    }
}