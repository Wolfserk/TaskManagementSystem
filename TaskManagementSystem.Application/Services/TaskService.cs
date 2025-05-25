using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Domain.Models;
using TaskManagementSystem.Domain.Enums;
using TaskManagementSystem.Application.Interfaces;
using Microsoft.Extensions.Logging;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.Application.Services;

public class TaskService(ITaskRepository taskRepo, 
                         ILogger<TaskService> logger,
                         IUserRepository userRepo) : ITaskService
{
    private readonly ITaskRepository _taskRepo = taskRepo;
    private readonly ILogger<TaskService> _logger = logger;
    private readonly IUserRepository _userRepo = userRepo;

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

    private async Task ValidateUserAsync(Guid? userId)
    {
        if (userId.HasValue)
        {
            _ = await _userRepo.GetByIdAsync(userId.Value) ?? throw new ValidationException($"User with ID {userId} does not exist.");
        }
    }
    public async Task<Guid> CreateAsync(CreateTaskRequest request)
    {

        await ValidateUserAsync(request.UserId);
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
        _logger.LogInformation("Task created: {Title}, Id: {Id}", task.Title, task.Id);
        return task.Id;
    }

    public async Task UpdateAsync(Guid id, UpdateTaskRequest request)
    {
        await ValidateUserAsync(request.UserId);
        var task = await _taskRepo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Task not found");
        task.Title = request.Title;
        task.Description = request.Description;
        task.DueDate = request.DueDate;
        task.UpdatedAt = DateTime.UtcNow;
        task.UserId = request.UserId;

        await _taskRepo.UpdateAsync(task);
        _logger.LogInformation("Task updated: {Id}", id);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _taskRepo.DeleteAsync(id);
        _logger.LogInformation("Task soft deleted: {Id}", id);
    }

    public async Task ChangeStatusAsync(Guid id, UserTaskStatus status)
    {
        if (!Enum.IsDefined(status))
            throw new ValidationException($"Invalid task status: {status}");

        var task = await _taskRepo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Task not found");
        task.Status = status;
        await _taskRepo.UpdateAsync(task);
        _logger.LogInformation("Task status changed: {Id}, NewStatus: {Status}", id, status);
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
