using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Domain.Models;
using TaskManagementSystem.Infrastructure.Persistence;

namespace TaskManagementSystem.Infrastructure.Repositories;

public class TaskRepository(AppDbContext context) : ITaskRepository
{
    private readonly AppDbContext _context = context;

    public async Task<TaskItem?> GetByIdAsync(Guid id) =>
        await _context.Tasks.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);

    public async Task<IEnumerable<TaskItem>> GetAllAsync() =>
        await _context.Tasks.Include(t => t.User).ToListAsync();

    public async Task AddAsync(TaskItem task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task is null) return;

        task.IsDeleted = true;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Tasks
            .Where(t => t.UserId == userId)
            .Include(t => t.User)
            .ToListAsync();
    }

    public async Task<(IEnumerable<TaskItem> Tasks, int TotalCount)> GetFilteredAsync(TaskFilter filter)
    {
        var query = _context.Tasks
            .Include(t => t.User)
            .AsQueryable();

        if (filter.Status != null)
            query = query.Where(t => t.Status == filter.Status);

        query = filter.SortBy.ToLower() switch
        {
            "title" => filter.SortDirection == "asc"
                ? query.OrderBy(t => t.Title)
                : query.OrderByDescending(t => t.Title),
            "deadline" => filter.SortDirection == "asc"
                ? query.OrderBy(t => t.Deadline)
                : query.OrderByDescending(t => t.Deadline),
            _ => filter.SortDirection == "asc"
                ? query.OrderBy(t => t.CreatedAt)
                : query.OrderByDescending(t => t.CreatedAt),
        };

        var total = await query.CountAsync();
        var paged = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return (paged, total);
    }
}
