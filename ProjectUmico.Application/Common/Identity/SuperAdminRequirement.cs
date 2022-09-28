using Microsoft.AspNetCore.Authorization;

namespace ProjectUmico.Application.Common.Identity;

public class SuperAdminRequirement : IAuthorizationRequirement { }

public class SuperAdminRequirementHandler : AuthorizationHandler<SuperAdminRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SuperAdminRequirement requirement)
    {
        if (context.User.HasClaim("Role", "SuperAdmin"))
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}