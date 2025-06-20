using System.ComponentModel.DataAnnotations;

public class StudentDto
{
    [Required(ErrorMessage = "StudentId_Required")]
    public string StudentId { get; set; } = string.Empty;

    [Required(ErrorMessage = "FullName_Required")]
    [StringLength(100, ErrorMessage = "FullName_StringLength")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "DateOfBirth_Required")]
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    [Required(ErrorMessage = "Department_Required")]
    public string Department { get; set; } = string.Empty;
    [Required(ErrorMessage = "SchoolYear_Required")]
    public string SchoolYear { get; set; } = string.Empty;
    [Required(ErrorMessage = "StudyProgram_Required")]
    public string StudyProgram { get; set; } = string.Empty;
    [Required(ErrorMessage = "Status_Required")]
    public string Status { get; set; } = string.Empty;

    // Address fields from CSV
    [Required(ErrorMessage = "PermanentAddress_HouseNumber_Required")]
    public string PermanentAddress_HouseNumber { get; set; } = string.Empty;
    [Required(ErrorMessage = "PermanentAddress_StreetName_Required")]
    public string PermanentAddress_StreetName { get; set; } = string.Empty;
    [Required(ErrorMessage = "PermanentAddress_Ward_Required")]
    public string PermanentAddress_Ward { get; set; } = string.Empty;
    [Required(ErrorMessage = "PermanentAddress_District_Required")]
    public string PermanentAddress_District { get; set; } = string.Empty;
    [Required(ErrorMessage = "PermanentAddress_Province_Required")]
    public string PermanentAddress_Province { get; set; } = string.Empty;
    [Required(ErrorMessage = "PermanentAddress_Country_Required")]
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

    [Required(ErrorMessage = "Email_Required")]
    [EmailAddress(ErrorMessage = "Email_Invalid")]
    public string Email { get; set; } = string.Empty;

    public string Nationality { get; set; } = string.Empty;

    [Required(ErrorMessage = "PhoneNumber_Required")]
    [Phone]
    [PhoneNumber("VN")]
    public string PhoneNumber { get; set; } = string.Empty;
}
