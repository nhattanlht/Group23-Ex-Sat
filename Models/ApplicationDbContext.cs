using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<StudentStatus> StudentStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Khoa Luật" },
                new Department { Id = 2, Name = "Khoa Tiếng Anh thương mại" },
                new Department { Id = 3, Name = "Khoa Tiếng Nhật" },
                new Department { Id = 4, Name = "Khoa Tiếng Pháp" }
            );

            modelBuilder.Entity<StudentStatus>().HasData(
                new StudentStatus { Id = 1, Name = "Đang học" },
                new StudentStatus { Id = 2, Name = "Đã tốt nghiệp" },
                new StudentStatus { Id = 3, Name = "Đã thôi học" },
                new StudentStatus { Id = 4, Name = "Tạm dừng học" }
            );
        }
    }
}
