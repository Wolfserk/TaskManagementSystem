namespace TaskManagementSystem.Application.DTOs;

/// <summary>
///Для обновления задачи.
/// </summary>
public class UpdateTaskRequest : ITaskRequest
{
    /// <summary>Новое название задачи.</summary>
    public string Title { get; set; } = null!;

    /// <summary>Описание задачи.</summary>
    public string? Description { get; set; }

    /// <summary>Новая дата выполнения.</summary>
    public DateTime? Deadline { get; set; }

    /// <summary>ID пользователя, которому назначена задача.</summary>
    public Guid? UserId { get; set; }
}
