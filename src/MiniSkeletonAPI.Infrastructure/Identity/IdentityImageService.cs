using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Images.Dtos;
using MiniSkeletonAPI.Application.Identity.Images.Queries.GetImagesWithPagination;
using MiniSkeletonAPI.Application.Identity.Users.Queries.GetUsersWithPagination;
using MiniSkeletonAPI.Domain.Entities;
using MiniSkeletonAPI.Infrastructure.Data;
using System.Data;

namespace MiniSkeletonAPI.Infrastructure.Identity;

public class IdentityImageService : IIdentityImageService
{
    private readonly IDbConnection _connection;
    private readonly IMapper _mapper;

    public IdentityImageService(ApplicationDbContext context, IMapper mapper)
    {
        _connection = context.Database.GetDbConnection();
        _mapper = mapper;
    }

    public async Task<(Result Result, Guid ImageId)> CreateImageAsync(Image image)
    {
        var code = await GetNextCode_Async();
        var entity = new ApplicationImage(image.Name ?? "", code)
        {
            CreatedAt = DateTime.Now
        };

        var sqlInsert = "INSERT INTO Images (Id, Name, Code, CreatedAt) VALUES (@Id, @Name, @Code, @CreatedAt)";
        entity.Id = Guid.NewGuid().ToString(); 

        await _connection.ExecuteAsync(sqlInsert, new { Id = entity.Id, Name = entity.Name, Code = entity.Code, CreatedAt = entity.CreatedAt });
        Guid idAsGuid = Guid.Parse(entity.Id);

        return (Result.Success(), idAsGuid);
    }

    public async Task<(Result Result, string ImageId)> UpdateImageAsync(Image image, string imageId)
    {
        var existingImage = await GetImageByIdAsyncdata(imageId);

        if (existingImage == null)
        {
            return (Result.Failure("Image not found"), imageId);
        }

        var codeExists = await GetImageByIdAsync(image.Code) != null;

        var sqlUpdate = "UPDATE Images SET Code = @Code, Name = @Name, CreatedAt = @CreatedAt WHERE Id = @Id";
        await _connection.ExecuteAsync(sqlUpdate, new
        {
            Name = image.Name == "null" ? existingImage.Name : image.Name,
            Code = codeExists ? existingImage.Code : image.Code ?? "",
            CreatedAt = DateTime.Now,
            Id = imageId
        });

        var message = codeExists ? "Code Sudah Tersedia" : "Data berhasil di update";
        return (Result.Success(), $"{imageId} - {message}");
    }



    public async Task<Result> DeleteImageAsync(string imageId)
    {
        var sqlDelete = "DELETE FROM Images WHERE Id = @Id";
        var rowsAffected = await _connection.ExecuteAsync(sqlDelete, new { Id = imageId });

        if (rowsAffected == 0)
        {
            return Result.Failure("Image not found");
        }

        return Result.Success();
    }
    public async Task<Result> DeletePermissionAsync(string userId)
    {
        var sqlQuery = @"DELETE FROM ""AspNetUserClaims"" WHERE ""UserId"" = @UserId";
        var affectedRows = await _connection.ExecuteAsync(sqlQuery, new { UserId = userId });
        //return affectedRows;
        if (affectedRows == 0)
        {
            return Result.Failure("Image not found");
        }
        return Result.Success();
    }

    public async Task<PaginatedList<UserBriefDto>> GetUsersPaginatedAsync(GetUsersWithPaginationQuery request)
    {
        var offset = (request.PageNumber - 1) * request.PageSize;
        //var sqlSelect = @"
        //                    SELECT 
        //                        u.Id,
        //                        u.UserName,
        //                        u.Email,
        //                        u.Nik,
        //                        u.Last_Created,
        //                        GROUP_CONCAT(
        //                            -- Potong 'Permissions.Dashboards.' dan ambil sisanya
        //                            SUBSTR(c.ClaimValue, LENGTH('Permissions.Data.') + 1)
        //                        ) AS Permissions
        //                    FROM 
        //                        AspNetUsers u
        //                    LEFT JOIN 
        //                        AspNetUserClaims c ON u.Id = c.UserId
        //                    WHERE 
        //                        c.ClaimValue LIKE 'Permissions.Data.%' -- Pastikan ClaimValue dimulai dengan 'Permissions.Dashboards.'
        //                    GROUP BY 
        //                        u.Id, u.UserName, u.Email, u.Last_Created
        //                    ORDER BY 
        //                        u.Last_Created
        //                    LIMIT @PageSize OFFSET @Offset;
        //                    ";
        var sqlSelect = @"
                            SELECT 
                                u.""Id"",
                                u.""UserName"",
                                u.""Email"",
                                u.""Nik"",
                                u.""Role"",
                                u.""Last_Created"",
                                STRING_AGG(
                                    SUBSTRING(c.""ClaimValue"" FROM LENGTH('Permissions.Data.') + 1),
                                    ','
                                ) AS ""Permissions""
                            FROM 
                                ""AspNetUsers"" u
                            LEFT JOIN 
                                ""AspNetUserClaims"" c ON u.""Id"" = c.""UserId""
                            WHERE 
                                c.""ClaimValue"" LIKE 'Permissions.Data.%'
                            GROUP BY 
                                u.""Id"", u.""UserName"", u.""Email"", u.""Last_Created""
                            ORDER BY 
                                u.""Last_Created""
                            LIMIT @PageSize OFFSET @Offset;
                        ";

        //var totalSql = "SELECT COUNT(*) FROM AspNetUsers";
        var totalSql = "SELECT COUNT(*) FROM \"AspNetUsers\"";
        var totalCount = await _connection.ExecuteScalarAsync<int>(totalSql);
        var users = await _connection.QueryAsync<UserBriefDto>(
            sqlSelect,
            new { PageSize = request.PageSize, Offset = offset }
        );
        var userBriefDtos = _mapper.Map<IEnumerable<UserBriefDto>>(users);
        return new PaginatedList<UserBriefDto>(userBriefDtos.ToList(), totalCount, request.PageNumber, request.PageSize);
    }


    public async Task<string> GetNextCode_Async()
    {
        var sqlQuery = "SELECT COALESCE(MAX(CAST(code AS INTEGER)), 0) FROM Images WHERE code GLOB '[0-9]*';";
       // await _connection.OpenAsync();
        var result = await _connection.ExecuteScalarAsync(sqlQuery);
        var maxCode = Convert.ToInt32(result);
        var newCode = (maxCode + 1).ToString();

        return newCode;
    }

    public async Task<ImageBriefDto> GetImageByIdAsync(string code)
    {
        var sqlQuery = "SELECT * FROM Images WHERE Code = @Code";
        var image = await _connection.QueryFirstOrDefaultAsync<ImageBriefDto>(sqlQuery, new { Code = code });
        return _mapper.Map<ImageBriefDto>(image);
    }
    public async Task<List<string>> GetUserPermissionsAsync(string userId)
    {
        var sqlQuery = @"SELECT ""ClaimValue"" FROM ""AspNetUserClaims"" WHERE ""UserId"" = @UserId";
        var claimValues = await _connection.QueryAsync<string>(sqlQuery, new { UserId = userId });
        return claimValues.ToList();
    }
    public async Task DeleteUserPermissionsAsync(string userId)
    {
        //var sqlQuery = @"DELETE FROM AspNetUserClaims WHERE UserId  = @UserId";
        var sqlQuery = @"DELETE FROM ""AspNetUserClaims"" WHERE ""UserId"" = @UserId";
        var affectedRows = await _connection.ExecuteAsync(sqlQuery, new { UserId = userId });
        //return affectedRows;
    }

    public async Task<ImageBriefDto> GetImageByIdAsyncdata(string id)
    {
        var sqlQuery = "SELECT * FROM Images WHERE Id = @Id";
        var image = await _connection.QueryFirstOrDefaultAsync<ImageBriefDto>(sqlQuery, new { Id = id });

     /*   if (image == null)
        {

            throw new Exception("Image not found");
        }*/

        return _mapper.Map<ImageBriefDto>(image);
    }

}
