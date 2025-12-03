using WebRazorpage.Models;

namespace WebRazorpage.Services
{
    public class ProductService
    {
        private List<Product> products = new List<Product>()
        {
            new Product() { Id = 1, Name = "Iphone 14 Pro", Description = "Dien thoai cua Apple", Price = 1000 },
            new Product() { Id = 2, Name = "Samsung Galaxy", Description = "Dien thoai cua Samsung", Price = 500 },
            new Product() { Id = 3, Name = "Sony Xperia", Description = "Dien thoai cua Sony", Price = 800 }
        };

        public List<Product> GetProducts()
        {
            return products;
        }
    }
}