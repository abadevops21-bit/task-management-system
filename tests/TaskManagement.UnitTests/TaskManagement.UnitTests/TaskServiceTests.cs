using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagementSystem.Application.DTOs.Task;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Infrastructure.Data;
using TaskManagementSystem.Infrastructure.Services;
using Xunit;

public class TaskServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TaskService> _logger;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        //  InMemory DB
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        //  AutoMapper (real or mock)
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TodoTask, TaskResponseDto>();
        });
        _mapper = mapperConfig.CreateMapper();

        //  Logger mock
        _logger = new Mock<ILogger<TaskService>>().Object;

        //  Service
        _service = new TaskService(_context, _mapper, _logger);
    }

    [Fact]
    public async Task CreateTask_Should_Work()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var dto = new CreateTaskDto
        {
            Title = "Test Task",
            Description = "Test Desc"
        };

        // Act
        var result = await _service.CreateTaskAsync(userId, dto);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Test Task");

        var dbTask = await _context.Tasks.FirstOrDefaultAsync();
        dbTask.Should().NotBeNull();
    }

    [Fact]
    public async Task ToggleTask_Should_Change_Status()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var task = new TodoTask
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            UserId = userId,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.ToggleTaskStatusAsync(userId, task.Id);

        // Assert
        result.IsCompleted.Should().BeTrue();
    }
}
