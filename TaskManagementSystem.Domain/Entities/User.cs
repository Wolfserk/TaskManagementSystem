namespace TaskManagementSystem.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    public List<TaskItem> Tasks { get; set; } = [];
}
