using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Roles.Dtos;

public record RoleBriefDto
{
    public string Id { get; init; }
    public string Name { get; init; }
}
