using AutoMapper;
using TaskManagementSystem.Application.DTOs.Task;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoTask, TaskResponseDto>();
            CreateMap<CreateTaskDto, TodoTask>();
        }
    }
}