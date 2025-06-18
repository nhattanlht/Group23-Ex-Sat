using System;
using System.ComponentModel.DataAnnotations;

public class StudentDto
{
    [Required(ErrorMessage = "StudentId không được để trống")]
    public string StudentId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Họ và tên không được để trống")]
    [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    [Required(ErrorMessage = "Khoa không được để trống")]
    public string Department { get; set; } = string.Empty;
    [Required(ErrorMessage = "Khóa học không được để trống")]
    public string SchoolYear { get; set; } = string.Empty;
    [Required(ErrorMessage = "Chương trình học không được để trống")]
    public string StudyProgram { get; set; } = string.Empty;
    [Required(ErrorMessage = "Tình trạng không được để trống")]
    public string Status { get; set; } = string.Empty;

    // Address fields from CSV
    [Required(ErrorMessage = "Số nhà Địa chỉ nhận thư không được để trống")]
    public string PermanentAddress_HouseNumber { get; set; } = string.Empty;
    [Required(ErrorMessage = "Tên đường Địa chỉ nhận thư không được để trống")]
    public string PermanentAddress_StreetName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Phường Địa chỉ nhận thư không được để trống")]
    public string PermanentAddress_Ward { get; set; } = string.Empty;
    [Required(ErrorMessage = "Quận Địa chỉ nhận thư không được để trống")]
    public string PermanentAddress_District { get; set; } = string.Empty;
    [Required(ErrorMessage = "Tỉnh của Địa chỉ nhận thư không được để trống")]
    public string PermanentAddress_Province { get; set; } = string.Empty;
    [Required(ErrorMessage = "Quốc gia của Địa chỉ nhận thư không được để trống")]
    public string PermanentAddress_Country { get; set; } = string.Empty;

    public string? RegisteredAddress_HouseNumber { get; set; }
    public string? RegisteredAddress_StreetName { get; set; }
    public string? RegisteredAddress_Ward { get; set; }
    public string? RegisteredAddress_District { get; set; }
    public string? RegisteredAddress_Province { get; set; }
    public string? RegisteredAddress_Country { get; set; }

    public string? TemporaryAddress_HouseNumber { get; set; }
    public string? TemporaryAddress_StreetName { get; set; }
    public string? TemporaryAddress_Ward { get; set; }
    public string? TemporaryAddress_District { get; set; }
    public string? TemporaryAddress_Province { get; set; }
    public string? TemporaryAddress_Country { get; set; }

    // Identification fields from CSV
    public string Identification_Type { get; set; } = string.Empty;
    public string Identification_Number { get; set; } = string.Empty;
    public DateTime Identification_IssueDate { get; set; }
    public DateTime? Identification_ExpiryDate { get; set; }
    public string Identification_IssuedBy { get; set; } = string.Empty;
    public bool Identification_HasChip { get; set; }
    public string Identification_IssuingCountry { get; set; } = string.Empty;
    public string Identification_Notes { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = string.Empty;

    public string Nationality { get; set; } = string.Empty;

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone]
    [PhoneNumber("VN")]
    public string PhoneNumber { get; set; } = string.Empty;
}
