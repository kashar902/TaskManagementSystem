﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskManagementSystem.Application.Contracts.Common;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;

namespace TaskManagementSystem.Api.Controllers;
public class BaseController : ControllerBase
{
    private readonly ILogger<BaseController> _logger;

    public BaseController(ILogger<BaseController> logger)
    {
        _logger = logger;
    }

    protected IActionResult HandleErrorResponse(List<Error> errors)
    {
        ErrorResponse apiError = new();
        if (errors.Any(e => e.Code == ErrorCode.NotFound))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound);

            apiError.StatusCode = 404;
            apiError.StatusPhrase = "Not Found";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return NotFound(apiError);
        }

        if (errors.Any(e => e.Code == ErrorCode.IdentityUserAlreadyExists))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.IdentityUserAlreadyExists);

            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Request";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return BadRequest(apiError);
        }

        if (errors.Any(e => e.Code == ErrorCode.IdentityCreationFailed))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.IdentityCreationFailed);

            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Request";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return BadRequest(apiError);
        }

        if (errors.Any(e => e.Code == ErrorCode.IncorrectPassword))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.IncorrectPassword);

            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Request";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return BadRequest(apiError);
        }

        if (errors.Any(e => e.Code == ErrorCode.IdentityUserNotFound))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.IdentityUserNotFound);

            apiError.StatusCode = 404;
            apiError.StatusPhrase = "Not Found";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return NotFound(apiError);
        }

        if (errors.Any(e => e.Code == ErrorCode.UnAuthorizedAccountRemoval))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.UnAuthorizedAccountRemoval);

            apiError.StatusCode = 403;
            apiError.StatusPhrase = "Unauthenticated";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return new ObjectResult(apiError)
            {
                StatusCode = 403
            };
        }

        if (errors.Any(e => e.Code == ErrorCode.UserNotAllowed))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.UserNotAllowed);

            apiError.StatusCode = 403;
            apiError.StatusPhrase = "Forbidden Request";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return new ObjectResult(apiError)
            {
                StatusCode = 403
            };
        }

        apiError.StatusCode = 500;
        apiError.StatusPhrase = "Server Error";
        apiError.Timestamp = DateTime.Now;
        apiError.Errors.Add("Unknown Error");

        _logger.LogError(JsonSerializer.Serialize(apiError));

        return StatusCode(500, apiError);
    }
}
