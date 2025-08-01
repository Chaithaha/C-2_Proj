﻿namespace CreativeColab.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;

        // Navigation
        public ICollection<Product> Products { get; set; }
    }

}
