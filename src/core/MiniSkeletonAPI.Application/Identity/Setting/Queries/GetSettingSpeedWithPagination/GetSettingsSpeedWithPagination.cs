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

public record GetSpeedsWithPaginationQuery : IRequest<SingleResult<SettingSpeedBriefDto>>
{

}

public class GetSpeedsWithPaginationQueryHandler : IRequestHandler<GetSpeedsWithPaginationQuery, SingleResult<SettingSpeedBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSpeedsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    public async Task<SingleResult<SettingSpeedBriefDto>> Handle(GetSpeedsWithPaginationQuery request, CancellationToken cancellationToken)
    {
       
        var data = await _context.DataCounts
            .AsNoTracking()
            //.Where(x => x.Id == request.Id)
            .ProjectTo<SettingSpeedBriefDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return new SingleResult<SettingSpeedBriefDto>(data);
    }
}
