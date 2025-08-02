namespace CreativeColab.Models.ViewModels
{
    public class ProjectViewModel
    {
        public List<Project>? Projects { get; set; }
        public Project? Project { get; set; }
        public List<User>? AvailableUsers { get; set; }
        public List<ProjectUser>? ProjectUsers { get; set; }
        public List<ProjectDeadline>? ProjectDeadlines { get; set; }
        public List<Payment>? ProjectPayments { get; set; }
    }
}