using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebRazorpage.Models;
using WebRazorpage.Services;

namespace WebRazorpage.Pages
{
    public class ProductPageModel : PageModel
    {
        private readonly ProductService _productService;

        public ProductPageModel(ProductService productService)
        {
            _productService = productService;
        }

        public List<Product> Products { get; set; } = new List<Product>();
        public Product Product { get; set; }

        // Handler mặc định (chạy khi vào trang)
        // URL: /ProductPage hoặc /ProductPage?id=1
        public void OnGet(int? id)
        {
            if (id != null)
            {
                // Nếu có ID -> Xem chi tiết
                ViewData["Title"] = $"Thông tin sản phẩm (ID={id})";
                Product = _productService.GetProductById(id.Value);
            }
            else
            {
                // Không có ID -> Xem danh sách
                ViewData["Title"] = "Danh sách sản phẩm";
                Products = _productService.GetProducts();
            }
        }

        // Handler xem sản phẩm cuối cùng
        // URL: /ProductPage?handler=LastProduct
        public IActionResult OnGetLastProduct()
        {
            ViewData["Title"] = "Sản phẩm cuối";
            Product = _productService.GetProducts().LastOrDefault();
            if (Product != null)
            {
                return Page(); // Render lại trang hiện tại với dữ liệu mới
            }
            return NotFound();
        }

        // Handler xóa tất cả
        // URL: /ProductPage?handler=RemoveAll
        public IActionResult OnGetRemoveAll()
        {
            _productService.ClearProducts();
            return RedirectToPage("ProductPage"); // Load lại trang để thấy list trống
        }

        // Handler nạp lại dữ liệu
        // URL: /ProductPage?handler=LoadAll
        public IActionResult OnGetLoadAll()
        {
            _productService.LoadProducts();
            return RedirectToPage("ProductPage");
        }
    }
}