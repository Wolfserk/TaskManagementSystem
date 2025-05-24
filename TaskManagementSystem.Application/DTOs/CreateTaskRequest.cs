namespace TaskManagementSystem.Application.DTOs;

public class CreateTaskRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? UserId { get; set; }
}
