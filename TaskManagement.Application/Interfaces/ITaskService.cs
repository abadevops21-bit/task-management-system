using TaskManagementSystem.Application.DTOs.Task;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateTaskAsync(Guid userId, CreateTaskDto dto);
        Task<List<TaskResponseDto>> GetTasksAsync(Guid userId, bool? isCompleted);
        Task<TaskResponseDto> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto dto);
        Task<bool> DeleteTaskAsync(Guid userId, Guid taskId);
    }
}