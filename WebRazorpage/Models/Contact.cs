using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebRazorpage.Models
{
    public class Contact
    {
        [BindProperty]
        [Display(Name = "Id của bạn")]
        [Range(1, 100, ErrorMessage = "Id phải từ 1 đến 100")]
        public int ContactId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Bạn chưa nhập tên")]
        [Display(Name = "Tên")]
        public string FirstName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Bạn chưa nhập họ")]
        [Display(Name = "Họ")]
        public string LastName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Bạn chưa nhập Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
    }
}