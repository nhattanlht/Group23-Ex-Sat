using System.ComponentModel.DataAnnotations;

public class GradeUpdateDTO
{
    [Required]
    public double Score { get; set; }

    [MaxLength(5)]
    public string? GradeLetter { get; set; }

    [Range(0.0, 4.0)]
    public double GPA { get; set; }
}
