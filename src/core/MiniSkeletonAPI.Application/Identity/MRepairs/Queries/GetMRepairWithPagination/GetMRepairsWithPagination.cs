using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Mappings;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.MRepairs.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MRepairs.Queries.GetMRepairsWithPagination;

public record GetMRepairsWithPaginationQuery : IRequest<PaginatedList<MRepairBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; set; }
}

public class GetMRepairsWithPaginationQueryHandler : IRequestHandler<GetMRepairsWithPaginationQuery, PaginatedList<MRepairBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMRepairsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    public async Task<PaginatedList<MRepairBriefDto>> Handle(GetMRepairsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.MRepairs
                    .Where(x =>
                  string.IsNullOrEmpty(request.SearchTerm) ||
                  x.Repair.ToLower().Contains(request.SearchTerm.ToLower()))
           .OrderByDescending(x => x.CreatedAt)
           .ProjectTo<MRepairBriefDto>(_mapper.ConfigurationProvider)
           .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
