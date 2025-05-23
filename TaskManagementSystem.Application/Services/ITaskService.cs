using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllAsync();
        Task<TaskDto?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(CreateTaskRequest request);
        Task UpdateAsync(Guid id, UpdateTaskRequest request);
        Task DeleteAsync(Guid id);
        Task ChangeStatusAsync(Guid id, UserTaskStatus status);
    }
}
