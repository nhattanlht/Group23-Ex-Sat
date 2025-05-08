using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using StudentManagement.Models;
using StudentManagement.Services;
using StudentManagement.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StudentManagement API", Version = "v1" });
});

// Register ApplicationDbContext with the connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Serilog với MSSqlServerSinkOptions
var columnOptions = new ColumnOptions();
columnOptions.Store.Remove(StandardColumn.Properties);
columnOptions.Store.Add(StandardColumn.LogEvent);
columnOptions.LogEvent.DataLength = 2048;

var sinkOptions = new MSSqlServerSinkOptions
{
    TableName = "Logs",
    AutoCreateSqlTable = true
};

// Định cấu hình Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
    .WriteTo.File("logs/errors.log", restrictedToMinimumLevel: LogEventLevel.Error)
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: sinkOptions,
        columnOptions: columnOptions
    )
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName() // Log môi trường chạy (Development/Production)
    .Enrich.WithThreadId() // Đã có package Serilog.Enrichers.Thread
    .CreateLogger();

// Đăng ký Serilog trong ASP.NET Core
builder.Host.UseSerilog();

// Đăng ký các interface service
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<ISchoolYearService, SchoolYearService>();
builder.Services.AddScoped<IStudentStatusService, StudentStatusService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IIdentificationService, IdentificationService>();

// Đăng ký các interface repository
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
builder.Services.AddScoped<ISchoolYearRepository, SchoolYearRepository>();
builder.Services.AddScoped<IStudentStatusRepository, StudentStatusRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IIdentificationRepository, IdentificationRepository>();


// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Đăng ký Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Validation service
builder.Services.AddSingleton<PhoneNumberValidationService>();

var app = builder.Build();

// Ensure the database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); // Apply migrations
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentManagement API v1"));
}

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll"); // Ensure CORS middleware is added after UseRouting
app.UseAuthorization();
app.UseSerilogRequestLogging(); // Ghi log request

app.MapControllers(); // Đăng ký HomeController

app.Run();
