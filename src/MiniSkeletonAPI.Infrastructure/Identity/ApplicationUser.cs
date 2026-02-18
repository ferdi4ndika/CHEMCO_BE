using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MiniSkeletonAPI.Application.Identity.Parts.Dtos;
using MiniSkeletonAPI.Application.Identity.Users.Queries.GetUsersWithPagination;
using MiniSkeletonAPI.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace MiniSkeletonAPI.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public DateTime Last_Created { get; set; }
    public string? RefreshToken { get; set; }
    public string Nik { get; set; }
    public string Role { get; set; }
    [NotMapped] 
    public override string? Email { get; set; } = null;

    [NotMapped]
    public override string? NormalizedEmail { get; set; } = null;

    [NotMapped]
    public override bool EmailConfirmed { get; set; } = false;
    public DateTime RefreshTokenExpiryTime { get; set; }
    [NotMapped]
    public ICollection<string>? Roles { get; set; }
    public ICollection<ApplicationUserRole>? UserRoles { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ApplicationUser, User>();
            CreateMap<ApplicationUser, UserBriefDto>();
          

        }
    }
}
