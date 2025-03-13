using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Khoa> CacKhoa { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<StudentStatus> StudentStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Khoa Luật" },
                new Department { Id = 2, Name = "Khoa Tiếng Anh thương mại" },
                new Department { Id = 3, Name = "Khoa Tiếng Nhật" },
                new Department { Id = 4, Name = "Khoa Tiếng Pháp" }
            );

            modelBuilder.Entity<Khoa>().HasData(
                new Khoa { Id = 1, Name = "18" },
                new Khoa { Id = 2, Name = "19" },
                new Khoa { Id = 3, Name = "20" },
                new Khoa { Id = 4, Name = "21" },
                new Khoa { Id = 5, Name = "22" },
                new Khoa { Id = 6, Name = "23" },
                new Khoa { Id = 7, Name = "24" }
            );

            modelBuilder.Entity<Program>().HasData(
                new Program { Id = 1, Name = "Chính quy" },
                new Program { Id = 2, Name = "Chất lượng cao" },
                new Program { Id = 3, Name = "Việt Pháp" },
                new Program { Id = 4, Name = "Tiên tiến" }
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
