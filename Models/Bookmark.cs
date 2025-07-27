namespace CreativeColab.Models
{
    public class Bookmark
    {
        public int BookmarkId { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public decimal? NotifyBelowPrice { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public User User { get; set; }
        public Game Game { get; set; }
    }

}
