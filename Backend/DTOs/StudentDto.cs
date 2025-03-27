using System;
using System.ComponentModel.DataAnnotations;

public class StudentDto
{
    [Required(ErrorMessage = "MSSV không được để trống")]
    public string MSSV { get; set; }

    [Required(ErrorMessage = "Họ và tên không được để trống")]
    [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
    public string HoTen { get; set; }

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    public DateTime NgaySinh { get; set; }
    public string GioiTinh { get; set; }
    [Required(ErrorMessage = "Khoa không được để trống")]
    public string Department { get; set; }
    [Required(ErrorMessage = "Khóa học không được để trống")]
    public string SchoolYear { get; set; }
    [Required(ErrorMessage = "Chương trình học không được để trống")]
    public string StudyProgram { get; set; }
    [Required(ErrorMessage = "Tình trạng không được để trống")]
    public string Status { get; set; }

    // Address fields from CSV
    [Required(ErrorMessage = "Địa chỉ nhận thư không được để trống")]
    public string AddressNhanThu_HouseNumber { get; set; }
    [Required(ErrorMessage = "Địa chỉ nhận thư không được để trống")]
    public string AddressNhanThu_StreetName { get; set; }
    [Required(ErrorMessage = "Địa chỉ nhận thư không được để trống")]
    public string AddressNhanThu_Ward { get; set; }
    [Required(ErrorMessage = "Địa chỉ nhận thư không được để trống")]
    public string AddressNhanThu_District { get; set; }
    [Required(ErrorMessage = "Địa chỉ nhận thư không được để trống")]
    public string AddressNhanThu_Province { get; set; }
    [Required(ErrorMessage = "Địa chỉ nhận thư không được để trống")]
    public string AddressNhanThu_Country { get; set; }

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
    public string Identification_Type { get; set; }
    public string Identification_Number { get; set; }
    public DateTime Identification_IssueDate { get; set; }
    public DateTime? Identification_ExpiryDate { get; set; }
    public string Identification_IssuedBy { get; set; }
    public bool Identification_HasChip { get; set; }
    public string Identification_IssuingCountry { get; set; }
    public string Identification_Notes { get; set; }

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; }

    public string QuocTich { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone]
    [PhoneNumber("VN")]
    public string SoDienThoai { get; set; }
}
