using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Student
    {
        [Key]
        public string StudentId { get; set; } = string.Empty; // Default value to avoid nullability warning

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty; // Default value to avoid nullability warning

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty; // Default value to avoid nullability warning

        [Required]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }

        [Required]
        public int SchoolYearId { get; set; }

        [ForeignKey("SchoolYearId")]
        public virtual SchoolYear? SchoolYear { get; set; }

        [Required]
        public int StudyProgramId { get; set; }

        [ForeignKey("StudyProgramId")]
        public virtual StudyProgram? StudyProgram { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string Nationality { get; set; } = string.Empty; // Default value to avoid nullability warning

        [Phone]
        [PhoneNumber("VN")]
        public string? PhoneNumber { get; set; }

        [Required]
        public int PermanentAddressId { get; set; }

        public int? RegisteredAddressId { get; set; }
        
        public int? TemporaryAddressId { get; set; }

        [ForeignKey("PermanentAddressId")]
        public virtual Address? PermanentAddress { get; set; }

        [ForeignKey("RegisteredAddressId")]
        public virtual Address? RegisteredAddress { get; set; }

        [ForeignKey("TemporaryAddressId")]
        public virtual Address? TemporaryAddress { get; set; }

        [Required]
        public int IdentificationId { get; set; }

        [ForeignKey("IdentificationId")]
        public virtual Identification? Identification { get; set; }

        [Required]
        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public virtual StudentStatus? StudentStatus { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
