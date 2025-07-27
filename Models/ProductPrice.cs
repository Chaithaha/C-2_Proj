namespace CreativeColab.Models
{
    public class ProductPrice
    {
        public int ProductPriceId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int StoreId { get; set; }
        public DateTime DateTracked { get; set; }

        // Navigation
        public Product Product { get; set; }
        public Store Store { get; set; }
    }

}
