namespace CreativeColab.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; } = null!; // e.g. "user", "designer", "admin"

        // Navigation
        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public ICollection<Project> OwnedProjects { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public DesignerStatus DesignerStatus { get; set; }
    }
}