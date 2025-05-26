using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;
using TaskManagementSystem.Infrastructure.Persistence;

namespace TaskManagementSystem.Infrastructure.Seed;

public static class SeedData
{
    public static void EnsureSeeded(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            var user1 = new User { Id = Guid.Parse("f1811537-a05b-49bb-bee9-7a9480267c12"), Name = "TestUser1", Email = "user1@test.ru" };
            var user2 = new User { Id = Guid.Parse("f67b8dc6-0bee-4732-85fc-ff31a90615ad"), Name = "TestUser2", Email = "user2@test.ru" };

            var tasks = new List<TaskItem>
            {
                new() {
                    Title = "Task1",
                    Description = "Description of Task1",
                    Status = UserTaskStatus.New,
                    User = user1
                },
                new() {
                    Title = "Task2",
                    Status = UserTaskStatus.InProgress,
                    User = user2
                }
            };

            context.Users.AddRange(user1, user2);
            context.Tasks.AddRange(tasks);
            context.SaveChanges();
        }
    }
}
