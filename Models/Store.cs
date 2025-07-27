namespace CreativeColab.Models
{
    public class Store
    {
        public int StoreId { get; set; }
        public string Name { get; set; } = null!;
        public string? Website { get; set; }

        // Navigation
        public ICollection<GamePrice> GamePrices { get; set; }
        public ICollection<ProductPrice> ProductPrices { get; set; }
    }

}
