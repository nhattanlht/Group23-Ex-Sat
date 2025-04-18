public class GradeResponseDTO
{
    public int GradeId { get; set; }

    public string MSSV { get; set; } = string.Empty;

    public string ClassId { get; set; } = string.Empty;

    public double Score { get; set; }

    public string? GradeLetter { get; set; }

    public double GPA { get; set; }

    public string CourseName { get; set; } = string.Empty;

    public int Credit { get; set; }
}
