using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace SpotifyTrackView.Controllers.Api.V1.Shared;

public class BaseApiController: ControllerBase
{
    protected IActionResult ValidationError(string field, string message)
    {
        return BadRequest(new
        {
            status = 400,
            message = "Validation Failed",
            errors = new Dictionary<string, string[]>
            {
                { field, new[] { message } }
            }
        });
    }
}