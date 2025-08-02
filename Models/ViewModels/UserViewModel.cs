namespace CreativeColab.Models.ViewModels
{
    public class UserViewModel
    {
        public List<User>? Users { get; set; }
        public User? User { get; set; }
        public List<Project>? OwnedProjects { get; set; }
        public List<Bookmark>? UserBookmarks { get; set; }
        public List<Payment>? UserPayments { get; set; }
        public List<Reminder>? UserReminders { get; set; }
        public DesignerStatus? DesignerStatus { get; set; }
    }
}