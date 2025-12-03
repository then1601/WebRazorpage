using System.ComponentModel.DataAnnotations;

namespace WebRazorpage.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        // Quan hệ 1-nhiều: Một danh mục có nhiều sản phẩm
        public ICollection<Product> Products { get; set; }
    }
}