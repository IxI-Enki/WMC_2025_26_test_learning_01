using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions;

/// <summary>
/// Konvertiert Result-Objekte aus der Application Layer in IActionResult für die API.
/// </summary>
public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result,
        ControllerBase controller,
        string? createdAtAction = null,
        object? routeValues = null)
    {
        return result.Status switch
        {
            ResultStatus.Ok => controller.Ok(result.Value),
            ResultStatus.Created => createdAtAction is not null
                ? controller.CreatedAtAction(createdAtAction, routeValues, result.Value)
                : controller.StatusCode(StatusCodes.Status201Created, result.Value),
            ResultStatus.NotFound => controller.NotFound(result.ErrorMessage),
            ResultStatus.ValidationError => controller.BadRequest(result.ErrorMessage),
            ResultStatus.Conflict => controller.Conflict(result.ErrorMessage),
            _ => controller.Problem(
                detail: result.ErrorMessage ?? "An unexpected error occurred.",
                statusCode: 500
            )
        };
    }

    public static IActionResult ToActionResult(this Result result, 
        ControllerBase controller)
    {
        return result.Status switch
        {
            ResultStatus.Ok => controller.NoContent(),
            ResultStatus.NotFound => controller.NotFound(result.ErrorMessage),
            ResultStatus.ValidationError => controller.BadRequest(result.ErrorMessage),
            ResultStatus.Conflict => controller.Conflict(result.ErrorMessage),
            _ => controller.Problem(
                detail: result.ErrorMessage ?? "An unexpected error occurred.",
                statusCode: 500
            )
        };
    }
}

