using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using StudentManagement.Models;
using StudentManagement.Services;
using StudentManagement.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext với chuỗi kết nối từ appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đảm bảo database đã được tạo
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); // Tạo database nếu chưa có
}

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
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<DataService>();

// Đăng ký các repository
builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<DataRepository>();

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

app.UseCors("AllowAll");
// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseSerilogRequestLogging(); // Ghi log request

app.MapControllers(); // Đăng ký HomeController

app.Run();
