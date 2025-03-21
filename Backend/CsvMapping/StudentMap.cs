using CsvHelper.Configuration;
using StudentManagement.Models;

public sealed class StudentMap : ClassMap<StudentCsvDto>
{
    public StudentMap()
    {
        Map(m => m.MSSV).Name("MSSV");
        Map(m => m.HoTen).Name("HoTen");
        Map(m => m.NgaySinh).Name("NgaySinh").TypeConverterOption.Format("M/d/yyyy H:mm");
        Map(m => m.GioiTinh).Name("GioiTinh");

        Map(m => m.DepartmentName).Name("Department");
        Map(m => m.SchoolYearName).Name("SchoolYear");
        Map(m => m.StudyProgramName).Name("StudyProgram");
        Map(m => m.StatusName).Name("Status");

        // Mapping Address
        Map(m => m.DiaChiNhanThu_HouseNumber).Name("AddressNhanThu_HouseNumber");
        Map(m => m.DiaChiNhanThu_StreetName).Name("AddressNhanThu_StreetName");
        Map(m => m.DiaChiNhanThu_Ward).Name("AddressNhanThu_Ward");
        Map(m => m.DiaChiNhanThu_District).Name("AddressNhanThu_District");
        Map(m => m.DiaChiNhanThu_Province).Name("AddressNhanThu_Province");
        Map(m => m.DiaChiNhanThu_Country).Name("AddressNhanThu_Country");

        Map(m => m.DiaChiThuongTru_HouseNumber).Name("AddressThuongTru_HouseNumber").Optional();
        Map(m => m.DiaChiThuongTru_StreetName).Name("AddressThuongTru_StreetName").Optional();
        Map(m => m.DiaChiThuongTru_Ward).Name("AddressThuongTru_Ward").Optional();
        Map(m => m.DiaChiThuongTru_District).Name("AddressThuongTru_District").Optional();
        Map(m => m.DiaChiThuongTru_Province).Name("AddressThuongTru_Province").Optional();
        Map(m => m.DiaChiThuongTru_Country).Name("AddressThuongTru_Country").Optional();

        Map(m => m.DiaChiTamTru_HouseNumber).Name("AddressTamTru_HouseNumber").Optional();
        Map(m => m.DiaChiTamTru_StreetName).Name("AddressTamTru_StreetName").Optional();
        Map(m => m.DiaChiTamTru_Ward).Name("AddressTamTru_Ward").Optional();
        Map(m => m.DiaChiTamTru_District).Name("AddressTamTru_District").Optional();
        Map(m => m.DiaChiTamTru_Province).Name("AddressTamTru_Province").Optional();
        Map(m => m.DiaChiTamTru_Country).Name("AddressTamTru_Country").Optional();

        // Mapping Identification
        Map(m => m.Identification_Type).Name("Identification_Type");
        Map(m => m.Identification_Number).Name("Identification_Number");
        Map(m => m.Identification_IssueDate).Name("Identification_IssueDate").TypeConverterOption.Format("M/d/yyyy H:mm");
        Map(m => m.Identification_ExpiryDate).Name("Identification_ExpiryDate").TypeConverterOption.Format("M/d/yyyy H:mm").TypeConverterOption.NullValues("");;
        Map(m => m.Identification_IssuedBy).Name("Identification_IssuedBy");
        Map(m => m.Identification_HasChip).Name("Identification_HasChip").Optional();
        Map(m => m.Identification_IssuingCountry).Name("Identification_IssuingCountry").Optional();
        Map(m => m.Identification_Notes).Name("Identification_Notes").Optional();

        Map(m => m.Email).Name("Email");
        Map(m => m.QuocTich).Name("QuocTich");
        Map(m => m.SoDienThoai).Name("SoDienThoai");
    }
}
