using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetAllAsync();
    Task<PagedResult<TaskDto>> GetFilteredAsync(TaskFilterDto filter);
    Task<TaskDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreateTaskRequest request);
    Task UpdateAsync(Guid id, UpdateTaskRequest request);
    Task DeleteAsync(Guid id);
    Task ChangeStatusAsync(Guid id, UserTaskStatus status);

}
