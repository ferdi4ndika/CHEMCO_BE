using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniSkeletonAPI.Application.Identity.MasterdataMasterdataSettings.Commands.CreateSetting;
using MiniSkeletonAPI.Application.Identity.MasterdataMasterdataSettings.Commands.DeleteSetting;
using MiniSkeletonAPI.Application.Identity.MasterdataSettings.Commands.UpdateSetting;
using MiniSkeletonAPI.Application.Identity.MasterdataSpeeds.Commands.UpdateSpeed;
using MiniSkeletonAPI.Application.Identity.Settings.Queries.GetSettingsWithPagination;
using MiniSkeletonAPI.Presentation.Helpers;

namespace MiniSkeletonAPI.Presentation.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class SettingController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ILogger<SettingController> _logger;

        public SettingController(ISender sender, ILogger<SettingController> logger)
        {
            _sender = sender;
            _logger = logger;
        }
 
        [HttpGet]
        public Task<IActionResult> GetSettingsWithPagination([FromQuery] GetSettingsWithPaginationQuery query)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(query), _logger);
        }
        [HttpGet("speed")]
        public Task<IActionResult> GetSettingsSpeedWithPagination([FromQuery] GetSpeedsWithPaginationQuery query)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(query), _logger);
        }
        [HttpPost]
        public Task<IActionResult> CreatePlant([FromBody] CreateSettingCommand command)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(command), _logger);
        }
        [HttpPost("speed")]
        public Task<IActionResult> CreateSettingSpeed([FromBody] UpdateSpeedCommand command)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(command), _logger);
        }
        [HttpPut("{id}")]
        public Task<IActionResult> UpdatePlant(Guid id, [FromBody] UpdateSettingCommand command)
        {
            if (id != command.Id)
                return Task.FromResult<IActionResult>(BadRequest());

            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(command), _logger);
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeletePlant(Guid id)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(new DeleteSettingCommand(id)), _logger);
        }
    }
}
