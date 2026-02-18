using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Roles.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Roles.Queries.GetRolesWithPagination;

public record GetRolesWithPaginationQuery : IRequest<PaginatedList<RoleBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetRolesWithPaginationQueryHandler : IRequestHandler<GetRolesWithPaginationQuery, PaginatedList<RoleBriefDto>>
{
    private readonly IIdentityService _context;

    public GetRolesWithPaginationQueryHandler(IIdentityService context)
    {
        _context = context;
    }

    public async Task<PaginatedList<RoleBriefDto>> Handle(GetRolesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.GetRolesPaginatedAsync(request);
        return data;
    }
}
