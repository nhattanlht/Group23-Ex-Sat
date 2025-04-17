using System.ComponentModel.DataAnnotations;

public class EnrollmentCreateDto
{
    [Required]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    public string ClassId { get; set; } = string.Empty;
}
