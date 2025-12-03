using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebRazorpage.Models;

namespace WebRazorpage.Pages
{
    public class ProductPageModel : PageModel
    {
        private readonly QLBHContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductPageModel(QLBHContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // Danh sách
        public List<Product> Products { get; set; } = new();

        // Chi tiết
        public Product? Product { get; set; }

        // Form tạo mới
        [BindProperty]
        public Product NewProduct { get; set; } = new();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        // Tìm kiếm
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        // Lọc category
        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        // GET
        public async Task OnGetAsync(int? id)
        {
            if (id != null)
            {
                Product = await _context.Products
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == id);

                return;
            }

            var query = _context.Products.Include(x => x.Category).AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
                query = query.Where(x => x.Name.Contains(SearchString));

            if (CategoryId.HasValue)
                query = query.Where(x => x.CategoryId == CategoryId.Value);

            Products = await query.ToListAsync();
        }

        // POST (Create)
        public async Task<IActionResult> OnPostAsync()
        {
            // Bỏ Category khỏi validation (tránh lỗi null navigation property)
            ModelState.Remove("NewProduct.Category");

            if (!ModelState.IsValid)
            {
                await LoadProductsAgain();
                return Page();
            }

            // Upload ảnh
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await ImageFile.CopyToAsync(stream);

                NewProduct.Image = fileName;
            }
            else
            {
                NewProduct.Image = "no-image.jpg";
            }

            // Lưu DB
            _context.Products.Add(NewProduct);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ProductPage");
        }

        private async Task LoadProductsAgain()
        {
            Products = await _context.Products
                .Include(x => x.Category)
                .ToListAsync();
        }
    }
}
