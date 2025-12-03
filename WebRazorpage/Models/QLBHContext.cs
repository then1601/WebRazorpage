using Microsoft.EntityFrameworkCore;

namespace WebRazorpage.Models
{
    public class QLBHContext : DbContext
    {
        public QLBHContext(DbContextOptions<QLBHContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data (tạo dữ liệu mẫu) cho Category
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Điện thoại", Description = "Các loại smartphone" },
                new Category { Id = 2, Name = "Laptop", Description = "Máy tính xách tay" }
            );

            // Seed data cho Product
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Iphone 14", Price = 1000, CategoryId = 1, Stock = 10, Description = "Apple Phone" },
                new Product { Id = 2, Name = "Samsung S23", Price = 900, CategoryId = 1, Stock = 15, Description = "Samsung Phone" }
            );
        }
    }
}