using Microsoft.EntityFrameworkCore;
using CreativeColab.Models;

namespace CreativeColab.Data
{
    // The application's main database context. This manages all entity sets and database access.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets for all entities in the project
        public DbSet<User> Users { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<ProjectDeadline> ProjectDeadlines { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GamePrice> GamePrices { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DesignerStatus> DesignerStatuses { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
    }
}