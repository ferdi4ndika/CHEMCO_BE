using MiniSkeletonAPI.Infrastructure.Identity.Permission;
using Microsoft.AspNetCore.Authorization;
using MiniSkeletonAPI.Application.Identity.Roles.Commands.CreateRole;
using MiniSkeletonAPI.Application.Identity.Roles.Commands.UpdateRole;
using MiniSkeletonAPI.Application.Identity.Roles.Commands.DeleteRole;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Roles.Queries.GetRolesWithPagination;
using MiniSkeletonAPI.Application.Identity.Permissions.Commands;
using MiniSkeletonAPI.Application.Identity.Roles.Dtos;
using MiniSkeletonAPI.Application.Identity.Users.Commands.AddUserRoles;

namespace MiniSkeletonAPI.Presentation.Endpoints;

public class Roles : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetRolesWithPagination)
            .MapPost(CreateRole)
            .MapPut(AddRolePermissions, "Permissions/{roleId}")
            .MapPut(AddUserRoles, "User/{userId}")
            .MapPut(UpdateRole, "{id}")
            .MapDelete(DeleteRole, "{id}")
            ;
    }

    [Authorize(Permissions.Roles.View)]
    public async Task<PaginatedList<RoleBriefDto>> GetRolesWithPagination(ISender sender, [AsParameters] GetRolesWithPaginationQuery query)
    {
        return await sender.Send(query);
    }

    [Authorize(Permissions.Roles.Create)]
    public async Task<Guid> CreateRole(ISender sender, CreateRoleCommand command)
    {
        return await sender.Send(command);
    }

    [Authorize(Permissions.Roles.Edit)]
    public async Task<IResult> UpdateRole(ISender sender, Guid id, UpdateRoleCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    [Authorize(Permissions.Roles.Delete)]
    public async Task<IResult> DeleteRole(ISender sender, Guid id)
    {
        await sender.Send(new DeleteRoleCommand(id));
        return Results.NoContent();
    }
    [Authorize(Permissions.Roles.Edit)]
    public async Task<IResult> AddUserRoles(ISender sender, Guid userId, AddUserRolesCommand command)
    {
        if (userId != command.UserId) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }
    [Authorize(Permissions.Roles.Edit)]
    public async Task<IResult> AddRolePermissions(ISender sender, Guid roleId, AddRolePermissionsCommand command)
    {
        if (roleId != command.RoleId) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }
}