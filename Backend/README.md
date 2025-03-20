dotnet restore

//cai ef cho migration
dotnet tool install --global dotnet-ef
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
