using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MiniSkeletonAPI.Application.Common.Models;

using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Domain.Entities;
using MiniSkeletonAPI.Infrastructure.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MiniSkeletonAPI.Application.Common.Mappings;
using MiniSkeletonAPI.Application.Identity.Users.Queries.GetUsersWithPagination;
using MiniSkeletonAPI.Application.Identity.Roles.Queries.GetRolesWithPagination;
using MiniSkeletonAPI.Application.Identity.Permissions.Dtos;
using MiniSkeletonAPI.Infrastructure.Common.Helpers;
using System.Reflection;
using System.Security.Claims;
using MiniSkeletonAPI.Application.Identity.Users.Dtos;
using MiniSkeletonAPI.Application.Identity.Roles.Dtos;
using Microsoft.EntityFrameworkCore;
using MiniSkeletonAPI.Application.Identity.Images.Dtos;
using MiniSkeletonAPI.Application.Identity.Images.Queries.GetImagesWithPagination;
using System.Data;
using Dapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace MiniSkeletonAPI.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly ApplicationDbContext _context;
    private readonly IDbConnection _connection;
    private readonly IMapper _mapper;
 

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        RoleManager<ApplicationRole> roleManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IMapper mapper,
        IAuthorizationService authorizationService
      //  PlantService plantService
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _connection = context.Database.GetDbConnection();
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _mapper = mapper;
      //  _plantService = plantService;

    }


    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user?.UserName;
    }
    public async Task<User?> GetByIdAsync(string userId)
    {
        var userEntity = await _userManager.FindByIdAsync(userId);
        var user = new User
        {
            UserName = userEntity.UserName,
            Nik = userEntity.Nik
        };

        return user;
    }
    public async Task<User?> GetByNikAsync(string  nik)
    {
        var userEntity = await _userManager.Users
          .FirstOrDefaultAsync(d => d.Nik == nik);

        if (userEntity == null)
            return null;
        var user = new User
        {
            UserName = userEntity.UserName,
            Nik = userEntity.Nik
        };

        return user;
    }
    public async Task<PaginatedList<UserBriefDto>> GetUsersPaginatedAsync(GetUsersWithPaginationQuery request)
    {

        return await _userManager.Users
          .Where(x =>
              string.IsNullOrEmpty(request.SearchTerm) ||
              x.UserName.ToLower().Contains(request.SearchTerm.ToLower()))
          //.Include(u => u.Claims)
          .OrderBy(x => x.Last_Created)
          .ProjectTo<UserBriefDto>(_mapper.ConfigurationProvider)
          .PaginatedListAsync(request.PageNumber, request.PageSize);

    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(User user, string password)
    {
        var userEntity = new ApplicationUser
        {
            UserName = user.UserName,
            Nik = user.Nik,
            Role = user.Role,
            Last_Created = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(userEntity, password);
        if (result.Succeeded)
        {
          //  await Console.Out.WriteLineAsync("Sukses");
            return (Result.Success(), userEntity.Id);

        }
        {
            List<IdentityError> errorList = result.Errors.ToList();
            var errors = string.Join(", ", errorList.Select(e => e.Description));
            return (Result.Failure(errors), userEntity.Id);
        }
    }
    public async Task<(Result Result, string UserId)> UpdateUserAsync(User user, string userId)
    {
        var userEntity = await _userManager.FindByIdAsync(userId);
        if(userEntity.Nik== user.Nik)
        {
            userEntity.Nik = user.Nik;
            userEntity.Role = user.Role;
            userEntity.UserName = user.UserName;
            userEntity.Last_Created = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(userEntity);
            if (result.Succeeded)
            {
                if(user.Password != "")
                {
                    await _userManager.RemovePasswordAsync(userEntity);
                    await _userManager.AddPasswordAsync(userEntity, user.Password);
                }
               
                return (Result.Success(), userEntity.Id);

            }
            else
            {
                List<IdentityError> errorList = result.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));
                return (Result.Failure(errors), userEntity.Id);
            }
        }
        else
        {
            var d = await GetByNikAsync(user.Nik);
            if (d==null)
            {
                userEntity.Nik = user.Nik;
                userEntity.UserName = user.UserName;
                userEntity.Role = user.Role;
                userEntity.Last_Created = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(userEntity);
                if (result.Succeeded)
                {
                    await _userManager.RemovePasswordAsync(userEntity);
                    await _userManager.AddPasswordAsync(userEntity, user.Password);
                    return (Result.Success(), userEntity.Id);

                }
                else
                {
                    List<IdentityError> errorList = result.Errors.ToList();
                    var errors = string.Join("Nik is available", errorList.Select(e => e.Description));
                    return (Result.Failure(errors), userEntity.Id);
                }
            }
            else
            {
                List<IdentityError> errorList = new List<IdentityError>();
                var errors = string.Join("Nik is available", "n");
                return (Result.Failure(errors), userEntity.Id);
            }

        }
      
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }
    public async Task<PaginatedList<RoleBriefDto>> GetRolesPaginatedAsync(GetRolesWithPaginationQuery request)
    {
        return await _roleManager.Roles
            //.Where(x => x. == request)
            .OrderBy(x => x.LastCreated)
            .ProjectTo<RoleBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }

    public async Task<(Result Result, string RoleId)> CreateRoleAsync(Role role)
    {
        var entity = new ApplicationRole(role.Name)
        {
            LastCreated = new DateTime()
        };

        var result = await _roleManager.CreateAsync(entity);
        if (result.Succeeded)
        {
            return (Result.Success(), entity.Id);

        }
        {
            List<IdentityError> errorList = result.Errors.ToList();
            var errors = string.Join(", ", errorList.Select(e => e.Description));
            return (Result.Failure(errors), entity.Id);
        }
    }

    public async Task<(Result Result, string RoleId)> UpdateRoleAsync(Role role, string roleId)
    {
        var entity = await _roleManager.FindByIdAsync(roleId);
        entity.Name = role.Name;

        var result = await _roleManager.UpdateAsync(entity);
        if (result.Succeeded)
        {
            //await _userManager.RemovePasswordAsync(entity);
            //await _userManager.AddPasswordAsync(entity, user.Password);
            return (Result.Success(), entity.Id);
        }
        else
        {
            List<IdentityError> errorList = result.Errors.ToList();
            var errors = string.Join(", ", errorList.Select(e => e.Description));
            return (Result.Failure(errors), entity.Id);
        }
    }

    public async Task<Result> DeleteRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        return role != null ? await DeleteRoleAsync(role) : Result.Success();
    }

    public async Task<Result> DeleteRoleAsync(ApplicationRole role)
    {
        var result = await _roleManager.DeleteAsync(role);

        return result.ToApplicationResult();
    }

    public async Task<(Result Result, string UserId)> AddUserRolesAsync(UserRolesDto userRoles)
    {

        var user = await _userManager.FindByIdAsync(userRoles.UserId);
        foreach (var role in userRoles.RoleIds)
        {
            var entityrole = await _roleManager.FindByIdAsync(role);
            _userManager.AddToRoleAsync(user, entityrole.Name);
        }
        return (Result.Success(), user.Id);

    }

    public async Task<(Result Result, string UserId)> AddUserPermissionsAsync(UserPermissionsDto pmsRole)
    {
        var user = await _userManager.FindByIdAsync(pmsRole.UserId);
        foreach (var permission in pmsRole.Permissions)
        {
            await _userManager.AddPermissionClaim(user, permission);
        }
        return (Result.Success(), user.Id);
    }

    public async Task<(Result Result, string RoleId)> AddRolePermissionsAsync(RolePermissionsDto pmsRole)
    {
        var role = await _roleManager.FindByIdAsync(pmsRole.RoleId);
        foreach (var permission in pmsRole.Permissions)
        {
            await _roleManager.AddPermissionClaim(role, permission);
        }
        return (Result.Success(), role.Id);
    }

    

}