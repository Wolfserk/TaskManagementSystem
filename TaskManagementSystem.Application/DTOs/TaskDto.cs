using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.DTOs;

public class TaskDto
{
    /// <summary>ID задачи.</summary>
    public Guid Id { get; set; }

    /// <summary>Название.</summary>
    public string Title { get; set; } = null!;

    /// <summary>Описание.</summary>
    public string? Description { get; set; }

    /// <summary>Статус.</summary>
    public UserTaskStatus Status { get; set; }

    /// <summary>Дата создания.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Конечный срок.</summary>
    public DateTime? Deadline { get; set; }

    /// <summary>ID пользователя, которому назначена задача.</summary>
    public Guid? UserId { get; set; }

    /// <summary>Имя пользователя.</summary>
    public string? UserName { get; set; }

    /// <summary>Email пользователя.</summary>
    public string? UserEmail { get; set; }
}
