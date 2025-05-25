namespace TaskManagementSystem.Application.DTOs;

/// <summary>
///Ответ с пагинацией.
/// </summary>
public class PagedResult<T>
{
    /// <summary>Список сущностей</summary>
    public IEnumerable<T> Items { get; set; } = [];

    /// <summary>Общее количество записей</summary>
    public int TotalCount { get; set; }

    /// <summary>Текущая страница</summary>
    public int Page { get; set; }

    /// <summary>Размер страницы</summary>
    public int PageSize { get; set; }
}
