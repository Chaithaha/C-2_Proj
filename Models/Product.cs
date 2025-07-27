namespace CreativeColab.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public DateTime AddedAt { get; set; }

        // Navigation
        public Category Category { get; set; }
        public ICollection<ProductPrice> ProductPrices { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
    }

}
