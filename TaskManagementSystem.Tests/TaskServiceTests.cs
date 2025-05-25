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

public class TaskServiceTests
{

    private readonly Mock<ITaskRepository> _taskRepoMock = new();
    private readonly Mock<ILogger<TaskService>> _loggerMock = new();
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly TaskService _service;


    public TaskServiceTests()
    {
        _service = new TaskService(_taskRepoMock.Object, _loggerMock.Object, _userRepoMock.Object);
    }


    [Fact]
    public async Task GetFilteredAsync_ReturnsExpectedTasks()
    {
        // Arrange
        var taskList = new List<TaskItem>
        {
            new() {
                Id = Guid.NewGuid(),
                Title = "Test Task",
                Status = UserTaskStatus.New,
                CreatedAt = DateTime.UtcNow
            }
        };

        _taskRepoMock.Setup(r => r.GetFilteredAsync(It.IsAny<TaskFilter>()))
                    .ReturnsAsync((taskList, 1));

        var filter = new TaskFilterDto
        {
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _service.GetFilteredAsync(filter);

        // Assert
        Assert.Single(result.Items);
        Assert.Equal("Test Task", result.Items.First().Title);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task ChangeStatusAsync_UpdatesStatus_WhenTaskExists()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new TaskItem
        {
            Id = taskId,
            Title = "Old Task",
            Status = UserTaskStatus.New
        };

        _taskRepoMock.Setup(r => r.GetByIdAsync(taskId))
                    .ReturnsAsync(task);

        // Act
        await _service.ChangeStatusAsync(taskId, UserTaskStatus.Completed);

        // Assert
        Assert.Equal(UserTaskStatus.Completed, task.Status);
        _taskRepoMock.Verify(r => r.UpdateAsync(task), Times.Once);
        VerifyLog(LogLevel.Information, $"Task status changed: {taskId}, NewStatus: {UserTaskStatus.Completed}");
    }

    [Fact]
    public async Task ChangeStatusAsync_ThrowsException_WhenTaskNotFound()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _taskRepoMock.Setup(r => r.GetByIdAsync(taskId))
                    .ReturnsAsync((TaskItem?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.ChangeStatusAsync(taskId, UserTaskStatus.InProgress));
    }

    [Fact]
    public async Task CreateAsync_CreatesTask_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };

        _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                    .ReturnsAsync(user);

        var request = new CreateTaskRequest
        {
            Title = "New Task",
            Description = "Test Description",
            UserId = userId,
            Deadline = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var resultId = await _service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, resultId);
        _taskRepoMock.Verify(r => r.AddAsync(It.Is<TaskItem>(t =>
            t.Title == request.Title &&
            t.Description == request.Description &&
            t.UserId == userId &&
            t.Status == UserTaskStatus.New
        )), Times.Once);
        VerifyLog(LogLevel.Information, $"Task created: {request.Title}, Id: {resultId}");
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                    .ReturnsAsync((User?)null);

        var request = new CreateTaskRequest
        {
            Title = "Task",
            Description = "Desc",
            UserId = userId
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.CreateAsync(request));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTask_WhenTaskExists()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var user = new User { Id = Guid.NewGuid(), Name = "Test User", Email = "test@example.com" };
        var task = new TaskItem
        {
            Id = taskId,
            Title = "Test Task",
            Description = "Test Description",
            Status = UserTaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            User = user,
            UserId = user.Id
        };

        _taskRepoMock.Setup(r => r.GetByIdAsync(taskId))
                     .ReturnsAsync(task);

        // Act
        var result = await _service.GetByIdAsync(taskId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(task.Title, result.Title);
        Assert.Equal(user.Name, result.UserName);
        Assert.Equal(user.Email, result.UserEmail);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _taskRepoMock.Setup(r => r.GetByIdAsync(taskId))
                     .ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _service.GetByIdAsync(taskId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesTask_WhenTaskAndUserExist()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var existingTask = new TaskItem
        {
            Id = taskId,
            Title = "Old Title",
            Description = "Old Description",
            Status = UserTaskStatus.New,
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var user = new User { Id = userId };
        _taskRepoMock.Setup(r => r.GetByIdAsync(taskId))
                     .ReturnsAsync(existingTask);
        _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                     .ReturnsAsync(user);

        var request = new UpdateTaskRequest
        {
            Title = "New Title",
            Description = "New Description",
            Deadline = DateTime.UtcNow.AddDays(7),
            UserId = userId
        };

        // Act
        await _service.UpdateAsync(taskId, request);

        // Assert
        _taskRepoMock.Verify(r => r.UpdateAsync(It.Is<TaskItem>(t =>
            t.Id == taskId &&
            t.Title == request.Title &&
            t.Description == request.Description &&
            t.Deadline == request.Deadline &&
            t.UserId == userId &&
            t.UpdatedAt != null
        )), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenTaskNotFound()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                    .ReturnsAsync(new User { Id = userId });

        _taskRepoMock.Setup(r => r.GetByIdAsync(taskId))
                    .ReturnsAsync((TaskItem?)null);

        var request = new UpdateTaskRequest
        {
            Title = "Title",
            Description = "Description",
            UserId = userId 
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateAsync(taskId, request));
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenUserDoesNotExist()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var existingTask = new TaskItem { Id = taskId };

        _taskRepoMock.Setup(r => r.GetByIdAsync(taskId))
                     .ReturnsAsync(existingTask);
        _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                     .ReturnsAsync((User?)null);

        var request = new UpdateTaskRequest
        {
            Title = "Title",
            Description = "Description",
            UserId = userId
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.UpdateAsync(taskId, request));
    }

    [Fact]
    public async Task DeleteAsync_DeletesTask_WhenTaskExists()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _taskRepoMock.Setup(r => r.DeleteAsync(taskId))
                     .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(taskId);

        // Assert
        _taskRepoMock.Verify(r => r.DeleteAsync(taskId), Times.Once);
        VerifyLog(LogLevel.Information, $"Task soft deleted: {taskId}");
    }

    [Fact]
    public async Task ChangeStatusAsync_ThrowsException_WhenStatusIsInvalid()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var invalidStatus = (UserTaskStatus)999;

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.ChangeStatusAsync(taskId, invalidStatus));
    }

    [Fact]
    public async Task CreateAsync_CreatesTaskWithNewStatus_WhenNoStatusProvided()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                     .ReturnsAsync(user);

        var request = new CreateTaskRequest
        {
            Title = "Title",
            Description = "Description",
            UserId = userId
        };

        // Act
        var resultId = await _service.CreateAsync(request);

        // Assert
        _taskRepoMock.Verify(r => r.AddAsync(It.Is<TaskItem>(t =>
            t.Status == UserTaskStatus.New
        )), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_SetsCreatedAtToUtcNow()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userRepoMock.Setup(r => r.GetByIdAsync(userId))
                     .ReturnsAsync(user);

        var request = new CreateTaskRequest
        {
            Title = "Title",
            Description = "Description",
            UserId = userId
        };

        var testStartTime = DateTime.UtcNow;

        // Act
        await _service.CreateAsync(request);

        // Assert
        _taskRepoMock.Verify(r => r.AddAsync(It.Is<TaskItem>(t =>
            t.CreatedAt >= testStartTime &&
            t.CreatedAt <= DateTime.UtcNow
        )), Times.Once);
    }

    [Fact]
    public async Task GetFilteredAsync_AppliesCorrectFilterParameters()
    {
        // Arrange
        var filterDto = new TaskFilterDto
        {
            Status = UserTaskStatus.Completed,
            SortBy = "deadline",
            SortDirection = "asc",
            Page = 2,
            PageSize = 20
        };

        var taskList = new List<TaskItem> { new() { Id = Guid.NewGuid() } };
        _taskRepoMock.Setup(r => r.GetFilteredAsync(It.IsAny<TaskFilter>()))
                     .ReturnsAsync((taskList, 1));

        // Act
        await _service.GetFilteredAsync(filterDto);

        // Assert
        _taskRepoMock.Verify(r => r.GetFilteredAsync(It.Is<TaskFilter>(f =>
            f.Status == UserTaskStatus.Completed &&
            f.SortBy == "deadline" &&
            f.SortDirection == "asc" &&
            f.Page == 2 &&
            f.PageSize == 20
        )), Times.Once);
    }

    private void VerifyLog(LogLevel level, string message)
    {
        _loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(message)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }
}