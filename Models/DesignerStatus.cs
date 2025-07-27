namespace CreativeColab.Models
{
    public class DesignerStatus
    {
        public int DesignerStatusId { get; set; }
        public int UserId { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public string? StatusMessage { get; set; }

        // Navigation
        public User User { get; set; }
    }

}
