using Microsoft.AspNetCore.Mvc;
using MiniSkeletonAPI.Presentation.Helpers;
using MiniSkeletonAPI.Application.Identity.Parts.Queries.GetPartsWithPagination;
using MiniSkeletonAPI.Application.Identity.MasterdataMasterdataParts.Commands.CreatePart;
using MiniSkeletonAPI.Application.Identity.MasterdataParts.Commands.UpdatePart;
using MiniSkeletonAPI.Application.Identity.MasterdataMasterdataParts.Commands.DeletePart;
using MiniSkeletonAPI.Application.Identity.Parts.Queries.GetPartById;
using Microsoft.AspNetCore.Authorization;

namespace MiniSkeletonAPI.Presentation.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class MasterdataPartsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ILogger<MasterdataPartsController> _logger;

        public MasterdataPartsController(ISender sender, ILogger<MasterdataPartsController> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        [HttpGet]
        public Task<IActionResult> GetMasterdataPartsWithPagination([FromQuery] GetPartsWithPaginationQuery query)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(query), _logger);
        }

        [HttpPost]
        public Task<IActionResult> CreatePlant([FromBody] CreatePartCommand command)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(command), _logger);
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdatePlant(Guid id, [FromBody] UpdatePartCommand command)
        {
            if (id != command.Id)
                return Task.FromResult<IActionResult>(BadRequest());

            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(command), _logger);
        }

        [HttpGet("byid/{id}")]
        public Task<IActionResult> GetPartById(Guid id)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(new GetPartByIdQuery(id)), _logger);
        }


        [HttpDelete("{id}")]
        public Task<IActionResult> DeletePlant(Guid id)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(new DeletePartCommand(id)), _logger);
        }
    }
}
