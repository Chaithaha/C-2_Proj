namespace CreativeColab.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public int? GameId { get; set; }
        public int? ProjectId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public string? Description { get; set; }
        public string Type { get; set; } = null!; // game, project, product, etc.

        // Navigation
        public User User { get; set; }
        public Game? Game { get; set; }
        public Project? Project { get; set; }
    }

}
