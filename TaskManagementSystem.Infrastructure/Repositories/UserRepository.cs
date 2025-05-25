using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Infrastructure.Persistence;

namespace TaskManagementSystem.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;
    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Id == userId);
    }
}
