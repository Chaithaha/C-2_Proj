using System.ComponentModel.DataAnnotations;

namespace CreativeColab.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        
        public int UserId { get; set; }
        
        public int? GameId { get; set; }
        
        public int? ProjectId { get; set; }
        
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        
        [Required(ErrorMessage = "Payment method is required")]
        [StringLength(50, ErrorMessage = "Payment method cannot be longer than 50 characters")]
        public string PaymentMethod { get; set; } = null!;
        
        public DateTime PaymentDate { get; set; }
        
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Type is required")]
        [StringLength(20, ErrorMessage = "Type cannot be longer than 20 characters")]
        public string Type { get; set; } = null!; // game, project, product, etc.

        // Navigation
        public User? User { get; set; }
        public Game? Game { get; set; }
        public Project? Project { get; set; }
    }

}
