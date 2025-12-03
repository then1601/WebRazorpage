using Microsoft.AspNetCore.Mvc;
using WebRazorpage.Models;
using System.Linq; // Cần thêm cái này để dùng OrderBy

namespace WebRazorpage.Pages.Components.ProductBox
{
    public class ProductBox : ViewComponent
    {
        private readonly QLBHContext _context; // Đổi từ ProductService sang QLBHContext

        // Inject QLBHContext vào Constructor
        public ProductBox(QLBHContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(bool sapxeptang = true)
        {
            // Lấy danh sách từ Database
            // Lưu ý: Dùng ToList() để thực thi câu lệnh SQL lấy dữ liệu về
            var products = _context.Products.ToList();

            // Xử lý sắp xếp (Logic giữ nguyên)
            if (sapxeptang)
            {
                products = products.OrderBy(p => p.Price).ToList();
            }
            else
            {
                products = products.OrderByDescending(p => p.Price).ToList();
            }

            return View(products);
        }
    }
}