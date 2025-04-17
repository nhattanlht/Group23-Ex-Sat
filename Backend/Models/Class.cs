using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Class
    {
        [Key]
        [Required]
        [MaxLength(20)]
        public string ClassId { get; set; } = string.Empty; // Mã lớp học

        [Required]
        public string CourseCode { get; set; } = string.Empty;

        [ForeignKey(nameof(CourseCode))]
        public Course Course { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        public string AcademicYear { get; set; } = string.Empty; // VD: 2024-2025

        [Required]
        [Range(1, 3)]
        public int Semester { get; set; } // 1 | 2 | 3

        [Required]
        [MaxLength(100)]
        public string Teacher { get; set; } = string.Empty;

        [Required]
        [Range(1, 200)]
        public int MaxStudents { get; set; }

        [MaxLength(100)]
        public string? Schedule { get; set; } // Lịch học

        [MaxLength(50)]
        public string? Classroom { get; set; } // Phòng học

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
