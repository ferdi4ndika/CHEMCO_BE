using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Identity.Permissions.Dtos;
using MiniSkeletonAPI.Domain.Entities;
using MiniSkeletonAPI.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MiniSkeletonAPI.Application.Identity.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<string>
{
    [JsonPropertyName("nik")]
    public required string Nik{ get; init; }
    [JsonPropertyName("username")]
    public required string UserName { get; init; }

    [JsonPropertyName("role")]
    public required string Role { get; init; }
    [JsonPropertyName("password")]
    public required string Password { get; init; }

    [JsonPropertyName("permissions")]
    public List<string>  Pemission { get; init; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
{
    private readonly IIdentityService _context;
    public CreateUserCommandHandler(
        IIdentityService context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {

        var user = new User
        {
            Nik = request.Nik,
            Role = request.Role,
            UserName = request.UserName,
            Password = request.Password
        };
        List<string> pr = new List<string>();

        foreach(var d_pr in request.Pemission)
        {
            pr.Add("Permissions.Data."+d_pr);
        }
        var nik = await _context.GetByNikAsync(request.Nik);
        if (nik != null)
        {
            return "Nik is available";
        }
        else
        {
            var entity = await _context.CreateUserAsync(user, request.Password);
            var rolePermissions = new UserPermissionsDto
            {
                UserId = entity.UserId,
                Permissions = pr,
            };
            var d = await _context.AddUserPermissionsAsync(rolePermissions);
            return entity.UserId;

        }



    }
}

