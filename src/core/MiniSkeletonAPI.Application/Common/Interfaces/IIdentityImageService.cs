using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Permissions.Dtos;
using MiniSkeletonAPI.Application.Identity.Images.Dtos;
using MiniSkeletonAPI.Application.Identity.Images.Queries.GetImagesWithPagination;
using MiniSkeletonAPI.Application.Identity.Roles.Dtos;
using MiniSkeletonAPI.Application.Identity.Roles.Queries.GetRolesWithPagination;
using MiniSkeletonAPI.Application.Identity.Users.Dtos;
using MiniSkeletonAPI.Application.Identity.Users.Queries.GetUsersWithPagination;
using MiniSkeletonAPI.Domain.Entities;

namespace MiniSkeletonAPI.Application.Common.Interfaces;

public interface IIdentityImageService
{
    //Image
    Task<(Result Result, Guid ImageId)> CreateImageAsync(Image Image);
    Task<(Result Result, string ImageId)> UpdateImageAsync(Image Image, string ImageId);
    Task DeleteUserPermissionsAsync(string userId);
    Task<ImageBriefDto> GetImageByIdAsync(string code);
    Task<ImageBriefDto> GetImageByIdAsyncdata(string id);
    Task<Result> DeleteImageAsync(string ImageId);
    Task<PaginatedList<UserBriefDto>> GetUsersPaginatedAsync(GetUsersWithPaginationQuery request);
    Task<string> GetNextCode_Async();
    Task<List<string>> GetUserPermissionsAsync(string userId);
    Task<Result> DeletePermissionAsync(string userId);




}
