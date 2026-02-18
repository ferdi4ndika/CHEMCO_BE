using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Permissions.Dtos;

public record RolePermissionsDto
{
    public string RoleId { get; init; }
    public IEnumerable<string> Permissions { get; init; }
}
