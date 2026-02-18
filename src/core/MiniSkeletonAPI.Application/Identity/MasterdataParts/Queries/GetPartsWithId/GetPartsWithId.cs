using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Identity.Parts.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MiniSkeletonAPI.Application.Identity.Parts.Queries.GetPartById
{
    public record GetPartByIdQuery(Guid Id) : IRequest<PartBriefDto>;
    public class GetPartByIdQueryHandler : IRequestHandler<GetPartByIdQuery, PartBriefDto>
    {
        private readonly IIdentityMasterdataPartService _context;

        public GetPartByIdQueryHandler(IIdentityMasterdataPartService context)
        {
            _context = context;
        }

        public async Task<PartBriefDto> Handle(GetPartByIdQuery request, CancellationToken cancellationToken)
        {
     
            var entity = await _context.GetPartByIdAsync(request.Id.ToString());

            if (entity == null)
            {
                return null; 
            }
            return entity;
        }
    }
}
