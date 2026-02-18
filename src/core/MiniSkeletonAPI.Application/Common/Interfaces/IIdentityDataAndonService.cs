using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.DataAndons.Queries.GetDataAndonsWithPagination;
using MiniSkeletonAPI.Application.Identity.Permissions.Dtos;
using MiniSkeletonAPI.Application.Identity.DataAndons.Dtos;
using MiniSkeletonAPI.Application.Identity.Roles.Dtos;
using MiniSkeletonAPI.Application.Identity.Roles.Queries.GetRolesWithPagination;
using MiniSkeletonAPI.Application.Identity.Users.Dtos;
using MiniSkeletonAPI.Application.Identity.Users.Queries.GetUsersWithPagination;
using MiniSkeletonAPI.Domain.Entities;

namespace MiniSkeletonAPI.Application.Common.Interfaces;

public interface IIdentityDataAndonService
{

    Task<Result> UpdateAllDataAndonAsync();

    Task<PaginatedList<DataAndonBriefDto>> GetDataAndonsAsync();
    Task<Result> UpdateDetail(CancellationToken token);
    Task<Result> UpdateCounting(CancellationToken token, bool data, int speeed);
    Task<Result> AddTimeStop(CancellationToken token, int dataTiem);
    Task<List<DataAndonDetailBriefDto>> GetDataAndonAsync(CancellationToken token);

    // Update return type untuk export
    Task<(byte[] fileBytes, string fileName, string contentType)> ExportDataAndonByDateAsync(DateTime startDate, DateTime endDate);



}
