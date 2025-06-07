using System.ComponentModel.DataAnnotations;

public class GradeCreateDTO
{
    [Required]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    public string ClassId { get; set; } = string.Empty;

    [Required]
    [Range(0, 10)]
    public double Score { get; set; }

    [MaxLength(5)]
    public string? GradeLetter { get; set; }

    [Range(0.0, 4.0)]
    public double GPA { get; set; }
}
