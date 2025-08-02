using System.ComponentModel.DataAnnotations;

namespace CreativeColab.Models
{
    public class ProjectDeadline
    {
        // Primary key for ProjectDeadline
        [Key]
        public int DeadlineId { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }

        // Navigation
        public Project Project { get; set; }
    }

}

