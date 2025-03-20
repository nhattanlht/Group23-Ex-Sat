using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using StudentManagement.Models;
using StudentManagement.Services;


var builder = WebApplication.CreateBuilder(args);

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

// Đăng ký DbContext với chuỗi kết nối từ appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký các service
builder.Services.AddScoped<IStudentService, StudentService>();

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

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.UseSerilogRequestLogging(); // Ghi log request

app.MapControllers(); // Đăng ký HomeController

app.Run();
