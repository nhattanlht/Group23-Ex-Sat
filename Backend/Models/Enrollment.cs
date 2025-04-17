using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required]
        public string StudentId { get; set; } = string.Empty;

        [Required]
        public string ClassId { get; set; } = string.Empty;

        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; } = null!;

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; } = null!;

        [Required]
        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        public bool IsCancelled { get; set; } = false;

        public string? CancelReason { get; set; }

        public DateTime? CancelDate { get; set; }
    }
}
