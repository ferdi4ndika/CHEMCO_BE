using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Mappings;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.MRepairs.Dtos;
using MiniSkeletonAPI.Application.Identity.MRepairs.Queries.GetMRepairsWithPagination;
using MiniSkeletonAPI.Domain.Entities;
using MiniSkeletonAPI.Infrastructure.Data;

namespace MiniSkeletonAPI.Infrastructure.Identity;

public class IdentityMRepairService : IIdentityMRepairService

{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public IdentityMRepairService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


}
