using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Permissions.Dtos;
using MiniSkeletonAPI.Application.Identity.Parts.Dtos;
using MiniSkeletonAPI.Application.Identity.Parts.Queries.GetPartsWithPagination;
using MiniSkeletonAPI.Application.Identity.Roles.Dtos;
using MiniSkeletonAPI.Application.Identity.Roles.Queries.GetRolesWithPagination;
using MiniSkeletonAPI.Application.Identity.Users.Dtos;
using MiniSkeletonAPI.Application.Identity.Users.Queries.GetUsersWithPagination;
using MiniSkeletonAPI.Domain.Entities;

namespace MiniSkeletonAPI.Application.Common.Interfaces;

public interface IIdentityMasterdataPartService
{
    //Part

    Task<PartBriefDto> GetPartByIdAsync(string PartId);




}
