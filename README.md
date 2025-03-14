1. Project Name: Student Management

2. Cấu trúc source code: Folder by Type
/StudentManagement
|-- /Controllers     chứa các controllers xử lý logic của ứng dụng
|-- /Migrations      chứa các file migrations khi sử dụng entity framework (ef) core để làm việc với database
|-- /Models          chứa các model class đại diện cho dữ liệu của ứng dụng
|-- /Properties      chứa file cấu hình launchSettings.json giúp thiết lập môi trường chạy cho ứng dụng
|-- /Views           chứa các giao diện người dùng trong mô hình MVC
|-- /wwwroot         chứa các file tĩnh CSS, JS, ... phục vụ cho giao diện người dùng

3. Hướng dẫn cài đặt và chạy chương trình:
- Điều chỉnh các thông số trong mục "DefaultConnection" trong file appsettings.json cho phù hợp
- Enable .NET trong command prompt với quyền admin:
	Dism /online /Enable-Feature /FeatureName:"NetFx3"
- Cài đặt các package cần thiết cho ứng dụng:
	dotnet restore
- Cài đặt dotnet-ef cho ứng dụng:
	dotnet tool install --global dotnet-ef
- Khởi tạo 1 migration mới trong Entity Framework Core:
	dotnet ef migrations add InitialCreate
- Cập nhật database theo migration mới nhất trong ef:
	dotnet ef database update
- Chạy ứng dụng:
	dotnet run