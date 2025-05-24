namespace TaskManagementSystem.Application.DTOs;

public class UpdateTaskRequest : ITaskRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? UserId { get; set; }
}
