using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Domain.Models
{
    public class TaskFilter
    {
        public UserTaskStatus? Status { get; set; }
        public Guid? UserId { get; set; }
        public string SortBy { get; set; } = "createdAt";
        public string SortDirection { get; set; } = "desc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
