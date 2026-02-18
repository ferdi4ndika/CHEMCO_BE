using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Infrastructure.Identity.Permission
{
    //public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    //{
    //    public PermissionAuthorizationHandler() { }

    //    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    //    {
    //        if (context.User.Identity != null && !context.User.Identity.IsAuthenticated || context.User.Identity == null)
    //        {
    //            context.Fail();
    //            //context.Succeed(requirement);
    //            return;
    //        }
    //        if (requirement.Permission == null)
    //        {
    //            context.Succeed(requirement);
    //            return;
    //        }
    //        var permissionss = context.User.Claims.Where(x => x.Type == "Permission" &&
    //                                                            x.Value == requirement.Permission &&
    //                                                              x.Issuer == "SkeletonAPI");

    //        if (permissionss.Any())
    //        {
    //            context.Succeed(requirement);
    //            return;
    //        }

    //    }

    //}
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;

        public PermissionAuthorizationHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                return;
            }

            // Get all the roles the user belongs to and check if any of the roles has the permission required
            // for the authorization to succeed.
            var permissions = new List<string>();
            var user = await _userManager.GetUserAsync(context.User);
            //await Console.Out.WriteLineAsync(JsonSerializer.Serialize(user));
            var userRoleNames = await _userManager.GetRolesAsync(user);
            var userClaimNames = await _userManager.GetClaimsAsync(user);
            var userRoles = _roleManager.Roles.Where(x => userRoleNames.Contains(x.Name));
            //await Console.Out.WriteLineAsync(JsonSerializer.Serialize(userRoles));
            permissions.AddRange(userClaimNames.Select(x=>x.Value));
            foreach (var role in userRoles)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                permissions.AddRange(roleClaims.Where(x => x.Type == CustomClaimTypes.Permission &&
                                                        x.Value == requirement.Permission &&
                                                        x.Issuer == "LOCAL AUTHORITY")
                                            .Select(x => x.Value));
                //await Console.Out.WriteLineAsync(JsonSerializer.Serialize(roleClaims));
                //await Console.Out.WriteLineAsync(JsonSerializer.Serialize(permissions));
                
            }

            if (permissions.Any())
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}