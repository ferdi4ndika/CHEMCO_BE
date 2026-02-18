using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Mappings;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Settings.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Settings.Queries.GetSettingsWithPagination;

public record GetSettingsWithPaginationQuery : IRequest<PaginatedList<SettingBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; set; }
}

public class GetSettingsWithPaginationQueryHandler : IRequestHandler<GetSettingsWithPaginationQuery, PaginatedList<SettingBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSettingsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    public async Task<PaginatedList<SettingBriefDto>> Handle(GetSettingsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Settings
                    .Where(x =>
                  string.IsNullOrEmpty(request.SearchTerm) ||
                  x.Name.ToLower().Contains(request.SearchTerm.ToLower()))
           .OrderByDescending(x => x.CreatedAt)
           .ProjectTo<SettingBriefDto>(_mapper.ConfigurationProvider)
           .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
