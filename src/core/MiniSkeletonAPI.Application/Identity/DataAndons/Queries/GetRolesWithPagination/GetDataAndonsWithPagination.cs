using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Mappings;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.DataAndons.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.DataAndons.Queries.GetDataAndonsWithPagination;

public record GetDataAndonsWithPaginationQuery : IRequest<PaginatedList<DataAndonBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int? Status { get; set; } // tambahkan ini agar bisa difilter

}

public class GetDataAndonsWithPaginationQueryHandler : IRequestHandler<GetDataAndonsWithPaginationQuery, PaginatedList<DataAndonBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDataAndonsWithPaginationQueryHandler(IApplicationDbContext context , IMapper mapper )
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DataAndonBriefDto>> Handle(GetDataAndonsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.DataAndons
           .Where(x => x.Status == 0)
           .OrderByDescending(x => x.CreatedAt)
           .ProjectTo<DataAndonBriefDto>(_mapper.ConfigurationProvider).PaginatedListAsync(request.PageNumber, request.PageSize);
       
    }
}
