
using Microsoft.AspNetCore.Authorization;
using MiniSkeletonAPI.Infrastructure.Identity;
using MiniSkeletonAPI.Infrastructure.Identity.Permission;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Users.Commands.DeleteUser;
using MiniSkeletonAPI.Application.Identity.Users.Commands.CreateUser;
using MiniSkeletonAPI.Application.Identity.Users.Commands.UpdateUser;
using MiniSkeletonAPI.Application.Identity.Users.Queries.GetUsersWithPagination;
using MiniSkeletonAPI.Application.Identity.Permissions.Commands;
using MiniSkeletonAPI.Application.Common.Exceptions;

namespace CleanArchitecture.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetUsersWithPagination)
            .MapPost(CreateUser)
            .MapPut(AddUserPermissions, "Permissions/{userid}")
            .MapPut(UpdateUser, "{id}")
            .MapDelete(DeleteUser, "{id}")
            .MapCustomizedIdentityApi<ApplicationUser>()
            //.HasApiVersion(1.0)
            ;
    }

    //[Authorize(Permissions.Users.View)]
    public Task<PaginatedList<UserBriefDto>> GetUsersWithPagination(ISender sender, [AsParameters] GetUsersWithPaginationQuery query)
    {
        throw new Exception();
        return sender.Send(query);
    }
    [Authorize(Permissions.Users.Create)]
    public Task<string> CreateUser(ISender sender, CreateUserCommand command)
    {
        return sender.Send(command);
    }

    [Authorize(Permissions.Users.Edit)]
    public async Task<IResult> UpdateUser(ISender sender, Guid id, UpdateUserCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    [Authorize(Permissions.Users.Delete)]
    public async Task<IResult> DeleteUser(ISender sender, Guid id)
    {
        await sender.Send(new DeleteUserCommand(id));
        return Results.NoContent();
    }

    [Authorize(Permissions.Users.Edit)]
    public async Task<IResult> AddUserPermissions(ISender sender, Guid userId, AddUserPermissionsCommand command)
    {
        if (userId != command.UserId) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }
}
