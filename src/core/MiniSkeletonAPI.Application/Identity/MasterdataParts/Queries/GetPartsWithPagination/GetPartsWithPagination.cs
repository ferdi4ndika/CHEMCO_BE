using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Mappings;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Parts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Parts.Queries.GetPartsWithPagination;

public record GetPartsWithPaginationQuery : IRequest<PaginatedList<PartBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; set; }
}

public class GetPartsWithPaginationQueryHandler : IRequestHandler<GetPartsWithPaginationQuery, PaginatedList<PartBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPartsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    public async Task<PaginatedList<PartBriefDto>> Handle(GetPartsWithPaginationQuery request, CancellationToken cancellationToken)
    {

        return await _context.Parts
           .Where(x =>
              string.IsNullOrEmpty(request.SearchTerm) ||
              x.PartName.ToLower().Contains(request.SearchTerm.ToLower()))
           .OrderByDescending(x => x.CreatedAt)
           .ProjectTo<PartBriefDto>(_mapper.ConfigurationProvider)
           .PaginatedListAsync(request.PageNumber, request.PageSize);

    }
}
