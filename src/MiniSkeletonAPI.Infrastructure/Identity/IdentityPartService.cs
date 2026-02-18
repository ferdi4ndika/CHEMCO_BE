using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Mappings;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Parts.Dtos;
using MiniSkeletonAPI.Application.Identity.Parts.Queries.GetPartsWithPagination;
using MiniSkeletonAPI.Domain.Entities;
using MiniSkeletonAPI.Infrastructure.Data;

namespace MiniSkeletonAPI.Infrastructure.Identity;

public class IdentityPartService : IIdentityMasterdataPartService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public IdentityPartService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

  
    public async Task<PartBriefDto> GetPartByIdAsync(string partId)
    {
        //var part = await _context.Parts.FindAsync(partId);
        return await _context.Parts
        .Where(p => p.Id == Guid.Parse( partId))
        .ProjectTo<PartBriefDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();
    }


  
}
