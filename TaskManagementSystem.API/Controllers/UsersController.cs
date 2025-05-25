using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;

namespace TaskManagementSystem.API.Controllers;

/// <summary>
/// Управление пользователями.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    /// <summary>
    /// Получить список задач конкретного пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Список задач пользователя.</returns>
    [HttpGet("{id}/tasks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetUserTasks(Guid id)
    {
        var tasks = await _userService.GetUserTasksAsync(id);
        return Ok(tasks);
    }
}
