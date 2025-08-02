using System.ComponentModel.DataAnnotations;

namespace CreativeColab.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string Username { get; set; } = null!;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
        public string Email { get; set; } = null!;
        
        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Password cannot be longer than 255 characters")]
        public string PasswordHash { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; }
        
        [Required(ErrorMessage = "Role is required")]
        [StringLength(20, ErrorMessage = "Role cannot be longer than 20 characters")]
        public string Role { get; set; } = null!; // e.g. "user", "designer", "admin"

        // Navigation
        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
        public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
        public DesignerStatus? DesignerStatus { get; set; }
    }
}