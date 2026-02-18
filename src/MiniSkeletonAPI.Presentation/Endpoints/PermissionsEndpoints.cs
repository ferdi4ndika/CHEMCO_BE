using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniSkeletonAPI.Infrastructure.Identity.Permission;


public class PermissionsEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup("api/Permissions")
            .WithTags("Permissions")
            //.HasApiVersion(2.0)
            //.NewApiVersionSet(2z)
            .MapGet(GetPermissions);

    }
    //[Authorize(Permissions.GetPermissions.View)]
    public async Task<IResult> GetPermissions()
    {
        var json = StaticSerialization.GetFieldFromStaticClass(typeof(Permissions));
        return Results.Ok(json);
    }
}
