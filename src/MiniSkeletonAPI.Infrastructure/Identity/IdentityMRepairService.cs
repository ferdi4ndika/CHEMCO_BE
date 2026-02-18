using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Mappings;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.MWarnas.Dtos;
using MiniSkeletonAPI.Application.Identity.MWarnas.Queries.GetMWarnasWithPagination;
using MiniSkeletonAPI.Domain.Entities;
using MiniSkeletonAPI.Infrastructure.Data;

namespace MiniSkeletonAPI.Infrastructure.Identity;

public class IdentityMWarnaService : IIdentityMWarnaService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public IdentityMWarnaService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

   


}
