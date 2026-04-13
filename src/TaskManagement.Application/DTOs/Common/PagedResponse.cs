namespace TaskManagementSystem.Application.DTOs.Common
{
    // A generic class to represent paginated responses
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; }

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}