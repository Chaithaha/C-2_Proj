using System.ComponentModel.DataAnnotations;

namespace CreativeColab.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }
        
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; } = null!;
        
        [Required(ErrorMessage = "Genre is required")]
        [StringLength(50, ErrorMessage = "Genre cannot be longer than 50 characters")]
        public string Genre { get; set; } = null!;
        
        [Required(ErrorMessage = "Platform is required")]
        [StringLength(50, ErrorMessage = "Platform cannot be longer than 50 characters")]
        public string Platform { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? Developer { get; set; }

        // Navigation
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public ICollection<GamePrice> GamePrices { get; set; } = new List<GamePrice>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
    }

}
