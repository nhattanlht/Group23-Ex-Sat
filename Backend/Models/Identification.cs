using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Identification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? IdentificationType { get; set; } // CMND, CCCD, or Passport

        // Common fields
        [Required]
        public string? Number { get; set; } // Identification number (e.g., CMND/CCCD/Passport number)

        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; } // Date of issue

        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; } // Expiry date (nullable for CMND/CCCD)

        [Required]
        public string? IssuedBy { get; set; } // Place of issue

        // Specific to CCCD
        public bool? HasChip { get; set; } // Whether the CCCD has a chip (nullable for non-CCCD)

        // Specific to Passport
        public string? IssuingCountry { get; set; } // Country that issued the passport (nullable for non-passport)

        public string? Notes { get; set; } // Additional notes (optional)
    }
}