using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;

namespace TaskManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("{id}/tasks")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetUserTasks(Guid id)
    {
        var tasks = await _userService.GetUserTasksAsync(id);
        return Ok(tasks);
    }
}
