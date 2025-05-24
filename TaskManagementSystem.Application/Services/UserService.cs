using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Domain.Models;


namespace TaskManagementSystem.Application.Services;

public class UserService(ITaskRepository taskRepo) : IUserService
{
    private readonly ITaskRepository _taskRepo = taskRepo;

    public async Task<IEnumerable<TaskDto>> GetUserTasksAsync(Guid userId)
    {
        var filter = new TaskFilter
        {
            UserId = userId,
            Page = 1,
            PageSize = int.MaxValue
        };

        var (tasks, _) = await _taskRepo.GetFilteredAsync(filter);

        return tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            CreatedAt = t.CreatedAt,
            DueDate = t.DueDate,
            UserId = t.UserId,
            UserName = t.User?.Name,
            UserEmail = t.User?.Email
        });
    }
}
