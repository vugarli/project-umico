using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using umico.Models;

namespace ProjectUmico.Api.Identity;

public static class AuthorizationRequirementExtensions
{
    public static bool UserIsSuperAdmin(this ClaimsPrincipal user)
    {
        return user.HasClaim("Role", "SuperAdmin");
    }
    public static bool UserIsCompanyType(this ClaimsPrincipal user)
    {
        return user.HasClaim("UserType", nameof(Company));
    }
    
    
}