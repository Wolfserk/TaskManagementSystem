using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.DTOs;


/// <summary>
///Фильтрация, сортировка и пагинация задач.
/// </summary>
public class TaskFilterDto
{
    /// <summary>Статус задачи (0 - Новая, 1 - В работе, 2 - Завершена).</summary>
    public UserTaskStatus? Status { get; set; }

    /// <summary>Сортировка по: названию (title), конечному сроку (deadline) и дате создания (createdAt - по умолчанию).</summary>
    public string? SortBy { get; set; } = "createdAt";

    /// <summary>Направление сортировки asc и desc (по умолчанию).</summary>
    public string? SortDirection { get; set; } = "desc";

    /// <summary>Текущая страница.</summary>
    public int Page { get; set; } = 1;

    /// <summary>Размер страницы.</summary>
    public int PageSize { get; set; } = 10;
}
