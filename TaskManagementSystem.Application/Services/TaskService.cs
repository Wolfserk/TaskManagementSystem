using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Domain.Models;
using TaskManagementSystem.Domain.Enums;
using TaskManagementSystem.Application.Interfaces;

namespace TaskManagementSystem.Application.Services;

public class TaskService(ITaskRepository taskRepo) : ITaskService
{
    private readonly ITaskRepository _taskRepo = taskRepo;

    public async Task<IEnumerable<TaskDto>> GetAllAsync()
    {
        var tasks = await _taskRepo.GetAllAsync();

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

    public async Task<TaskDto?> GetByIdAsync(Guid id)
    {
        var task = await _taskRepo.GetByIdAsync(id);
        if (task is null) return null;

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            DueDate = task.DueDate,
            UserId = task.UserId,
            UserName = task.User?.Name,
            UserEmail = task.User?.Email
        };
    }

    public async Task<Guid> CreateAsync(CreateTaskRequest request)
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow,
            Status = UserTaskStatus.New,
            UserId = request.UserId
        };

        await _taskRepo.AddAsync(task);
        return task.Id;
    }

    public async Task UpdateAsync(Guid id, UpdateTaskRequest request)
    {
        var task = await _taskRepo.GetByIdAsync(id);
        if (task is null) throw new KeyNotFoundException("Task not found");

        task.Title = request.Title;
        task.Description = request.Description;
        task.DueDate = request.DueDate;
        task.UpdatedAt = DateTime.UtcNow;
        task.UserId = request.UserId;

        await _taskRepo.UpdateAsync(task);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _taskRepo.DeleteAsync(id);
    }

    public async Task ChangeStatusAsync(Guid id, UserTaskStatus status)
    {
        var task = await _taskRepo.GetByIdAsync(id);
        if (task is null) throw new KeyNotFoundException("Task not found");

        task.Status = status;
        await _taskRepo.UpdateAsync(task);
    }

    public async Task<PagedResult<TaskDto>> GetFilteredAsync(TaskFilterDto dto)
    {
        var filter = new TaskFilter
        {
            Status = dto.Status,
            UserId = dto.UserId,
            SortBy = dto.SortBy ?? "createdAt",
            SortDirection = dto.SortDirection ?? "desc",
            Page = dto.Page,
            PageSize = dto.PageSize
        };

        var (tasks, total) = await _taskRepo.GetFilteredAsync(filter);

        var result = tasks.Select(t => new TaskDto
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

        return new PagedResult<TaskDto>
        {
            Items = result,
            TotalCount = total,
            Page = dto.Page,
            PageSize = dto.PageSize
        };
    }
}
