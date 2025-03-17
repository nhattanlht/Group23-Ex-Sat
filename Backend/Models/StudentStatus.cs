using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class StudentStatus
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}