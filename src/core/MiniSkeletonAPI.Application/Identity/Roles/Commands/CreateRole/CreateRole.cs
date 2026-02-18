using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Roles.Commands.CreateRole;
public record CreateRoleCommand : IRequest<Guid>
{
    public required string Name { get; init; }
}

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly IIdentityService _context;
    public CreateRoleCommandHandler(
        IIdentityService context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            Name = request.Name,
        };

        var entity = await _context.CreateRoleAsync(role);
        return Guid.Parse(entity.RoleId);
    }
}
