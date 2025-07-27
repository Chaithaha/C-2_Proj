namespace CreativeColab.Models
{
    public class GamePrice
    {
        public int PriceId { get; set; }
        public int GameId { get; set; }
        public int StoreId { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTracked { get; set; }

        // Navigation
        public Game Game { get; set; }
        public Store Store { get; set; }
    }

}
