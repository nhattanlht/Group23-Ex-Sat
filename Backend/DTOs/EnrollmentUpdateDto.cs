using System.ComponentModel.DataAnnotations;

public class EnrollmentUpdateDto
{
    [Required]
    public string StudentId { get; set; } = string.Empty;

    [Required]
    public string ClassId { get; set; } = string.Empty;

    public bool IsCancelled { get; set; } = false;

    public string? CancelReason { get; set; }
}
