using TaskManagementSystem.Application.DTOs.Common;
using TaskManagementSystem.Application.DTOs.Task;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateTaskAsync(Guid userId, CreateTaskDto dto);
        Task<PagedResponse<TaskResponseDto>> GetTasksAsync(Guid userId, string role, bool? isCompleted, PaginationParams pagination);
        Task<TaskResponseDto> GetTasksAsync(Guid userId, Guid taskId);
        Task<TaskResponseDto> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto dto);
        Task<bool> DeleteTaskAsync(Guid userId, Guid taskId);
        Task<TaskResponseDto> ToggleTaskStatusAsync(Guid userId, Guid id);
    }
}