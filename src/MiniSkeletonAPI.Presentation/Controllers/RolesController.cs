//using MiniSkeletonAPI.Infrastructure.Identity.Permission;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MiniSkeletonAPI.Application.Identity.Roles.Commands.CreateRole;
//using MiniSkeletonAPI.Application.Identity.Roles.Commands.UpdateRole;
//using MiniSkeletonAPI.Application.Identity.Roles.Commands.DeleteRole;
//using MiniSkeletonAPI.Application.Common.Models;
//using MiniSkeletonAPI.Application.Identity.Roles.Queries.GetRolesWithPagination;
//using MiniSkeletonAPI.Application.Identity.Permissions.Commands;
//using MiniSkeletonAPI.Application.Identity.Roles.Dtos;
//using MiniSkeletonAPI.Application.Identity.Users.Commands.AddUserRoles;
//using System;
//using System.Threading.Tasks;
//using MediatR;

//namespace MiniSkeletonAPI.Presentation.Controllers
//{

//    [Authorize] // Anda bisa menambahkan aturan otorisasi umum di sini
//    public class RolesController : ControllerBases
//    {
//        private readonly ISender _sender;

//        public RolesController(ISender sender)
//        {
//            _sender = sender;
//        }

//        [HttpGet]
//        [Authorize]
//        public async Task<ActionResult<PaginatedList<RoleBriefDto>>> GetRolesWithPagination([FromQuery] GetRolesWithPaginationQuery query)
//        {
//            var result = await _sender.Send(query);
//            return Ok(result);
//        }

//        [HttpPost]
//        [Authorize]
//        public async Task<ActionResult<Guid>> CreateRole(CreateRoleCommand command)
//        {
//            var roleId = await _sender.Send(command);
//            return CreatedAtAction(nameof(GetRolesWithPagination), new { id = roleId }, roleId);
//        }

//        [HttpPut("{id}")]
//        [Authorize(Permissions.Roles.Edit)]
//        public async Task<IActionResult> UpdateRole(Guid id, UpdateRoleCommand command)
//        {
//            if (id != command.Id) return BadRequest();
//            await _sender.Send(command);
//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        [Authorize(Permissions.Roles.Delete)]
//        public async Task<IActionResult> DeleteRole(Guid id)
//        {
//            await _sender.Send(new DeleteRoleCommand(id));
//            return NoContent();
//        }

//        [HttpPut("Permissions/{roleId}")]
//        [Authorize(Permissions.Roles.Edit)]
//        public async Task<IActionResult> AddRolePermissions(Guid roleId, AddRolePermissionsCommand command)
//        {
//            if (roleId != command.RoleId) return BadRequest();
//            await _sender.Send(command);
//            return NoContent();
//        }

//        [HttpPut("User/{userId}")]
//        [Authorize(Permissions.Roles.Edit)]
//        public async Task<IActionResult> AddUserRoles(Guid userId, AddUserRolesCommand command)
//        {
//            if (userId != command.UserId) return BadRequest();
//            await _sender.Send(command);
//            return NoContent();
//        }
//    }
//}
