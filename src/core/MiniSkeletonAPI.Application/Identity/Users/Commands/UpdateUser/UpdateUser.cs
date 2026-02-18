using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Permissions.Dtos;
using MiniSkeletonAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSkeletonAPI.Application.Identity.Users.Commands.UpdateUser;

public record UpdateUserCommand : IRequest  <(Result Result, string UserId)>
{

    [JsonPropertyName("id")]
    public required Guid Id { get; init; }
    [JsonPropertyName("nik")]
    public required string Nik { get; init; }
    [JsonPropertyName("username")]
    public required string UserName { get; init; }

    [JsonPropertyName("role")]
    public required string Role { get; init; }
    [JsonPropertyName("password")]
    public required string Password { get; init; }

    [JsonPropertyName("permissions")]
    public List<string> Pemission { get; init; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, (Result Result, string UserId)>
{
    private readonly IIdentityService _context;
    private readonly IIdentityImageService _ctx;

    public UpdateUserCommandHandler(
        IIdentityService context,
        IIdentityImageService ctx

        )
    {
        _context = context;
        _ctx = ctx;
    }

    public async Task<(Result Result, string UserId)> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
 
            var user = new User
            {
          
                UserName = request.UserName,
                Nik = request.Nik,
                Role = request.Role,
                Password = request.Password
            };
            var entity = await _context.UpdateUserAsync(user, request.Id.ToString());
            var di = await _ctx.DeletePermissionAsync(request.Id.ToString());
            List<string> pr = new List<string>();
            foreach (var d_pr in request.Pemission)
            {
                pr.Add("Permissions.Data." + d_pr);
            }
             //await _ctx.DeletePermissionAsync(request.Id.ToString() );
            var rolePermissions = new UserPermissionsDto
            {
                UserId = entity.UserId,
                Permissions = pr,
            };
            var d = await _context.AddUserPermissionsAsync(rolePermissions);
            return (entity.Result ,entity.UserId);
            

    }


    }


