namespace CreativeColab.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public string Title { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public string Platform { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? Developer { get; set; }

        // Navigation
        public ICollection<Bookmark> Bookmarks { get; set; }
        public ICollection<GamePrice> GamePrices { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }

}
