using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
public class Grade
{
    [Key] // Đánh dấu trường này là khóa chính
    public int GradeId { get; set; }

    [Required]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    public string ClassId { get; set; } = string.Empty;

    [ForeignKey(nameof(StudentId))]
    public Student Student { get; set; } = null!;

    [ForeignKey(nameof(ClassId))]
    public Class Class { get; set; } = null!;

    [Required]
    [Range(0, 10)]
    public double Score { get; set; }

    [MaxLength(5)]
    public string? GradeLetter { get; set; } // "A", "B+"

    [Range(0.0, 4.0)]
    public double GPA { get; set; }

    [NotMapped]
    public string CourseName => Class.Course.Name;

    [NotMapped]
    public int Credit => Class.Course.Credits;
}

}
