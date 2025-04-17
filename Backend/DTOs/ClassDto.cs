using System.ComponentModel.DataAnnotations;

public class ClassCreateDto
{
    [Required]
    [MaxLength(20)]
    public string ClassId { get; set; } = string.Empty; // Mã lớp học

    [Required]
    [MaxLength(10)]
    public string CourseCode { get; set; } = string.Empty; // Mã khóa học

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
}
