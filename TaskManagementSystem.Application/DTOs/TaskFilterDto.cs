using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.DTOs;


/// <summary>
/// Модель для фильтрации и сортировки задач
/// </summary>
public class TaskFilterDto
{
    /// <summary>
    /// Статус задачи (0 - Новая, 1 - В работе, 2 - Завершена)
    /// </summary>
    /// <example>1</example>
    public UserTaskStatus? Status { get; set; }

    /// <summary>
    /// Поле для сортировки (title, deadline, createdAt)
    /// </summary>
    /// <example>deadline</example>
    public string? SortBy { get; set; } = "createdAt";

    /// <summary>
    /// Направление сортировки (asc и desc).
    /// </summary>
    /// <example>asc</example>
    public string? SortDirection { get; set; } = "desc";

    /// <summary>
    /// Текущая страница.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Размер страницы.
    /// </summary>
    public int PageSize { get; set; } = 10;
}
