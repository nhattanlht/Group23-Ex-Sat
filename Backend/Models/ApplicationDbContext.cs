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
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Identification> Identifications { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Data> Data { get; set; } // Add DbSet for Data

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Cấu hình dữ liệu mẫu
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

            // Cấu hình Unique Index cho Student
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.SoDienThoai)
                .IsUnique();

            // Cấu hình quan hệ khóa ngoại cho Student
            modelBuilder.Entity<Student>()
                .HasOne(s => s.DiaChiNhanThu)
                .WithMany()
                .HasForeignKey(s => s.DiaChiNhanThuId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.DiaChiThuongTru)
                .WithMany()
                .HasForeignKey(s => s.DiaChiThuongTruId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.DiaChiTamTru)
                .WithMany()
                .HasForeignKey(s => s.DiaChiTamTruId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Identification)
                .WithMany()
                .HasForeignKey(s => s.IdentificationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ khóa ngoại cho Course
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ khóa ngoại cho Enrollment
            _ = modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Class)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ khóa ngoại cho Grade
            _ = modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.MSSV)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Class)
                .WithMany(c => c.Grades)
                .HasForeignKey(g => g.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ khóa ngoại cho Class
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Course)
                .WithMany(co => co.Classes)
                .HasForeignKey(c => c.CourseCode)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình Unique Index cho Class
            modelBuilder.Entity<Class>()
                .HasIndex(c => c.ClassId)
                .IsUnique();

            // Cấu hình Unique Index cho Course
            modelBuilder.Entity<Course>()
                .HasIndex(c => c.CourseCode)
                .IsUnique();

            modelBuilder.Entity<Class>()
                .Property(c => c.ClassId)
                .IsRequired();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=ConnectionStrings:DefaultConnection");
            }
        }
    }
}
