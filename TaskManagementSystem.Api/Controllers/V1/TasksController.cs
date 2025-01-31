﻿using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Api.Extensions;
using TaskManagementSystem.Application.Contracts.Task.Request;
using TaskManagementSystem.Application.Tasks.Commands;
using TaskManagementSystem.Application.Tasks.Queries;

namespace TaskManagementSystem.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TasksController : BaseController
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator, ILogger<BaseController> logger) : base(logger)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks([FromQuery] int page, int pageSize)
    {
        var query = new GetAllTasks(page, pageSize);
        var response = await _mediator.Send(query);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
    }

    [HttpGet(ApiRoutes.Tasks.IdRoute)]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var query = new GetTaskById() { TaskId = id };
        var response = await _mediator.Send(query);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTask request)
    {
        var userClaim = HttpContext.GetUserProfileIdClaimValue();
        var command = new CreateTaskCommand()
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            IsCompleted = request.IsCompleted,
            CategoryId = request.CategoryId,
            UserId = userClaim
        };

        var result = await _mediator.Send(command);
        return result.IsError ? HandleErrorResponse(result.Errors)
            : CreatedAtAction(nameof(GetTaskById), new { id = result.Payload.Id }, result.Payload);
    }

    [HttpPatch(ApiRoutes.Tasks.IdRoute)]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTask request)
    {
        var userClaim = HttpContext.GetUserProfileIdClaimValue();
        var command = new UpdateTaskCommand()
        {
            Description = request.Description,
            IsCompleted = request.IsCompleted,
            TaskId = id,
            Title = request.Title,
            Priority = request.Priority,
            CategoryId = request.CategoryId,
            UserId = userClaim
        };

        var result = await _mediator.Send(command);
        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }
    
    [HttpDelete(ApiRoutes.Tasks.IdRoute)]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var userClaim = HttpContext.GetUserProfileIdClaimValue();
        var command = new DeleteTaskCommand() 
        { 
            TaskId = id,
            UserId = userClaim
        };
        var resposne = await _mediator.Send(command);
        return resposne.IsError ? HandleErrorResponse(resposne.Errors) : NoContent();
    }
}
