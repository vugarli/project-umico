using Microsoft.AspNetCore.Authorization;
using umico.Models;

namespace ProjectUmico.Api.Identity;

public class UserIsCompanyTypeRequirement: IAuthorizationRequirement { }


public class UserIsCompanyTypeRequirementHandler : AuthorizationHandler<UserIsCompanyTypeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserIsCompanyTypeRequirement requirement)
    {
        if (context.User.UserIsCompanyType() || context.User.UserIsSuperAdmin())
        {
            context.Succeed(requirement);          
        }
        
        return Task.CompletedTask;
        
    }
}

