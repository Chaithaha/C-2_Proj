namespace CreativeColab.Models
{
    public class ProjectUser
    {
        public int ProjectUserId { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; } = null!; // designer, collaborator, client

        // Navigation
        public Project Project { get; set; }
        public User User { get; set; }
    }

}
