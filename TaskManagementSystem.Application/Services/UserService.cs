using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Interfaces;


namespace TaskManagementSystem.Application.Services;

public class UserService(ITaskRepository taskRepo) : IUserService
{
    private readonly ITaskRepository _taskRepo = taskRepo;

    public async Task<IEnumerable<TaskDto>> GetUserTasksAsync(Guid userId)
    {
        var tasks = await _taskRepo.GetByUserIdAsync(userId);

        return tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            CreatedAt = t.CreatedAt,
            Deadline = t.Deadline,
            UserId = t.UserId,
            UserName = t.User?.Name,
            UserEmail = t.User?.Email
        });
    }
}
