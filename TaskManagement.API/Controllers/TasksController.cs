using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using TaskManagementSystem.Application.DTOs.Common;
using TaskManagementSystem.Application.DTOs.Task;
using TaskManagementSystem.Application.Interfaces;

namespace TaskManagementSystem.API.Controllers
{
    [EnableRateLimiting("fixed")]
    [ApiController]
    [Route("api/tasks")]
    [Authorize] 
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            var result = await _taskService.CreateTaskAsync(GetUserId(), dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] bool? isCompleted,[FromQuery] PaginationParams pagination)
        {
            var result = await _taskService.GetTasksAsync(GetUserId(),isCompleted,pagination);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTaskDto dto)
        {
            var result = await _taskService.UpdateTaskAsync(GetUserId(), id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _taskService.DeleteTaskAsync(GetUserId(), id);
            return Ok(result);
        }
    }
}