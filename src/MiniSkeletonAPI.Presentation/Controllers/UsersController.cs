using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniSkeletonAPI.Application.Common.Models;
using MiniSkeletonAPI.Application.Identity.Users.Commands.DeleteUser;
using MiniSkeletonAPI.Application.Identity.Users.Commands.CreateUser;
using MiniSkeletonAPI.Application.Identity.Users.Commands.UpdateUser;
using MiniSkeletonAPI.Application.Identity.Users.Queries.GetUsersWithPagination;
using MiniSkeletonAPI.Application.Identity.Permissions.Commands;
using System;
using System.Threading.Tasks;
using MediatR;
using MiniSkeletonAPI.Infrastructure.Identity.Permission;
using MiniSkeletonAPI.Presentation.Controllers;
using MiniSkeletonAPI.Application.Identity.Users.Commands.Auth;

namespace CleanArchitecture.Web.Controllers
{
  
    //[Authorize] 
    public class UsersController : ControllerBases
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        //[Authorize(Permissions.Users.View)]
        public async Task<ActionResult<PaginatedList<UserBriefDto>>> GetUsersWithPagination([FromQuery] GetUsersWithPaginationQuery query)
        {
            var result = await _sender.Send(query);
            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Permissions.Users.Create)]
        public async Task<ActionResult<Guid>> CreateUser(CreateUserCommand command)
        {
            var userId = await _sender.Send(command);
            return CreatedAtAction(nameof(GetUsersWithPagination), new { id = userId }, userId);
        }



        [HttpPut("{id}")]
        //[Authorize(Permissions.Users.Edit)]
        public async Task<ActionResult<string>> UpdateUser(Guid id, UpdateUserCommand command)
        {
            if (id != command.Id)
            {
                return "User ID tidak cocok.";
            }

          var respose =await _sender.Send(command);

            return respose.UserId;
        }



        [HttpDelete("{id}")]
        //[Authorize(Permissions.Users.Delete)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _sender.Send(new DeleteUserCommand(id));
            return NoContent();
        }

        [HttpPut("Permissions/{userId}")]
        //[Authorize(Permissions.Users.Edit)]
        public async Task<IActionResult> AddUserPermissions(Guid userId, AddUserPermissionsCommand   command)
        {
            if (userId != command.UserId) return BadRequest("User ID tidak cocok.");
            await _sender.Send(command);
            return NoContent();
        }

        [HttpPost("/api/login")]
        public async Task<IActionResult> Login([FromBody] AuthCommand command)
        {
            var (result, token) = await _sender.Send(command);

            if (!result.Succeeded)
            {
                return Unauthorized(new { message = result.Errors });
            }

            return Ok(new
            {
                token = token,
                expiresIn = 3600, 
                tokenType = "Bearer"
            });
        }
    }
}
