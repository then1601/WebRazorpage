using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebRazorpage.Models;

namespace WebRazorpage.Pages
{
    public class ProductPageModel : PageModel
    {
        private readonly QLBHContext _context; // Dùng DbContext thay vì ProductService

        public ProductPageModel(QLBHContext context)
        {
            _context = context;
        }

        public List<Product> Products { get; set; } = new List<Product>();
        public Product Product { get; set; }

        // Biến phục vụ tìm kiếm và lọc
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; } // Từ khóa tìm kiếm

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; } // ID danh mục cần lọc

        public async Task OnGetAsync(int? id)
        {
            if (id != null)
            {
                // Xem chi tiết
                Product = await _context.Products
                                        .Include(p => p.Category) // Kèm thông tin danh mục
                                        .FirstOrDefaultAsync(m => m.Id == id);
                ViewData["Title"] = $"Chi tiết: {Product?.Name}";
            }
            else
            {
                // Xem danh sách (Có tìm kiếm và lọc)
                ViewData["Title"] = "Danh sách sản phẩm";

                // Truy vấn cơ bản
                var query = _context.Products.Include(p => p.Category).AsQueryable();

                // 1. Lọc theo tên (Tìm kiếm)
                if (!string.IsNullOrEmpty(SearchString))
                {
                    query = query.Where(p => p.Name.Contains(SearchString));
                }

                // 2. Lọc theo danh mục
                if (CategoryId.HasValue)
                {
                    query = query.Where(p => p.CategoryId == CategoryId);
                }

                Products = await query.ToListAsync();
            }
        }

        // --- Code phần Thêm Mới Sản Phẩm (POST) giữ nguyên logic upload ảnh ---
        // Chỉ cần thay _productService.Add() bằng _context.Add() và _context.SaveChanges()
        [BindProperty]
        public Product NewProduct { get; set; }
        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (NewProduct.Name != null)
            {
                // (Giữ nguyên phần code upload ảnh ở bài 6 của bạn tại đây)
                // ...
                // Sau khi xử lý ảnh xong:

                _context.Products.Add(NewProduct);
                await _context.SaveChangesAsync(); // Lưu vào DB
            }
            return RedirectToPage("./ProductPage");
        }
    }
}