using Microsoft.AspNetCore.Authorization;

namespace ProjectUmico.Api.Identity;

public class SuperAdminRequirement : IAuthorizationRequirement { }

public class SuperAdminRequirementHandler : AuthorizationHandler<SuperAdminRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SuperAdminRequirement requirement)
    {
        if (context.User.UserIsSuperAdmin())
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}