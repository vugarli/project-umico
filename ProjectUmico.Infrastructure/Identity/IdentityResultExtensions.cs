using Microsoft.AspNetCore.Identity;
using ProjectUmico.Application.Common.Models;

namespace ProjectUmico.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded ? Result.Success() : 
            Result.Failure(result.Errors.Select(error => error.Description));
        
    }
}