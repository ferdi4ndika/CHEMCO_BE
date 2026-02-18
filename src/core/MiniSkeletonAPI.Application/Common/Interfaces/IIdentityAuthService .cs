using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Permissions.Dtos;
using MiniSkeletonAPI.Application.Identity.Parts.Dtos;
using MiniSkeletonAPI.Application.Identity.Parts.Queries.GetPartsWithPagination;
using MiniSkeletonAPI.Application.Identity.Roles.Dtos;
using MiniSkeletonAPI.Application.Identity.Roles.Queries.GetRolesWithPagination;
using MiniSkeletonAPI.Application.Identity.Users.Dtos;
using MiniSkeletonAPI.Application.Identity.Users.Queries.GetUsersWithPagination;
using MiniSkeletonAPI.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MiniSkeletonAPI.Application.Identity.Users.Requests;
using MiniSkeletonAPI.Domain.Constants;

namespace MiniSkeletonAPI.Application.Common.Interfaces;

public interface IIdentityAuthService
{

    //Task<User?> FindByNikAsync(string nik);
    //Task<string> GenerateJwtTokenAsync(User user);
    Task<(Result Result, string token)> LoginAsync(string nik, string password);

}
