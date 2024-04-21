using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SafeNetInsureWebApp.Data.Authorization;

public class OrganizationHandler : AuthorizationHandler<OrganizationRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, OrganizationRequirement requirement)
    {
        var accessRole = context.User.GetClaimIssuer(ClaimTypes.Role);

        if (accessRole is null || requirement.PermissionRoles == null)
            return Task.CompletedTask;

        foreach (var requirementRole in requirement.PermissionRoles)
            if (accessRole.Value == requirementRole)
                context.Succeed(requirement);
        return Task.CompletedTask;
    }
}