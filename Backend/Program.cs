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

// Đăng ký các service
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<IdentificationService>();
builder.Services.AddScoped<ProgramService>();
builder.Services.AddScoped<SchoolYearService>();
builder.Services.AddScoped<StudentStatusService>();
builder.Services.AddScoped<CourseService>();


// Register CourseService
builder.Services.AddScoped<StudentManagement.Services.CourseService>();

// Đăng ký các repository
builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<DataRepository>(); // Register DataRepository
builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<IdentificationRepository>();
builder.Services.AddScoped<ProgramRepository>();
builder.Services.AddScoped<SchoolYearRepository>();
builder.Services.AddScoped<StudentStatusRepository>();
builder.Services.AddScoped<StudentRepository>();


// Register services and repositories
builder.Services.AddScoped<StudentManagement.Repositories.CourseRepository>();
builder.Services.AddScoped<StudentManagement.Services.CourseService>();
builder.Services.AddScoped<StudentManagement.Repositories.ClassRepository>();
builder.Services.AddScoped<StudentManagement.Services.ClassService>();


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
