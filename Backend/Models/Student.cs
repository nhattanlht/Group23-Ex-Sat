using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Student
    {
        [Key]
        public string MSSV { get; set; }

        [Required, StringLength(100)]
        public string HoTen { get; set; }

        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        [Required]
        public string GioiTinh { get; set; }

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

        public string? DiaChi { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? SoDienThoai { get; set; }

        [Required]
        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public virtual StudentStatus? StudentStatus { get; set; } // ✅ Cho phép null
    }
}
