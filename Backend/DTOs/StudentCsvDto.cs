public class StudentCsvDto
{
    public string MSSV { get; set; }
    public string HoTen { get; set; }
    public DateTime NgaySinh { get; set; }
    public string GioiTinh { get; set; }
    
    public string DepartmentName { get; set; }
    public string SchoolYearName { get; set; }
    public string StudyProgramName { get; set; }
    public string StatusName { get; set; }

    // Address fields from CSV
    public string DiaChiNhanThu_HouseNumber { get; set; }
    public string DiaChiNhanThu_StreetName { get; set; }
    public string DiaChiNhanThu_Ward { get; set; }
    public string DiaChiNhanThu_District { get; set; }
    public string DiaChiNhanThu_Province { get; set; }
    public string DiaChiNhanThu_Country { get; set; }

    public string DiaChiThuongTru_HouseNumber { get; set; }
    public string DiaChiThuongTru_StreetName { get; set; }
    public string DiaChiThuongTru_Ward { get; set; }
    public string DiaChiThuongTru_District { get; set; }
    public string DiaChiThuongTru_Province { get; set; }
    public string DiaChiThuongTru_Country { get; set; }

    public string DiaChiTamTru_HouseNumber { get; set; }
    public string DiaChiTamTru_StreetName { get; set; }
    public string DiaChiTamTru_Ward { get; set; }
    public string DiaChiTamTru_District { get; set; }
    public string DiaChiTamTru_Province { get; set; }
    public string DiaChiTamTru_Country { get; set; }

    // Identification fields from CSV
    public string Identification_Type { get; set; }
    public string Identification_Number { get; set; }
    public DateTime Identification_IssueDate { get; set; }
    public DateTime? Identification_ExpiryDate { get; set; }
    public string Identification_IssuedBy { get; set; }
    public bool Identification_HasChip { get; set; }
    public string Identification_IssuingCountry { get; set; }
    public string Identification_Notes { get; set; }

    public string Email { get; set; }
    public string QuocTich { get; set; }
    public string SoDienThoai { get; set; }
}
