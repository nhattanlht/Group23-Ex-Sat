using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Course
    {
        [Key]
        [Required]
        [MaxLength(20)]
        public string CourseCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Range(2, 10, ErrorMessage = "CreditCount_Range")]
        public int Credits { get; set; }

        // Môn tiên quyết - nullable FK đến chính nó
        [MaxLength(20)]
        public string? PrerequisiteCourseCode { get; set; }

        [ForeignKey("PrerequisiteCourseCode")]
        public virtual Course? PrerequisiteCourse { get; set; }

        // Tham chiếu đến khoa
        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Khóa học có thể được mở thành nhiều lớp học
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}
