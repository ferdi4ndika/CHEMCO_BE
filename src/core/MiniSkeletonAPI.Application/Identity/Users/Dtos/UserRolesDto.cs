using MiniSkeletonAPI.Application.Identity.Permissions.Dtos;
using MiniSkeletonAPI.Application.Identity.Roles.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Users.Dtos;

public record UserRolesDto
{
    public string UserId { get; init; }
    public IEnumerable<string> RoleIds { get; init; }
}
