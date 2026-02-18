using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Mappings;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.MWarnas.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.MWarnas.Queries.GetMWarnasWithPagination;

public record GetMWarnasWithPaginationQuery : IRequest<PaginatedList<MWarnaBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; set; }
}

public class GetMWarnasWithPaginationQueryHandler : IRequestHandler<GetMWarnasWithPaginationQuery, PaginatedList<MWarnaBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMWarnasWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    public async Task<PaginatedList<MWarnaBriefDto>> Handle(GetMWarnasWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.MWarnas
               .Where(x =>
             string.IsNullOrEmpty(request.SearchTerm) ||
             x.Coler.ToLower().Contains(request.SearchTerm.ToLower()))
      .OrderByDescending(x => x.CreatedAt)
      .ProjectTo<MWarnaBriefDto>(_mapper.ConfigurationProvider)
      .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
