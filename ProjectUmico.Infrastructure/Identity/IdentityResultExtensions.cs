using System.Collections.Immutable;
using Microsoft.AspNetCore.Identity;
using ProjectUmico.Application.Common.Models;

namespace ProjectUmico.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static Result<ApplicationUser> ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded ? Result<ApplicationUser>.Success() : 
            Result<ApplicationUser>.Failure(result.Errors.Select(error => error.Description).ToImmutableList());
        
    }
}