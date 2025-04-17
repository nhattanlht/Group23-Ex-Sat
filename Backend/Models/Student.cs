using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Student
    {
        [Key]
        public string MSSV { get; set; } = string.Empty; // Default value to avoid nullability warning

        [Required, StringLength(100)]
        public string HoTen { get; set; } = string.Empty; // Default value to avoid nullability warning

        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        [Required]
        public string GioiTinh { get; set; } = string.Empty; // Default value to avoid nullability warning

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

        [Required]
        public string QuocTich { get; set; } = string.Empty; // Default value to avoid nullability warning

        [Phone]
        [PhoneNumber("VN")]
        public string? SoDienThoai { get; set; }

        [Required]
        public int DiaChiNhanThuId { get; set; }

        public int? DiaChiThuongTruId { get; set; }
        
        public int? DiaChiTamTruId { get; set; }

        [ForeignKey("DiaChiNhanThuId")]
        public virtual Address? DiaChiNhanThu { get; set; }

        [ForeignKey("DiaChiThuongTruId")]
        public virtual Address? DiaChiThuongTru { get; set; }

        [ForeignKey("DiaChiTamTruId")]
        public virtual Address? DiaChiTamTru { get; set; }

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
