using CsvHelper.Configuration;
using StudentManagement.Models;

public sealed class StudentMap : ClassMap<StudentDto>
{
    public StudentMap()
    {
        Map(m => m.StudentId).Name("StudentId");
        Map(m => m.FullName).Name("FullName");
        Map(m => m.DateOfBirth).Name("DateOfBirth").TypeConverterOption.Format(new[] { "M/d/yyyy H:mm", "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss" });
        Map(m => m.Gender).Name("Gender");

        Map(m => m.Department).Name("Department");
        Map(m => m.SchoolYear).Name("SchoolYear");
        Map(m => m.StudyProgram).Name("StudyProgram");
        Map(m => m.Status).Name("Status");

        // Mapping Address
        Map(m => m.PermanentAddress_HouseNumber).Name("PermanentAddress_HouseNumber");
        Map(m => m.PermanentAddress_StreetName).Name("PermanentAddress_StreetName");
        Map(m => m.PermanentAddress_Ward).Name("PermanentAddress_Ward");
        Map(m => m.PermanentAddress_District).Name("PermanentAddress_District");
        Map(m => m.PermanentAddress_Province).Name("PermanentAddress_Province");
        Map(m => m.PermanentAddress_Country).Name("PermanentAddress_Country");

        Map(m => m.RegisteredAddress_HouseNumber).Name("RegisteredAddress_HouseNumber").TypeConverterOption.NullValues("");
        Map(m => m.RegisteredAddress_StreetName).Name("RegisteredAddress_StreetName").TypeConverterOption.NullValues("");
        Map(m => m.RegisteredAddress_Ward).Name("RegisteredAddress_Ward").TypeConverterOption.NullValues("");
        Map(m => m.RegisteredAddress_District).Name("RegisteredAddress_District").TypeConverterOption.NullValues("");
        Map(m => m.RegisteredAddress_Province).Name("RegisteredAddress_Province").TypeConverterOption.NullValues("");
        Map(m => m.RegisteredAddress_Country).Name("RegisteredAddress_Country").TypeConverterOption.NullValues("");

        Map(m => m.TemporaryAddress_HouseNumber).Name("TemporaryAddress_HouseNumber").TypeConverterOption.NullValues("");
        Map(m => m.TemporaryAddress_StreetName).Name("TemporaryAddress_StreetName").TypeConverterOption.NullValues("");
        Map(m => m.TemporaryAddress_Ward).Name("TemporaryAddress_Ward").TypeConverterOption.NullValues("");
        Map(m => m.TemporaryAddress_District).Name("TemporaryAddress_District").TypeConverterOption.NullValues("");
        Map(m => m.TemporaryAddress_Province).Name("TemporaryAddress_Province").TypeConverterOption.NullValues("");
        Map(m => m.TemporaryAddress_Country).Name("TemporaryAddress_Country").TypeConverterOption.NullValues("");

        // Mapping Identification
        Map(m => m.Identification_Type).Name("Identification_Type");
        Map(m => m.Identification_Number).Name("Identification_Number");
        Map(m => m.Identification_IssueDate).Name("Identification_IssueDate").TypeConverterOption.Format(new[] { "M/d/yyyy H:mm", "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss" });
        Map(m => m.Identification_ExpiryDate).Name("Identification_ExpiryDate").TypeConverterOption.Format(new[] { "M/d/yyyy H:mm", "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss" }).TypeConverterOption.NullValues("");;
        Map(m => m.Identification_IssuedBy).Name("Identification_IssuedBy");
        Map(m => m.Identification_HasChip).Name("Identification_HasChip").Optional();
        Map(m => m.Identification_IssuingCountry).Name("Identification_IssuingCountry").Optional();
        Map(m => m.Identification_Notes).Name("Identification_Notes").Optional();

        Map(m => m.Email).Name("Email");
        Map(m => m.Nationality).Name("Nationality");
        Map(m => m.PhoneNumber).Name("PhoneNumber");
    }
}
