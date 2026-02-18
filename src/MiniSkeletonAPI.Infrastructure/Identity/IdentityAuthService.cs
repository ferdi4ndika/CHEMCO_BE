using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Domain.Entities;
using MiniSkeletonAPI.Infrastructure.Data;
using Newtonsoft.Json.Linq;
using static MiniSkeletonAPI.Infrastructure.Identity.Permission.Permissions;

namespace MiniSkeletonAPI.Infrastructure.Identity
{
    public class IdentityAuthService : IIdentityAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IIdentityImageService _daper;

        public IdentityAuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext context, IIdentityImageService daper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _daper = daper;
        }

        public async Task<ApplicationUser?> FindByNikAsync(string nik)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Nik == nik);
        }

        public async Task<bool> ValidatePasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }


        public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var permission = await _daper.GetUserPermissionsAsync(user.Id);
            //var permissionString = "[" + string.Join(", ", permission) + "]";
            var permissionString = "[" + string.Join(", ", permission.Select(p => p.Replace("Permissions.Data.", ""))) + "]";


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(CustomClaimTypes.Id, Guid.NewGuid().ToString()),
                new Claim(CustomClaimTypes.Name, user.UserName),
                new Claim(CustomClaimTypes.Permission, permissionString),
                new Claim(CustomClaimTypes.Role, user.Role),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
                
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<(Result Result, string token)> LoginAsync(string nik, string password)
        {


            var user = await FindByNikAsync(nik);
            if (user == null)
                return (Result.Failure("User not found"), ""); 

            var isValidPassword = await ValidatePasswordAsync(user, password);
            if (!isValidPassword)
                return (Result.Failure("Invalid password"),""); 
            var token = await GenerateJwtTokenAsync(user);
            return (Result.Success(), token); 
        }
      

        public class CustomClaimTypes
        {
            public const string Permission = "permissions";
            public const string Id = "user_id";
            public const string Name = "username";
            public const string Role = "role";
        }

    }
}
