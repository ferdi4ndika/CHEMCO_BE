using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Identity.Permissions.Dtos;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Permissions.Commands;

public record AddRolePermissionsCommand : IRequest
{
    public required Guid RoleId { get; init; }
    public List<string> Permissions { get; init; }
}

public class AddRolePermissionsCommandHandler : IRequestHandler<AddRolePermissionsCommand>
{
    private readonly IIdentityService _context;
    public AddRolePermissionsCommandHandler(
        IIdentityService identityService)
    {
        _context = identityService;
    }

    public async Task Handle(AddRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var rolePermissions = new RolePermissionsDto
        {
           RoleId = request.RoleId.ToString(),
           Permissions = request.Permissions,
        };

        var entity = await _context.AddRolePermissionsAsync(rolePermissions);
    }
}
