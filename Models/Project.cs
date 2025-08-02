using System.ComponentModel.DataAnnotations;

namespace CreativeColab.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; } = null!;
        
        public string? Description { get; set; }
        
        public int OwnerUserId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        [Required(ErrorMessage = "Status is required")]
        [StringLength(20, ErrorMessage = "Status cannot be longer than 20 characters")]
        public string Status { get; set; } = null!; // e.g. planned, active, completed

        // Navigation
        public User? OwnerUser { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
        public ICollection<ProjectDeadline> ProjectDeadlines { get; set; } = new List<ProjectDeadline>();
        public ICollection<Installment> Installments { get; set; } = new List<Installment>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

}
