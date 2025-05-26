using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.API.Controllers;

/// <summary>
/// Управление задачами: создание, редактирование, фильтрация, удаление.
/// </summary>
/// <remarks>
/// ## Возможные статусы задач:
/// - 0: Новая
/// - 1: В работе
/// - 2: Завершена
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class TasksController(ITaskService taskService) : ControllerBase
{
    private readonly ITaskService _taskService = taskService;

    /// <summary>
    /// Получить список задач с фильтрацией, сортировкой и пагинацией.
    /// </summary>
    /// <param name="filter">
    /// Параметры фильтрации:
    /// - Status: Фильтр по статусу (0, 1, 2)
    /// - SortBy: Поле для сортировки (title, deadline, createdAt)
    /// - SortDirection: Направление сортировки (asc, desc)
    /// - Page: Номер страницы (начиная с 1)
    /// - PageSize: Количество элементов на странице
    /// </param>
    /// <returns>Список задач с информацией о пользователях и метаданными пагинации.</returns>
    /// <response code="200">Успешный запрос. Возвращает список задач.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TaskDto>>> GetAll([FromQuery] TaskFilterDto filter)
    {
        var result = await _taskService.GetFilteredAsync(filter);
        return Ok(result);
    }


    /// <summary>
    /// Создать новую задачу.
    /// </summary>
    /// <param name="request">Модель задачи для создания (CreateTaskRequest).</param>
    /// <returns>ID созданной задачи.</returns>
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
    /// <param name="id">ID задачи.</param>
    /// <param name="request">Модель с обновлёнными данными (UpdateTaskRequest).</param>
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
    /// <param name="id">ID задачи.</param>
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
    /// <param name="id">ID задачи.</param>
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
}
