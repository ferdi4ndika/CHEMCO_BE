using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Permissions.Dtos;
using MiniSkeletonAPI.Application.Identity.Roles.Dtos;
using MiniSkeletonAPI.Application.Identity.Roles.Queries.GetRolesWithPagination;
using MiniSkeletonAPI.Application.Identity.Users.Dtos;
using MiniSkeletonAPI.Application.Identity.Users.Queries.GetUsersWithPagination;
using MiniSkeletonAPI.Domain.Entities;

namespace MiniSkeletonAPI.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);
    Task<User?> GetByIdAsync(string userId);
    Task<User?> GetByNikAsync(string nik);
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<(Result Result, string UserId)> UpdateUserAsync(User user, string userId);
    Task<PaginatedList<UserBriefDto>> GetUsersPaginatedAsync(GetUsersWithPaginationQuery request);
    Task<bool> AuthorizeAsync(string userId, string policyName);
    Task<(Result Result, string UserId)> CreateUserAsync(User user,string password);
    Task<Result> DeleteUserAsync(string userId);
    Task<(Result Result, string RoleId)> CreateRoleAsync(Role role);
    Task<(Result Result, string RoleId)> UpdateRoleAsync(Role role, string roleId);
    Task<Result> DeleteRoleAsync(string userId);
    Task<PaginatedList<RoleBriefDto>> GetRolesPaginatedAsync(GetRolesWithPaginationQuery request);

    Task<(Result Result, string UserId)> AddUserRolesAsync(UserRolesDto userRoles);
    Task<(Result Result, string RoleId)> AddRolePermissionsAsync(RolePermissionsDto rolePms);
    Task<(Result Result, string UserId)> AddUserPermissionsAsync(UserPermissionsDto userPms);






}
