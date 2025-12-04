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

        // Danh sách sản phẩm
        public List<Product> Products { get; set; } = new();

        // Chi tiết 1 sản phẩm (khi xem)
        public Product? Product { get; set; }

        // Form thêm mới
        [BindProperty]
        public Product NewProduct { get; set; } = new();

        // Ảnh upload
        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        // Lọc / tìm kiếm
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        // Danh sách category cho dropdown
        public List<Category> Categories { get; set; } = new();


        // GET
        public async Task OnGetAsync(int? id)
        {
            // Load dropdown Category
            Categories = await _context.Categories.ToListAsync();

            // Nếu có id → xem chi tiết
            if (id.HasValue)
            {
                Product = await _context.Products
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == id);
            }

            // Luôn load danh sách
            await LoadProductList();
        }


        // POST
        public async Task<IActionResult> OnPostAsync()
        {
            // Bỏ validate navigation property
            ModelState.Remove("NewProduct.Category");

            if (!ModelState.IsValid)
            {
                Categories = await _context.Categories.ToListAsync();
                await LoadProductList();
                return Page();
            }

            // Xử lý upload ảnh
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "images");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await ImageFile.CopyToAsync(stream);

                NewProduct.Image = "/images/" + fileName;
            }
            else
            {
                NewProduct.Image = "/images/no-image.jpg";
            }

            _context.Products.Add(NewProduct);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ProductPage");
        }


        private async Task LoadProductList()
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
                query = query.Where(x => x.Name.Contains(SearchString));

            if (CategoryId.HasValue)
                query = query.Where(x => x.CategoryId == CategoryId.Value);

            query = query.OrderByDescending(x => x.Id);

            Products = await query.ToListAsync();
        }
    }
}
