using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Identity.Permissions.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Permissions.Commands;

internal class AddUserPermissions
{
}

public record AddUserPermissionsCommand : IRequest
{
    public required Guid UserId { get; init; }
    public IEnumerable<string> Permissions { get; init; }
}

public class AddUserPermissionsCommandHandler : IRequestHandler<AddUserPermissionsCommand>
{
    private readonly IIdentityService _context;
    public AddUserPermissionsCommandHandler(
        IIdentityService context)
    {
        _context = context;
    }

    public async Task Handle(AddUserPermissionsCommand request, CancellationToken cancellationToken)
    {
        var rolePermissions = new UserPermissionsDto
        {
            UserId = request.UserId.ToString(),
            Permissions = request.Permissions,
        };

        var entity = await _context.AddUserPermissionsAsync(rolePermissions);
    }
}
