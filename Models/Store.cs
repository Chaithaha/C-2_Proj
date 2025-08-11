using System.ComponentModel.DataAnnotations;

namespace CreativeColab.Models
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }

        [Required(ErrorMessage = "Store name is required")]
        public string Name { get; set; }
        public string? Website { get; set; }

        // Navigation
        public ICollection<GamePrice> GamePrices { get; set; } = new List<GamePrice>();
        public ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
    }

}
