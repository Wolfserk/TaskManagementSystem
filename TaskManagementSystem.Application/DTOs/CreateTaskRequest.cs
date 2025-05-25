namespace TaskManagementSystem.Application.DTOs;


/// <summary>
///Для создания задачи.
/// </summary>
public class CreateTaskRequest : ITaskRequest
{
    /// <summary>Название.</summary>
    public string Title { get; set; } = null!;

    /// <summary>Описание.</summary>
    public string? Description { get; set; }

    /// <summary>Конечный срок.</summary>
    public DateTime? Deadline { get; set; }

    /// <summary>ID пользователя, которому назначена задача.</summary>
    public Guid? UserId { get; set; }
}
