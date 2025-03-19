using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Address
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string HouseNumber { get; set; }

    [Required]
    public string StreetName { get; set; }

    [Required]
    public string Ward { get; set; }

    [Required]
    public string District { get; set; }

    [Required]
    public string Province { get; set; }

    [Required]
    public string Country { get; set; }

    public override string ToString()
    {
        return $"{HouseNumber}, {StreetName}, {Ward}, {District}, {Province}, {Country}";
    }
}