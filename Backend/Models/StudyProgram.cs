using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class StudyProgram
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng giá trị
        public int Id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Tên chương trình không được vượt quá 200 ký tự.")]
        public string Name { get; set; } = string.Empty; // Default value to avoid nullability warning

        // Khởi tạo danh sách để tránh lỗi NullReferenceException
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}