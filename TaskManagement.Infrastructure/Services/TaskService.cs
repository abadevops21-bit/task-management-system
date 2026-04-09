using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Application.DTOs.Common;
using TaskManagementSystem.Application.DTOs.Task;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Infrastructure.Data;

namespace TaskManagementSystem.Infrastructure.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ApplicationDbContext context, IMapper mapper, ILogger<TaskService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<TaskResponseDto> CreateTaskAsync(Guid userId, CreateTaskDto dto)
        {
            var task = new TodoTask
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Creating task for user {UserId}", userId);
            return _mapper.Map<TaskResponseDto>(task);
        }

        public async Task<PagedResponse<TaskResponseDto>> GetTasksAsync(Guid userId,bool? isCompleted,PaginationParams pagination)
        {
            var query = _context.Tasks
                .AsNoTracking()
                .Where(t => t.UserId == userId);

            if (isCompleted.HasValue)
                query = query.Where(t => t.IsCompleted == isCompleted.Value);

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PagedResponse<TaskResponseDto>
            {
                Data = _mapper.Map<List<TaskResponseDto>>(data),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<TaskResponseDto> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto dto)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null)
                throw new Exception("Task not found");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskResponseDto>(task);
        }

        public async Task<bool> DeleteTaskAsync(Guid userId, Guid taskId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}