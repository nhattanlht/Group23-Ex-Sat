using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class ImportRequest
{
    [Required]
    public IFormFile File { get; set; }
}
