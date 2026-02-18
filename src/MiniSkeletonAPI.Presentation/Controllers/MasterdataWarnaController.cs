using Microsoft.AspNetCore.Mvc;
using MiniSkeletonAPI.Presentation.Helpers;
using MiniSkeletonAPI.Application.Identity.MWarnas.Queries.GetMWarnasWithPagination;
using MiniSkeletonAPI.Application.Identity.MasterdataMasterdataMWarnas.Commands.CreateMWarna;
using MiniSkeletonAPI.Application.Identity.MasterdataMWarnas.Commands.UpdateMWarna;
using MiniSkeletonAPI.Application.Identity.MasterdataMasterdataMWarnas.Commands.DeleteMWarna;
using Microsoft.AspNetCore.Authorization;

namespace MiniSkeletonAPI.Presentation.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class MasterdataWarnasController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ILogger<MasterdataWarnasController> _logger;

        public MasterdataWarnasController(ISender sender, ILogger<MasterdataWarnasController> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        [HttpGet]
        public Task<IActionResult> GetMasterdataMWarnasWithPagination([FromQuery] GetMWarnasWithPaginationQuery query)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(query), _logger);
        }

        [HttpPost]
        public Task<IActionResult> CreatePlant([FromBody] CreateMWarnaCommand command)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(command), _logger);
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdatePlant(Guid id, [FromBody] UpdateMWarnaCommand command)
        {
            if (id != command.Id)
                return Task.FromResult<IActionResult>(BadRequest());

            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(command), _logger);
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeletePlant(Guid id)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(new DeleteMWarnaCommand(id)), _logger);
        }
    }
}
