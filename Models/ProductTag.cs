namespace CreativeColab.Models
{
    public class ProductTag
    {
        public int ProductTagId { get; set; }
        public int ProductId { get; set; }
        public string Tag { get; set; } = null!;
        public bool IsActive { get; set; }

        // Navigation
        public Product Product { get; set; }
    }

}
