using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebRazorpage.Models;

namespace WebRazorpage.Pages
{
    public class ContactPageModel : PageModel
    {
        [BindProperty]
        public Contact Contact { get; set; } = new Contact();

        public string ThongBao { get; set; }

        public void OnGet()
        {
            ThongBao = "Mời nhập thông tin";
        }

        public void OnPost()
        {
            // Kiểm tra dữ liệu có hợp lệ theo các rules trong Model không
            if (ModelState.IsValid)
            {
                ThongBao = $"Dữ liệu hợp lệ! Xin chào {Contact.LastName} {Contact.FirstName}.";
            }
            else
            {
                ThongBao = "Dữ liệu không hợp lệ, vui lòng kiểm tra lại!";
            }
        }
    }
}