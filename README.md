# TECHCOMBANK CORE BANKING SYSTEM (WINUI 3 & .NET 8 & SQL SERVER)

Hệ thống quản lý tài khoản Core Banking xây dựng bằng công nghệ **WinUI 3 (Windows App SDK)** nguyên bản, giao diện Fluent Design Windows 11, kiến trúc **MVVM + Dependency Injection** và kết nối trực tiếp cơ sở dữ liệu **SQL Server (Windows Authentication)** bằng Entity Framework Core 8.

---

## 🚀 CÁCH KHỞI CHẠY ỨNG DỤNG

### 1. Khởi chạy 1-Click (Nhanh nhất)
Double-click vào file **`Chay_Ung_Dung.bat`** tại thư mục gốc để mở ngay ứng dụng.

### 2. Khởi chạy bằng Visual Studio 2022
1. Mở file **`NganHang.sln`** bằng Visual Studio 2022.
2. Chọn cấu hình **Debug | x64**.
3. Đặt project **`NganHang.App`** làm Startup Project (chuột phải > *Set as Startup Project*).
4. Nhấn **F5** để bắt đầu trải nghiệm!

---

## 🔑 TÀI KHOẢN ĐĂNG NHẬP
Mật khẩu mặc định: **`123456`** (đã mã hóa an toàn bằng thuật toán **BCrypt**).
Tại màn hình đăng nhập có sẵn 2 nút bấm đăng nhập nhanh 1-click:

- 👨‍💼 **Giao dịch viên (Teller):** `teller` / `123456`
  - Thực hiện các nghiệp vụ: Mở tài khoản thanh toán, chuyển khoản, mở sổ tiết kiệm (trích tài khoản hoặc nộp tiền mặt tại quầy), tất toán sổ, lập hồ sơ vay vốn & tính toán phương án trả nợ tín dụng, tra cứu khách hàng 360°, nhật ký giao dịch.
  - *Phân quyền an toàn:* Giao dịch viên không xem Dashboard doanh số và không xem Phòng chống rửa tiền AML.

- 🛡️ **Quản trị viên / Lãnh đạo (Admin):** `admin` / `123456`
  - Toàn quyền hệ thống: Xem toàn diện Dashboard thời gian thực và biểu đồ doanh số 7 ngày, phê duyệt và giải ngân các khoản vay vốn tín dụng, xử lý cảnh báo rửa tiền AML, cài đặt hệ thống.

---

## 🗄️ CƠ SỞ DỮ LIỆU SQL SERVER
- **Tên cơ sở dữ liệu:** `CoSoDuLieuCoreBanking` (kết nối trực tiếp tới `localhost\SQLEXPRESS`).
- **Xác thực:** Windows Authentication (Integrated Security, không cần User/Password).
- **Tệp tin CSDL lưu tại thư mục `database/`:**
  - **`CoSoDuLieuCoreBanking_Full_Script.sql`**: Tệp tin T-SQL trọn gói (khuyên dùng) chứa toàn bộ cấu trúc bảng, khóa ngoại, chỉ mục, triggers tự động, stored procedures và toàn bộ dữ liệu mẫu thực tế. Chạy trực tiếp trên bất kỳ phiên bản SQL Server nào (2016 -> 2022).
  - **`CoSoDuLieuCoreBanking.bak`**: Bản sao lưu trực tiếp (Backup) của SQL Server, hỗ trợ Restore 1-click trong SSMS.
  - **`HUONG_DAN_CAI_DAT_DATABASE.txt`**: Tài liệu hướng dẫn chi tiết các bước cài đặt CSDL cho giáo viên chấm bài.

---

## 📂 DANH MỤC 10 TÍNH NĂNG HOÀN CHỈNH
1. **Màn hình Đăng nhập (Login):** Giao diện Gradient hiện đại, đăng nhập nhanh 1-click, tự kiểm tra trạng thái SQL Server.
2. **Tổng quan hệ thống (Dashboard):** 4 thẻ KPI tài chính thời gian thực, biểu đồ cột doanh số 7 ngày, nhật ký giao dịch mới nhất (Dành riêng cho Quản trị viên/Lãnh đạo).
3. **Tra cứu khách hàng 360° (Customer 360):** Xem toàn diện hồ sơ định danh CIF, điểm tín dụng, đa tầng tài khoản thanh toán, sổ tiết kiệm và hợp đồng vay. Tích hợp tìm kiếm thời gian thực.
4. **Quản lý tài khoản thanh toán:** Hỗ trợ mở tài khoản cho khách hàng mới toanh hoặc khách hàng hiện hữu, tùy chọn số tài khoản đẹp hoặc tự sinh 14 chữ số ngẫu nhiên (`sp_MoTaiKhoanThanhToan`), quản lý hạn mức thấu chi, khóa/mở khóa tức thì.
5. **Giao dịch chuyển khoản:** Hạch toán ACID tức thời qua Stored Procedure `sp_ChuyenKhoanNoiBo`, tự động miễn phí giao dịch theo hạng hội viên VIP.
6. **Quản lý tiết kiệm:** Mở sổ trực tuyến (trích tài khoản) hoặc nộp tiền mặt tại quầy (không cần tài khoản thanh toán), tự sinh mã sổ hoặc nhập mã định danh tùy chọn, tự tính lãi suất kỳ hạn và tất toán đa phương thức.
7. **Quản lý tín dụng & Cho vay:** Lập hồ sơ vay vốn, tính toán phương án trả góp gốc/lãi niên kim từng kỳ, phê duyệt và giải ngân tự động vào tài khoản khách hàng.
8. **Phòng chống rửa tiền (AML):** Tự động phát hiện giao dịch ≥ 400 triệu VND bởi trigger database, cho phép cán bộ kiểm soát duyệt an toàn hoặc phong tỏa tài khoản.
9. **Chính sách phân hạng hội viên VIP:** 5 hạng (Chuẩn, Bạc, Vàng, Bạch Kim, Kim Cương), tự động thăng hạng theo tích lũy chi tiêu và cấp thấu chi không cần tài sản bảo đảm.
10. **Cài đặt hệ thống:** Chuyển đổi giao diện Sáng (Light) / Tối (Dark) / Theo Windows, đổi mật khẩu mã hóa BCrypt, kiểm tra kết nối SQL Server.
