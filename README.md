1. Project Name: Student Management

2. Cấu trúc source code: Folder by Type
/StudentManagement
|-- /Backend		Chứa các thư mục và file phụ trách các thao tác backend
|------/Controllers		Chứa các bộ điều khiển xử lý yêu cầu và điều phối logic giữa Models và Services
|------/DTOs			Chứa các đối tượng dùng để truyền dữ liệu giữa client và server
|------/Migrations		Chứa các tập tin quản lý cơ sở dữ liệu thông qua Entity Framework
|------/Models			Chứa các mô hình đại diện cho cấu trúc dữ liệu trong ứng dụng
|------/Properties		Chứa tập tin cấu hình ứng dụng
|------/Repositories		Chứa các tập tin truy cập và thao tác trên cơ sở dữ liệu
|------/Services 		Chứa các lớp xử lý nghiệp vụ, giúp giữ Controllers gọn gàng
|------/logs			Chứa các tệp nhật ký (logs) để theo dõi hoạt động của hệ thống
|------/wwwroot			Chứa các tài nguyên hình ảnh, CSS, JavaScript để phục vụ frontend
|------/CsvMapping		Chứa dữ liệu liên quan đến việc ánh xạ từ CSV vào các model của ứng dụng
|------/Validators		Chứa các logic liên quan đến việc kiểm tra dữ liệu đầu vào
|-- /Backend.Tests 	Chứa các file phục vụ cho unit testing
|-- /Frontend		Chứa các thư mục và file phụ trách các thao tác frontend
|------/public			Chứa các thư mục hình ảnh, favicon và index.html, lưu trữ các tài nguyên không thay đổi
				trong quá trình chạy ứng dụng
|------/src			Chứa mã nguồn chính của frontend, gồm các thành phần React, CSS, JavaScript và logic
				ứng dụng

3. Hướng dẫn cài đặt và chạy chương trình:
- Backend:
	+ Điều chỉnh các thông số trong mục "DefaultConnection" trong file appsettings.json cho phù hợp
	+ Enable .NET trong command prompt với quyền admin:
		Dism /online /Enable-Feature /FeatureName:"NetFx3"
	+ Khởi chạy Docker hoặc SQLServer
	+ Tạo database StudentManagementDB trong cơ sở dữ liệu
	+ Mở command prompt trong folder Backend
	+ Cài đặt các package cần thiết cho backend:
		dotnet restore
	+ Cài đặt dotnet-ef cho ứng dụng:
		dotnet tool install --global dotnet-ef
	+ Cập nhật database theo migration mới nhất trong ef:
		dotnet ef database update
	+ Chạy backend:
		dotnet run
- Backend.Tests:
	+ Mở command prompt trong folder Backend.Tests
	+ Chạy lệnh sau để thực hiện các unit test đã được cài đặt:
		dotnet test
- Frontend:
	+ Mở command prompt trong folder Frontend
	+ Cài đặt các package cần thiết cho frontend:
		npm i
	+ Chạy frontend:
		npm start

4. Hướng dẫn sử dụng + minh chứng Version 2.0:
- Import file:
	https://drive.google.com/drive/folders/1-BLw2wAikQIU9IeGeS8nEkVJX_w_oBeR?usp=sharing
- Export file:
	https://drive.google.com/drive/folders/1V_W5m6Mu9YoIth0SkvHs4kLlanlR0Kak?usp=sharing
- Searching:
	https://drive.google.com/drive/folders/1dzJIWdRTL6HzP4sxGBUjz9EihVIPVnRu?usp=sharing
- Logging:
	https://drive.google.com/drive/folders/1ZI9GggYPb2FEz-4j5Kd274uA5l1-tXxn?usp=sharing
- Department Configuration:
	https://drive.google.com/drive/folders/1JQBREQKBxFH-iU_CdWvQdee_E5f2Az9q?usp=sharing

5. Hướng dẫn sử dụng + minh chứng Version 3.0:
- Business Rule Id:
	+ Khi thêm hoặc sửa mã số sinh viên thành 1 mã số đã tồn tại thì sẽ báo lỗi
	+ Minh chứng:
		https://drive.google.com/drive/folders/1fAxrcf5GOUreYNVTwavUleqmTMMZyMuS?usp=sharing
- Business Rule Email:
	+ Điều chỉnh email ending trong file .env theo ý muốn, khi cập nhật hoặc thêm sinh viên với email có đuôi khác
	giá trị đã chọn sẽ thông báo lỗi
	+ Minh chứng:
		https://drive.google.com/drive/folders/1DalKguHN87ox5Hbpl4l8Z0mYd3UxhYBn?usp=sharing
- Business Rule Phone:
	+ Điều chỉnh giá trị số điện thoại mong muốn ở file Backend/appsettings.json, khi cập nhật hoặc thêm sinh viên
	với số điện thoại không đúng quy định trong file đó thì sẽ thông báo lỗi
	+ Minh chứng:
		https://drive.google.com/drive/folders/1TzjuF3HJFBZ0sdtbB-P2TPkbubkgYpTQ?usp=sharing
- Business Rule Status:
	https://drive.google.com/drive/folders/1joTf3GKLsAT9NXy9gW_yQJCeYOTISakM?usp=sharing

6. Hướng dẫn sử dụng + minh chứng Version 5.0:
- Course:
	https://drive.google.com/drive/folders/1jSxYSmPIvIQqwRJZ29d3OQLY5OobYuji?usp=sharing
- Class:
	https://drive.google.com/drive/folders/1Q_TXWGeEX5rV9SprpzHpWZDoAA_e71xn?usp=sharing
- Enrollment:
	https://drive.google.com/drive/folders/10emL71Get2U91BW89ZFbsLfoiBABoRFE?usp=sharing
- Grade:
	https://drive.google.com/drive/folders/1cRW4WL6anyVKzFHnglffkPMtjoY1201U?usp=sharing