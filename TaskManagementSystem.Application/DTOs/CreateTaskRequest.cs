namespace TaskManagementSystem.Application.DTOs;


/// <summary>
///Для создания задачи.
/// </summary>
public class CreateTaskRequest : ITaskRequest
{
    /// <summary>Название задачи.</summary>
    public string Title { get; set; } = null!;

    /// <summary>Описание задачи.</summary>
    public string? Description { get; set; }

    /// <summary>Срок выполнения.</summary>
    public DateTime? DueDate { get; set; }

    /// <summary>ID пользователя, которому назначена задача.</summary>
    public Guid? UserId { get; set; }
}
