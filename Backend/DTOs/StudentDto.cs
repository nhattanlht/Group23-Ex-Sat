using System;
using System.ComponentModel.DataAnnotations;

public class StudentDto
{
    [Required(ErrorMessage = "MSSV không được để trống")]
    public string MSSV { get; set; } = string.Empty;

    [Required(ErrorMessage = "Họ và tên không được để trống")]
    [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
    public string HoTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    public DateTime NgaySinh { get; set; }
    public string GioiTinh { get; set; } = string.Empty;
    [Required(ErrorMessage = "Khoa không được để trống")]
    public string Department { get; set; } = string.Empty;
    [Required(ErrorMessage = "Khóa học không được để trống")]
    public string SchoolYear { get; set; } = string.Empty;
    [Required(ErrorMessage = "Chương trình học không được để trống")]
    public string StudyProgram { get; set; } = string.Empty;
    [Required(ErrorMessage = "Tình trạng không được để trống")]
    public string Status { get; set; } = string.Empty;

    // Address fields from CSV
    [Required(ErrorMessage = "Địa chỉ nhận thư không được để trống")]
    public string AddressNhanThu_HouseNumber { get; set; } = string.Empty;
    [Required(ErrorMessage = "Địa chỉ nhận thư không được để trống")]
    public string AddressNhanThu_StreetName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Địa chỉ nhận thư không được để trống")]
    public string AddressNhanThu_Ward { get; set; } = string.Empty;
    [Required(ErrorMessage = "Địa chỉ nhận thư không được để trống")]
    public string AddressNhanThu_District { get; set; } = string.Empty;
    [Required(ErrorMessage = "Địa chỉ nhận thư không được để trống")]
    public string AddressNhanThu_Province { get; set; } = string.Empty;
    [Required(ErrorMessage = "Địa chỉ nhận thư không được để trống")]
    public string AddressNhanThu_Country { get; set; } = string.Empty;

    public string? AddressThuongTru_HouseNumber { get; set; }
    public string? AddressThuongTru_StreetName { get; set; }
    public string? AddressThuongTru_Ward { get; set; }
    public string? AddressThuongTru_District { get; set; }
    public string? AddressThuongTru_Province { get; set; }
    public string? AddressThuongTru_Country { get; set; }

    public string? AddressTamTru_HouseNumber { get; set; }
    public string? AddressTamTru_StreetName { get; set; }
    public string? AddressTamTru_Ward { get; set; }
    public string? AddressTamTru_District { get; set; }
    public string? AddressTamTru_Province { get; set; }
    public string? AddressTamTru_Country { get; set; }

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

    public string QuocTich { get; set; } = string.Empty;

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone]
    [PhoneNumber("VN")]
    public string SoDienThoai { get; set; } = string.Empty;
}
