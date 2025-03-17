using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<SchoolYear> SchoolYears { get; set; }
        public DbSet<StudyProgram> StudyPrograms { get; set; }
        public DbSet<StudentStatus> StudentStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Khoa Luật" },
                new Department { Id = 2, Name = "Khoa Tiếng Anh thương mại" },
                new Department { Id = 3, Name = "Khoa Tiếng Nhật" },
                new Department { Id = 4, Name = "Khoa Tiếng Pháp" }
            );

            modelBuilder.Entity<SchoolYear>().HasData(
                new SchoolYear { Id = 1, Name = "18" },
                new SchoolYear { Id = 2, Name = "19" },
                new SchoolYear { Id = 3, Name = "20" },
                new SchoolYear { Id = 4, Name = "21" },
                new SchoolYear { Id = 5, Name = "22" },
                new SchoolYear { Id = 6, Name = "23" },
                new SchoolYear { Id = 7, Name = "24" }
            );

            modelBuilder.Entity<StudyProgram>().HasData(
                new StudyProgram { Id = 1, Name = "Chính quy" },
                new StudyProgram { Id = 2, Name = "Chất lượng cao" },
                new StudyProgram { Id = 3, Name = "Việt Pháp" },
                new StudyProgram { Id = 4, Name = "Tiên tiến" }
            );

            modelBuilder.Entity<StudentStatus>().HasData(
                new StudentStatus { Id = 1, Name = "Đang học" },
                new StudentStatus { Id = 2, Name = "Đã tốt nghiệp" },
                new StudentStatus { Id = 3, Name = "Đã thôi học" },
                new StudentStatus { Id = 4, Name = "Tạm dừng học" }
            );

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.SoDienThoai)
                .IsUnique();
        }
    }
}
