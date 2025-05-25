using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    public List<TaskItem> Tasks { get; set; } = [];
}
