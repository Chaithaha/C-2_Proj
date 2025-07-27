namespace CreativeColab.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int OwnerUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = null!; // e.g. planned, active, completed

        // Navigation
        public User OwnerUser { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public ICollection<ProjectDeadline> ProjectDeadlines { get; set; }
        public ICollection<Installment> Installments { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }

}
