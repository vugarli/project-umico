using Microsoft.AspNetCore.Identity;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;

namespace ProjectUmico.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<(Result result, string UserId)> CreateUser(string email, string password)
    {
        var appuser = new ApplicationUser()
        {
            UserName = email,
            Email = email
        };
        
        var result = await _userManager.CreateAsync(appuser,password);

        return (result.ToApplicationResult(), appuser.Id);
    }

    public async Task<Result> DeleteUser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user != null ? await DeleteUser(user) : Result.Success();
    }
    
    public async Task<Result> DeleteUser(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }
}