using Microsoft.AspNetCore.Authorization;

namespace SafeNetInsureWebApp.Data.Authorization;

public class OrganizationRequirement : IAuthorizationRequirement
{
    public OrganizationRequirement(string[] permissionRoles)
    {
        PermissionRoles = permissionRoles;
    }

    public string[]? PermissionRoles { get; }
}