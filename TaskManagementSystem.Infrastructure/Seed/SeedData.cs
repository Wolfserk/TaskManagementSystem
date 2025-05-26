using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;
using TaskManagementSystem.Infrastructure.Persistence;

namespace TaskManagementSystem.Infrastructure.Seed;

public static class SeedData
{
    public static void EnsureSeeded(AppDbContext context)
    {
        var user1Id = Guid.Parse("f1811537-a05b-49bb-bee9-7a9480267c12");
        var user2Id = Guid.Parse("f67b8dc6-0bee-4732-85fc-ff31a90615ad");

        var user1 = context.Users.Find(user1Id);
        var user2 = context.Users.Find(user2Id);

        if (user1 == null)
        {
            user1 = new User
            {
                Id = user1Id,
                Name = "TestUser1",
                Email = "user1@test.ru"
            };
            context.Users.Add(user1);
        }

        if (user2 == null)
        {
            user2 = new User
            {
                Id = user2Id,
                Name = "TestUser2",
                Email = "user2@test.ru"
            };
            context.Users.Add(user2);
        }

        context.SaveChanges();

        if (!context.Tasks.Any())
        {
            var tasks = new List<TaskItem>
        {
            new()
            {
                Title = "Task1",
                Description = "Description of Task1",
                Status = UserTaskStatus.New,
                UserId = user1.Id
            },
            new()
            {
                Title = "Task2",
                Status = UserTaskStatus.InProgress,
                UserId = user2.Id
            }
        };

            context.Tasks.AddRange(tasks);
            context.SaveChanges();
        }
    }
}
