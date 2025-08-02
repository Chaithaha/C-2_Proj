namespace CreativeColab.Models.ViewModels
{
    public class GameViewModel
    {
        public List<Game>? Games { get; set; }
        public Game? Game { get; set; }
        public List<Bookmark>? GameBookmarks { get; set; }
        public List<GamePrice>? GamePrices { get; set; }
        public List<Reminder>? GameReminders { get; set; }
        public List<User>? AvailableUsers { get; set; }
    }
}