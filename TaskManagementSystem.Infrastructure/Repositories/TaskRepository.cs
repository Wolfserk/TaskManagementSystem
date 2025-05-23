using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Infrastructure.Persistence;

namespace TaskManagementSystem.Infrastructure.Repositories
{
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

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
