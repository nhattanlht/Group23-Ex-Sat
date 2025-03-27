using CsvHelper.Configuration;
using StudentManagement.Models;

public sealed class StudentMap : ClassMap<StudentDto>
{
    public StudentMap()
    {
        Map(m => m.MSSV).Name("MSSV");
        Map(m => m.HoTen).Name("HoTen");
        Map(m => m.NgaySinh).Name("NgaySinh").TypeConverterOption.Format("M/d/yyyy H:mm");
        Map(m => m.GioiTinh).Name("GioiTinh");

        Map(m => m.Department).Name("Department");
        Map(m => m.SchoolYear).Name("SchoolYear");
        Map(m => m.StudyProgram).Name("StudyProgram");
        Map(m => m.Status).Name("Status");

        // Mapping Address
        Map(m => m.AddressNhanThu_HouseNumber).Name("AddressNhanThu_HouseNumber");
        Map(m => m.AddressNhanThu_StreetName).Name("AddressNhanThu_StreetName");
        Map(m => m.AddressNhanThu_Ward).Name("AddressNhanThu_Ward");
        Map(m => m.AddressNhanThu_District).Name("AddressNhanThu_District");
        Map(m => m.AddressNhanThu_Province).Name("AddressNhanThu_Province");
        Map(m => m.AddressNhanThu_Country).Name("AddressNhanThu_Country");

        Map(m => m.AddressThuongTru_HouseNumber).Name("AddressThuongTru_HouseNumber").TypeConverterOption.NullValues("");
        Map(m => m.AddressThuongTru_StreetName).Name("AddressThuongTru_StreetName").TypeConverterOption.NullValues("");
        Map(m => m.AddressThuongTru_Ward).Name("AddressThuongTru_Ward").TypeConverterOption.NullValues("");
        Map(m => m.AddressThuongTru_District).Name("AddressThuongTru_District").TypeConverterOption.NullValues("");
        Map(m => m.AddressThuongTru_Province).Name("AddressThuongTru_Province").TypeConverterOption.NullValues("");
        Map(m => m.AddressThuongTru_Country).Name("AddressThuongTru_Country").TypeConverterOption.NullValues("");

        Map(m => m.AddressTamTru_HouseNumber).Name("AddressTamTru_HouseNumber").TypeConverterOption.NullValues("");
        Map(m => m.AddressTamTru_StreetName).Name("AddressTamTru_StreetName").TypeConverterOption.NullValues("");
        Map(m => m.AddressTamTru_Ward).Name("AddressTamTru_Ward").TypeConverterOption.NullValues("");
        Map(m => m.AddressTamTru_District).Name("AddressTamTru_District").TypeConverterOption.NullValues("");
        Map(m => m.AddressTamTru_Province).Name("AddressTamTru_Province").TypeConverterOption.NullValues("");
        Map(m => m.AddressTamTru_Country).Name("AddressTamTru_Country").TypeConverterOption.NullValues("");

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
