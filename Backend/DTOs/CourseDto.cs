using System.ComponentModel.DataAnnotations;

public class CourseCreateDto
{
    public string CourseCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    [Range(2, 10, ErrorMessage = "Số tín chỉ phải từ 2 đến 10")]
    public int Credits { get; set; }
    public string? PrerequisiteCourseCode { get; set; }
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
