using Moq;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Domain.Enums;
using TaskManagementSystem.Domain.Models;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace TaskManagementSystem.Tests;

public class UserServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepoMock = new();
    private readonly UserService _service;

    public UserServiceTests()
    {
        _service = new UserService(_taskRepoMock.Object);
    }

    [Fact]
    public async Task GetUserTasksAsync_ReturnsTasks_WhenUserHasTasks()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com"
        };

        var testTasks = new List<TaskItem>
        {
            new() {
                Id = Guid.NewGuid(),
                Title = "Task 1",
                Description = "Description 1",
                Status = UserTaskStatus.New,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Deadline = DateTime.UtcNow.AddDays(7),
                User = user,
                UserId = userId
            },
            new() {
                Id = Guid.NewGuid(),
                Title = "Task 2",
                Description = "Description 2",
                Status = UserTaskStatus.InProgress,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                Deadline = DateTime.UtcNow.AddDays(5),
                User = user,
                UserId = userId
            }
        };

        _taskRepoMock.Setup(r => r.GetByUserIdAsync(userId))
                    .ReturnsAsync(testTasks);

        // Act
        var result = await _service.GetUserTasksAsync(userId);

        // Assert
        Assert.Equal(2, result.Count());

        var firstTask = result.First();
        Assert.Equal("Task 1", firstTask.Title);
        Assert.Equal("Description 1", firstTask.Description);
        Assert.Equal(UserTaskStatus.New, firstTask.Status);
        Assert.Equal("Test User", firstTask.UserName);
        Assert.Equal("test@example.com", firstTask.UserEmail);
        Assert.Equal(userId, firstTask.UserId);

        _taskRepoMock.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetUserTasksAsync_ReturnsEmptyList_WhenUserHasNoTasks()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _taskRepoMock.Setup(r => r.GetByUserIdAsync(userId))
                    .ReturnsAsync(new List<TaskItem>());

        // Act
        var result = await _service.GetUserTasksAsync(userId);

        // Assert
        Assert.Empty(result);
        _taskRepoMock.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetUserTasksAsync_ReturnsTasksWithoutUserInfo_WhenUserNotLoaded()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var testTasks = new List<TaskItem>
        {
            new() {
                Id = Guid.NewGuid(),
                Title = "Task",
                UserId = userId,
                User = null
            }
        };

        _taskRepoMock.Setup(r => r.GetByUserIdAsync(userId))
                    .ReturnsAsync(testTasks);

        // Act
        var result = await _service.GetUserTasksAsync(userId);
        var taskDto = result.First();

        // Assert
        Assert.Null(taskDto.UserName);
        Assert.Null(taskDto.UserEmail);
        Assert.Equal(userId, taskDto.UserId);
    }

    [Fact]
    public async Task GetUserTasksAsync_ReturnsCorrectDtoMapping()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var testTask = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Test Task",
            Description = "Test Description",
            Status = UserTaskStatus.Completed,
            CreatedAt = DateTime.UtcNow,
            Deadline = DateTime.UtcNow.AddDays(1),
            UserId = userId
        };

        _taskRepoMock.Setup(r => r.GetByUserIdAsync(userId))
                    .ReturnsAsync(new List<TaskItem> { testTask });

        // Act
        var result = await _service.GetUserTasksAsync(userId);
        var taskDto = result.First();

        // Assert
        Assert.Equal(testTask.Id, taskDto.Id);
        Assert.Equal(testTask.Title, taskDto.Title);
        Assert.Equal(testTask.Description, taskDto.Description);
        Assert.Equal(testTask.Status, taskDto.Status);
        Assert.Equal(testTask.CreatedAt, taskDto.CreatedAt);
        Assert.Equal(testTask.Deadline, taskDto.Deadline);
        Assert.Equal(testTask.UserId, taskDto.UserId);
    }
}
