using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Models;
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

namespace MiniSkeletonAPI.Application.Identity.Users.Commands.Auth;

public record AuthCommand : IRequest<(Result Result, string token)>
{
    [JsonPropertyName("nik")]
    public required string Nik { get; init; }

    [JsonPropertyName("password")]
    public required string Password { get; init; }
}

public class AuthCommandHandler : IRequestHandler<AuthCommand,(Result Result, string token)>
{
    private readonly IIdentityAuthService _identityService;

    public AuthCommandHandler(IIdentityAuthService identityService)
    {
        _identityService = identityService;
    }

    public async Task<(Result Result, string token)> Handle(AuthCommand request, CancellationToken cancellationToken)
    {

        return await _identityService.LoginAsync(request.Nik, request.Password);
    }
}


