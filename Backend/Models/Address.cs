using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Address
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string? HouseNumber { get; set; } // Make nullable

    [Required]
    public string? StreetName { get; set; } // Make nullable

    [Required]
    public string? Ward { get; set; } // Make nullable

    [Required]
    public string? District { get; set; } // Make nullable

    [Required]
    public string? Province { get; set; } // Make nullable

    [Required]
    public string? Country { get; set; } // Make nullable

    public override string ToString()
    {
        return $"{HouseNumber}, {StreetName}, {Ward}, {District}, {Province}, {Country}";
    }
}