using Microsoft.AspNetCore.Mvc;
using WebRazorpage.Models;
using WebRazorpage.Services;

namespace WebRazorpage.Pages.Components.ProductBox
{
    public class ProductBox : ViewComponent
    {
        private readonly ProductService _productService;

        // Inject ProductService
        public ProductBox(ProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke(bool sapxeptang = true)
        {
            var products = _productService.GetProducts();

            // Logic sắp xếp
            if (sapxeptang)
                products = products.OrderBy(p => p.Price).ToList();
            else
                products = products.OrderByDescending(p => p.Price).ToList();

            return View(products);
        }
    }
}