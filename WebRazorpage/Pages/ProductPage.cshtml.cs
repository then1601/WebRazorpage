// Thêm các thư viện này
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using WebRazorpage.Services;

namespace WebRazorpage.Pages
{
    public class ProductPageModel : PageModel
    {
        private readonly ProductService _productService;
        private readonly IWebHostEnvironment _environment; // Để lấy đường dẫn thư mục wwwroot

        public ProductPageModel(ProductService productService, IWebHostEnvironment environment)
        {
            _productService = productService;
            _environment = environment;
        }

        public List<Product> Products { get; set; } = new List<Product>();
        public Product Product { get; set; }

        // --- Model Binding cho Form Thêm mới ---
        [BindProperty]
        public Product NewProduct { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; } // Binding file upload

        public void OnGet(int? id)
        {
            // (Giữ nguyên code OnGet cũ của bạn ở đây)
            if (id != null)
            {
                ViewData["Title"] = $"Thông tin sản phẩm (ID={id})";
                Product = _productService.GetProductById(id.Value);
            }
            else
            {
                ViewData["Title"] = "Danh sách sản phẩm";
                Products = _productService.GetProducts();
            }
        }

        // --- Các Handler cũ giữ nguyên (LastProduct, RemoveAll...) ---
        // (Bạn tự copy lại các handler cũ vào đây nhé)
        public IActionResult OnGetRemoveAll() { _productService.ClearProducts(); return RedirectToPage("ProductPage"); }
        public IActionResult OnGetLoadAll() { _productService.LoadProducts(); return RedirectToPage("ProductPage"); }
        public IActionResult OnGetLastProduct() { Product = _productService.GetProducts().LastOrDefault(); return Page(); }

        // --- Handler Mới: Xử lý Thêm sản phẩm (POST) ---
        public async Task<IActionResult> OnPost()
        {
            if (NewProduct.Name != null) // Kiểm tra sơ bộ
            {
                // Xử lý Upload ảnh
                if (ImageFile != null)
                {
                    // Tạo tên file độc nhất để tránh trùng
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var uploadPath = Path.Combine(_environment.WebRootPath, "images");

                    // Tạo thư mục nếu chưa có
                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    var filePath = Path.Combine(uploadPath, fileName);

                    // Lưu file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    // Lưu đường dẫn ảnh vào Product
                    NewProduct.Image = "/images/" + fileName;
                }

                // Lưu sản phẩm vào Service
                _productService.Add(NewProduct);
            }
            return RedirectToPage("ProductPage");
        }
    }
}