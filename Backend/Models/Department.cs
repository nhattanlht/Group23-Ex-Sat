using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng giá trị
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Department_StringLength")]
        public string? Name { get; set; } // Make nullable

        // Khởi tạo danh sách để tránh lỗi NullReferenceException
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();

        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();
    }
}