using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Roles.Commands.UpdateRole;

public record UpdateRoleCommand : IRequest
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand>
{
    private readonly IIdentityService _context;

    public UpdateRoleCommandHandler(
        IIdentityService context
        )
    {
        _context = context;
    }

    public async Task Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            
            Name = request.Name,
        };
        var entity = await _context.UpdateRoleAsync(role, request.Id.ToString());
    }
}