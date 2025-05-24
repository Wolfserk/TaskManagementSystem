using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<TaskDto>> GetUserTasksAsync(Guid userId);
}
