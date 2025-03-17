using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class SchoolYear
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng giá trị
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Khởi tạo danh sách để tránh lỗi NullReferenceException
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}