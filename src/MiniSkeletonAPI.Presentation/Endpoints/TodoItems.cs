using Microsoft.AspNetCore.Authorization;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.TodoItems.Commands.CreateTodoItem;
using MiniSkeletonAPI.Application.TodoItems.Commands.DeleteTodoItem;
using MiniSkeletonAPI.Application.TodoItems.Commands.UpdateTodoItem;
using MiniSkeletonAPI.Application.TodoItems.Commands.UpdateTodoItemDetail;
using MiniSkeletonAPI.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using MiniSkeletonAPI.Infrastructure.Identity.Permission;
using System.Text.Json;

namespace MiniSkeletonAPI.Presentation.Endpoints;

public class TodoItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetTodoItemsWithPagination)
            .MapPost(CreateTodoItem)
            .MapPut(UpdateTodoItem, "{id}")
            .MapPut(UpdateTodoItemDetail, "UpdateDetail/{id}")
            .MapDelete(DeleteTodoItem, "{id}");
    }

    [Authorize(Permissions.TodoItems.View)]
    public Task<PaginatedList<TodoItemBriefDto>> GetTodoItemsWithPagination(ISender sender, [AsParameters] GetTodoItemsWithPaginationQuery query)
    {
        return sender.Send(query);
    }

    [Authorize(Permissions.TodoItems.Create)]
    public Task<Guid> CreateTodoItem(ISender sender, CreateTodoItemCommand command)
    {
        return sender.Send(command);
    }
    [Authorize(Permissions.TodoItems.Edit)]

    public async Task<IResult> UpdateTodoItem(ISender sender, Guid id, UpdateTodoItemCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    [Authorize(Permissions.TodoItems.Edit)]
    public async Task<IResult> UpdateTodoItemDetail(ISender sender, Guid id, UpdateTodoItemDetailCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    [Authorize(Permissions.TodoItems.Delete)]
    public async Task<IResult> DeleteTodoItem(ISender sender, Guid id)
    {
        await sender.Send(new DeleteTodoItemCommand(id));
        return Results.NoContent();
    }
}
