namespace CreativeColab.Models
{
    public class Reminder
    {
        public int ReminderId { get; set; }
        public int UserId { get; set; }
        public int? GameId { get; set; }
        public int? ProjectId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public string? Note { get; set; }

        // Navigation
        public User User { get; set; }
        public Game? Game { get; set; }
        public Project? Project { get; set; }
    }

}
