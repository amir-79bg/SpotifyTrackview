using Microsoft.AspNetCore.Mvc;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace SpotifyTrackView.Controllers.Api.V1.Shared;

public class BaseApiController: ControllerBase
{
    protected IActionResult ValidationError(IEnumerable<ValidationFailure> errors)
    {
        foreach (var error in errors)
        {
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return ValidationProblem(ModelState);
    }


    protected IActionResult Success()
    {
        return Ok(new
        {
            Success = true
        });
    }
}