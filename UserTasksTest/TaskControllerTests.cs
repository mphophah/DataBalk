using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserTaskApi.Controllers;

public class TaskControllerTests
{
    private readonly TaskController _controller;
    private readonly Mock<ITaskRepository> _taskRepository;

    // Arrange: Set up the test environment
    public TaskControllerTests()
    {
        // Arrange
        _taskRepository = new Mock<ITaskRepository>();
       
        _controller = new TaskController(_taskRepository.Object);
    }

    [Fact]
    public void GetTasks_ReturnsOkResult()
    {
        // Arrange
        _taskRepository.Setup(repo => repo.GetAll()).Returns(new List<UserTaskApi.Controllers.Task>());

        // Act
        var result = _controller.GetTasks() as OkObjectResult;

        // Assert: Check if the result is as expected
        Assert.NotNull(result);
       
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public void GetTask_WithValidId_ReturnsOkResult()
    {
        // Arrange
        int taskId = 1;
        
        _taskRepository.Setup(repo => repo.GetById(taskId)).Returns(new UserTaskApi.Controllers.Task());

        // Act
        var result = _controller.GetTask(taskId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public void GetTask_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        int taskId = 1;
       
        _taskRepository.Setup(repo => repo.GetById(taskId)).Returns((UserTaskApi.Controllers.Task)null);

        // Act
        var result = _controller.GetTask(taskId) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateTask_ReturnsOkResult()
    {
        // Arrange
        var taskDto = new TaskDto
        {
            Title = "Sample Task",
            Description = "Task description",
            AssigneeID = 1,
            DueDate = DateTime.Now
        };

        _taskRepository.Setup(repo => repo.Add(It.IsAny<UserTaskApi.Controllers.Task>())).Verifiable();

        // Act
        var result = _controller.CreateTask(taskDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
       
        Assert.Equal(200, result.StatusCode);
       
        _taskRepository.Verify(repo => repo.Add(It.IsAny<UserTaskApi.Controllers.Task>()), Times.Once);
    }

    [Fact]
    public void UpdateTask_WithValidId_ReturnsOkResult()
    {
        // Arrange
        int taskId = 1;
        
        var taskDto = new TaskDto
        {
            Title = "Updated Task",
            Description = "Updated task description",
            AssigneeID = 2,
            DueDate = DateTime.Now.AddHours(1)
        };

        _taskRepository.Setup(repo => repo.GetById(taskId)).Returns(new UserTaskApi.Controllers.Task());
        
        _taskRepository.Setup(repo => repo.Update(It.IsAny<UserTaskApi.Controllers.Task>())).Verifiable();

        // Act
        var result = _controller.UpdateTask(taskId, taskDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        
        Assert.Equal(200, result.StatusCode);
       
        _taskRepository.Verify(repo => repo.Update(It.IsAny<UserTaskApi.Controllers.Task>()), Times.Once);
    }

    [Fact]
    public void UpdateTask_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        int taskId = 1;
        
        var taskDto = new TaskDto
        {
            Title = "Updated Task",
            Description = "Updated task description",
            AssigneeID = 2,
            DueDate = DateTime.Now.AddHours(1)
        };

        _taskRepository.Setup(repo => repo.GetById(taskId)).Returns((UserTaskApi.Controllers.Task)null);

        // Act
        var result = _controller.UpdateTask(taskId, taskDto) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void DeleteTask_WithValidId_ReturnsOkResult()
    {
        // Arrange
        int taskId = 1;
       
        _taskRepository.Setup(repo => repo.GetById(taskId)).Returns(new UserTaskApi.Controllers.Task());
       
        _taskRepository.Setup(repo => repo.Remove(taskId)).Verifiable();

        // Act
        var result = _controller.DeleteTask(taskId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
       
        Assert.Equal(200, result.StatusCode);
       
        _taskRepository.Verify(repo => repo.Remove(taskId), Times.Once);
    }

    [Fact]
    public void DeleteTask_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        int taskId = 1;
        
        _taskRepository.Setup(repo => repo.GetById(taskId)).Returns((UserTaskApi.Controllers.Task)null);

        // Act
        var result = _controller.DeleteTask(taskId) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
    }
}
