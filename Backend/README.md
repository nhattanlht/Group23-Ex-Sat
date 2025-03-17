dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Mvc
dotnet add package Microsoft.AspNetCore.Razor.Runtime
dotnet add package Microsoft.Extensions.Logging.Console

//cai ef cho migration
dotnet tool install --global dotnet-ef

//khoi tao migrations
dotnet ef migrations add InitialCreate
//cap nhat database
dotnet ef database update
//chay ung dung
dotnet run

//appsetting.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=StudentManagementDB;User Id=sa;Password=Strong.Pwd-123;TrustServerCertificate=True"
  }
}
