using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.DTOs
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public UserTaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }

        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
