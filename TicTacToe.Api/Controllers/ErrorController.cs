using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TicTacToe.Api.Controllers;

[ApiController]
public class ErrorController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/error")]
    public IActionResult HandleError()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is KeyNotFoundException)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }

        if (exception is ArgumentException || exception is InvalidOperationException)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }

        return StatusCode(500, new
        {
            message = "An unexpected error occurred."
        });
    }
}