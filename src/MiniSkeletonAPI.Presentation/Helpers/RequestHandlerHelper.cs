using Microsoft.AspNetCore.Mvc;

namespace MiniSkeletonAPI.Presentation.Helpers
{
    public static class RequestHandlerHelper
    {
        public static async Task<IActionResult> HandleRequestAsync<T>(Func<Task<T>> action, ILogger logger)
        {
            try
            {
                var result = await action();
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during the request.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public static async Task<IActionResult> HandleRequestAsync(Func<Task> action, ILogger logger)
        {
            try
            {
                await action();
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during the request.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
