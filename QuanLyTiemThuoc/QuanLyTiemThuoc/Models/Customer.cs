using System;

namespace QuanLyTiemThuoc.Models
{
    /// <summary>
    /// Lớp đại diện cho một Khách hàng trong tiệm thuốc.
    /// Áp dụng OOP: đóng gói (encapsulation) toàn bộ thuộc tính khách hàng.
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime RegisterDate { get; set; }   // Ngày đăng ký tài khoản - chỉ set 1 lần khi tạo mới
        public int LoyaltyPoints { get; set; }        // Điểm tích lũy - LUÔN bắt đầu = 0 khi tạo mới

        public Customer()
        {
        }

        /// <summary>
        /// Constructor dùng khi TẠO MỚI khách hàng.
        /// Cố định RegisterDate = thời điểm hiện tại và LoyaltyPoints = 0
        /// để đảm bảo đúng yêu cầu nghiệp vụ (không cho phép gán điểm ban đầu khác 0).
        /// </summary>
        public Customer(int id, string fullName, string phoneNumber, string address)
        {
            Id = id;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Address = address;
            RegisterDate = DateTime.Now;
            LoyaltyPoints = 0;
        }

        /// <summary>
        /// Constructor đầy đủ dùng khi ĐỌC LẠI dữ liệu từ file txt.
        /// (Lúc này RegisterDate và LoyaltyPoints là dữ liệu đã lưu, không phải giá trị khởi tạo mới)
        /// </summary>
        public Customer(int id, string fullName, string phoneNumber, string address,
                         DateTime registerDate, int loyaltyPoints)
        {
            Id = id;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Address = address;
            RegisterDate = registerDate;
            LoyaltyPoints = loyaltyPoints;
        }

        /// <summary>
        /// Chuyển đối tượng thành 1 dòng text để lưu file (phân tách bằng dấu '|').
        /// Thay thế ký tự '|' và xuống dòng trong dữ liệu người dùng nhập để không phá vỡ định dạng file.
        /// </summary>
        public string ToFileLine()
        {
            return string.Join("|",
                Id,
                Sanitize(FullName),
                Sanitize(PhoneNumber),
                Sanitize(Address),
                RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"),
                LoyaltyPoints);
        }

        /// <summary>
        /// Parse 1 dòng text từ file thành đối tượng Customer.
        /// Trả về null nếu dòng dữ liệu bị lỗi định dạng (thay vì làm crash toàn bộ chương trình).
        /// </summary>
        public static Customer FromFileLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;

            var parts = line.Split('|');
            if (parts.Length != 6) return null;

            try
            {
                int id = int.Parse(parts[0]);
                string fullName = parts[1];
                string phone = parts[2];
                string address = parts[3];
                DateTime registerDate = DateTime.ParseExact(parts[4], "yyyy-MM-dd HH:mm:ss", null);
                int points = int.Parse(parts[5]);

                return new Customer(id, fullName, phone, address, registerDate, points);
            }
            catch
            {
                // Dòng dữ liệu hỏng -> bỏ qua, không làm sập chương trình
                return null;
            }
        }

        private static string Sanitize(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return input.Replace("|", "/").Replace("\r", " ").Replace("\n", " ").Trim();
        }

        public override string ToString() => FullName;
    }
}
