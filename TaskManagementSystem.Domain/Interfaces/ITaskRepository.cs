using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Models;


namespace TaskManagementSystem.Domain.Interfaces;

public interface ITaskRepository
{
    Task<(IEnumerable<TaskItem> Tasks, int TotalCount)> GetFilteredAsync(TaskFilter filter);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(Guid id);
}
