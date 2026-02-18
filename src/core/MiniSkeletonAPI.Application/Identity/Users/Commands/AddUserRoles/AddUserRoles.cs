using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Identity.Permissions.Dtos;
using MiniSkeletonAPI.Application.Identity.Roles.Dtos;
using MiniSkeletonAPI.Application.Identity.Roles.Queries.GetRolesWithPagination;
using MiniSkeletonAPI.Application.Identity.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Users.Commands.AddUserRoles;

public record AddUserRolesCommand : IRequest
{
    public required Guid UserId { get; init; }
    public IEnumerable<string> RoleIds { get; init; }
}

public class AddUserRolesCommandHandler : IRequestHandler<AddUserRolesCommand>
{
    private readonly IIdentityService _context;
    public AddUserRolesCommandHandler(
        IIdentityService context)
    {
        _context = context;
    }

    public async Task Handle(AddUserRolesCommand request, CancellationToken cancellationToken)
    {
        var rolePermissions = new UserRolesDto
        {
            UserId = request.UserId.ToString(),
            RoleIds = request.RoleIds,
        };

        var entity = await _context.AddUserRolesAsync(rolePermissions);

    }
}