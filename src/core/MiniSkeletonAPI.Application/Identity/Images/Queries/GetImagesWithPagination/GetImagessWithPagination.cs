using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Images.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Images.Queries.GetImagesWithPagination;

public record GetImagesWithPaginationQuery : IRequest<PaginatedList<ImageBriefDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetImagesWithPaginationQueryHandler : IRequestHandler<GetImagesWithPaginationQuery, PaginatedList<ImageBriefDto>>
{
    private readonly IIdentityImageService _context;

    public GetImagesWithPaginationQueryHandler(IIdentityImageService context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ImageBriefDto>> Handle(GetImagesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        //var data = await _context.GetIm(request);
        return null;
    }
}
