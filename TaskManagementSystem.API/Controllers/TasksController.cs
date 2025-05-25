using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.API.Controllers;

/// <summary>
/// Управление задачами: создание, редактирование, фильтрация, удаление.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TasksController(ITaskService taskService) : ControllerBase
{
    private readonly ITaskService _taskService = taskService;

    /// <summary>
    /// Получить список задач с фильтрацией, сортировкой и пагинацией.
    /// </summary>
    /// <param name="filter">Параметры фильтрации, сортировки и пагинации.</param>
    /// <returns>Список задач с информацией о пользователях.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TaskDto>>> GetAll([FromQuery] TaskFilterDto filter)
    {
        var result = await _taskService.GetFilteredAsync(filter);
        return Ok(result);
    }

    /// <summary>
    /// Получить задачу по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор задачи.</param>
    /// <returns>Информация о задаче.</returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> Get(Guid id)
    {
        var task = await _taskService.GetByIdAsync(id);
        if (task is null) return NotFound();
        return Ok(task);
    }


    /// <summary>
    /// Создать новую задачу.
    /// </summary>
    /// <param name="request">Модель задачи для создания.</param>
    /// <returns>Идентификатор созданной задачи.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        var id = await _taskService.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id }, null);
    }


    /// <summary>
    /// Обновить существующую задачу.
    /// </summary>
    /// <param name="id">Идентификатор задачи.</param>
    /// <param name="request">Модель с обновлёнными данными.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request)
    {
        try
        {
            await _taskService.UpdateAsync(id, request);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Удалить задачу (мягкое удаление).
    /// </summary>
    /// <param name="id">Идентификатор задачи.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _taskService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Изменить статус задачи.
    /// </summary>
    /// <param name="id">Идентификатор задачи.</param>
    /// <param name="status">Новый статус задачи (0 - Новая, 1 - В работе, 2 - Завершена).</param>
    [HttpPatch("{id}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] UserTaskStatus status)
    {
        try
        {
            await _taskService.ChangeStatusAsync(id, status);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
