using Microsoft.AspNetCore.Mvc;
using MiniSkeletonAPI.Presentation.Helpers;
using MiniSkeletonAPI.Application.Identity.MRepairs.Queries.GetMRepairsWithPagination;
using MiniSkeletonAPI.Application.Identity.MasterdataMasterdataMRepairs.Commands.CreateMRepair;
using MiniSkeletonAPI.Application.Identity.MasterdataMRepairs.Commands.UpdateMRepair;
using MiniSkeletonAPI.Application.Identity.MasterdataMasterdataMRepairs.Commands.DeleteMRepair;
using Microsoft.AspNetCore.Authorization;

namespace MiniSkeletonAPI.Presentation.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class MasterdataRepairsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ILogger<MasterdataRepairsController> _logger;

        public MasterdataRepairsController(ISender sender, ILogger<MasterdataRepairsController> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        [HttpGet]
        public Task<IActionResult> GetMasterdataMRepairsWithPagination([FromQuery] GetMRepairsWithPaginationQuery query)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(query), _logger);
        }

        [HttpPost]
        public Task<IActionResult> CreatePlant([FromBody] CreateMRepairCommand command)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(command), _logger);
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdatePlant(Guid id, [FromBody] UpdateMRepairCommand command)
        {
            if (id != command.Id)
                return Task.FromResult<IActionResult>(BadRequest());

            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(command), _logger);
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeletePlant(Guid id)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(new DeleteMRepairCommand(id)), _logger);
        }
    }
}
