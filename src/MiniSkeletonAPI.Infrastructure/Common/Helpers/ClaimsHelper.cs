using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MiniSkeletonAPI.Infrastructure.Identity;

namespace MiniSkeletonAPI.Infrastructure.Common.Helpers;

public static class ClaimsHelper
{
    public static void GetPermissions(this List<RoleClaimsDto> allPermissions, Type policy, string roleId)
    {
        FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);

        foreach (FieldInfo fi in fields)
        {
            allPermissions.Add(new RoleClaimsDto { Value = fi.GetValue(null).ToString(), Type = "Permissions" });
        }
    }

    public static async Task AddPermissionClaim(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string permission)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
        {
            await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
        }
    }

    public static async Task AddPermissionClaim(this UserManager<ApplicationUser> userManager, ApplicationUser user, string permission)
    {
        var allClaims = await userManager.GetClaimsAsync(user);
        if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
        {
            await userManager.AddClaimAsync(user, new Claim("Permission", permission));
        }
    }
}
