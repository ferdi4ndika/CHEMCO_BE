using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniSkeletonAPI.Application.Identity.DataAndons.Commands.CreateDataAndon;
using MiniSkeletonAPI.Application.Identity.DataAndons.Commands.DeleteDataAndon;
using MiniSkeletonAPI.Application.Identity.DataAndons.Commands.UpdateAllDataAndon;
using MiniSkeletonAPI.Application.Identity.DataAndons.Commands.UpdateDataAndon;
using MiniSkeletonAPI.Application.Identity.DataAndons.Queries.ExportDataAndonByDate;
using MiniSkeletonAPI.Application.Identity.DataAndons.Queries.GetDataAndonsWithPagination;
using MiniSkeletonAPI.Presentation.Helpers;

namespace MiniSkeletonAPI.Presentation.Controllers
{
    //[Authorize]
    public class DataAndonsController : ControllerBases
    {
        private readonly ISender _sender;
        private readonly ILogger<DataAndonsController> _logger;

        public DataAndonsController(ISender sender, ILogger<DataAndonsController> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        [HttpGet]
        public Task<IActionResult> GetDataAndonsWithPagination([FromQuery] GetDataAndonsWithPaginationQuery query)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(query), _logger);
        }

        [HttpPost]
        public Task<IActionResult> CreateDataAndon(CreateDataAndonCommand command)
        {

            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(command), _logger);
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateDataAndon(Guid id, [FromBody] UpdateDataAndonCommand command)
        {
            if (id != command.Id)
                return Task.FromResult<IActionResult>(BadRequest());

            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(command), _logger);
        }
        [HttpPut("update-all")]
        public Task<IActionResult> UpdateAllDataAndon()
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(new UpdateAllDataAndonCommand()), _logger);
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteDataAndon(Guid id)
        {
            return RequestHandlerHelper.HandleRequestAsync(() => _sender.Send(new DeleteDataAndonCommand(id)), _logger);
        }


        [HttpGet("export")]
        public async Task<IActionResult> ExportDataAndonByDate(
             [FromQuery] string startDate,
             [FromQuery] string endDate)
        {
            try
            {
                // Parse dates
                if (!DateTime.TryParse(startDate, out var parsedStartDate) ||
                    !DateTime.TryParse(endDate, out var parsedEndDate))
                {
                    return BadRequest(new { message = "Format tanggal tidak valid. Gunakan format: YYYY-MM-DD HH:mm:ss" });
                }

                var utcStartDate = parsedStartDate.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(parsedStartDate, DateTimeKind.Utc)
                    : parsedStartDate.ToUniversalTime();

                var utcEndDate = parsedEndDate.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(parsedEndDate, DateTimeKind.Utc)
                    : parsedEndDate.ToUniversalTime();

                var (fileBytes, fileName, contentType) = await _sender.Send(new ExportDataAndonByDateQuery
                {
                    StartDate = utcStartDate,
                    EndDate = utcEndDate
                });

                // **FIX: Gunakan FileContentResult dengan filename explicit**
                return new FileContentResult(fileBytes, contentType)
                {
                    FileDownloadName = fileName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting data andon");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
