using MiniSkeletonAPI.Infrastructure.Identity.Permission;
using Microsoft.AspNetCore.Authorization;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.DataAndons.Dtos;
using MiniSkeletonAPI.Application.Identity.DataAndons.Commands.DeleteDataAndon;
using MiniSkeletonAPI.Application.Identity.DataAndons.Commands.UpdateDataAndon;
using MiniSkeletonAPI.Application.Identity.DataAndons.Commands.CreateDataAndon;
using MiniSkeletonAPI.Application.Identity.DataAndons.Queries.GetDataAndonsWithPagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using MiniSkeletonAPI.Application.Identity.DataAndons.Dtos;

namespace MiniSkeletonAPI.Presentation.Endpoints;

public class DataAndons : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetDataAndonsWithPagination)
            .MapPost(CreateDataAndon)
            .MapGet(GetAntiForgeryToken,"token")
            .MapPut(UpdateDataAndon, "{id}")
            .MapDelete(DeleteDataAndon, "{id}");
    }

    // [Authorize]
    public async Task<PaginatedList<DataAndonBriefDto>> GetDataAndonsWithPagination(ISender sender, [AsParameters] GetDataAndonsWithPaginationQuery query)
    {
        return await sender.Send(query);
    }

    // [Authorize(Permissions.DataAndons.Create)]
    [HttpPost]
  //  [ValidateAntiForgeryToken]
   // [IgnoreAntiforgeryToken]
    public async Task<Guid> CreateDataAndon([FromServices] ISender sender,[FromForm]  CreateDataAndonCommand command, HttpContext httpContext)
    {
        return await sender.Send(command);
    }


    // [Authorize(Permissions.DataAndons.Edit)]
    public async Task<IResult> UpdateDataAndon(ISender sender, Guid id, UpdateDataAndonCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    // [Authorize(Permissions.DataAndons.Delete)]
    public async Task<IResult> DeleteDataAndon(ISender sender, Guid id)
    {
        await sender.Send(new DeleteDataAndonCommand(id));
        return Results.NoContent();
    }

    // Endpoint untuk mendapatkan AntiForgery Token
    public IResult GetAntiForgeryToken([FromServices] IAntiforgery antiforgery, HttpContext httpContext)
    {
        var tokens = antiforgery.GetAndStoreTokens(httpContext); // Perbaiki akses HttpContext
        return Results.Ok(new { token = tokens.RequestToken });
    }
}
