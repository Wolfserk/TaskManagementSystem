namespace TaskManagementSystem.Application.DTOs;

public interface ITaskRequest
{
    string Title { get; set; }
    string? Description { get; set; }
    DateTime? DueDate { get; set; }
}