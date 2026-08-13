using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyTiemThuoc.Models;

namespace QuanLyTiemThuoc.Services
{
    /// <summary>
    /// Service chịu trách nhiệm nghiệp vụ CRUD cho Khách hàng và lưu/đọc file txt.
    /// Toàn bộ danh sách được giữ trong bộ nhớ (_customers) và đồng bộ ra file sau mỗi thay đổi.
    /// </summary>
    public class CustomerService
    {
        private readonly string _filePath;
        private List<Customer> _customers;

        public CustomerService(string filePath)
        {
            _filePath = filePath;
            _customers = new List<Customer>();
            Load();
        }

        /// <summary>Đọc dữ liệu khách hàng từ file txt vào bộ nhớ.</summary>
        public void Load()
        {
            var lines = FileDataManager.ReadAllLines(_filePath);
            _customers = lines
                .Select(Customer.FromFileLine)
                .Where(c => c != null)
                .ToList();
        }

        /// <summary>Ghi toàn bộ danh sách khách hàng hiện tại xuống file txt.</summary>
        private void Save()
        {
            var lines = _customers.Select(c => c.ToFileLine());
            FileDataManager.WriteAllLines(_filePath, lines);
        }

        /// <summary>Lấy danh sách tất cả khách hàng (bản sao để tránh sửa trực tiếp danh sách gốc từ bên ngoài).</summary>
        public List<Customer> GetAll()
        {
            return _customers.OrderBy(c => c.Id).ToList();
        }

        public Customer GetById(int id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }

        public List<Customer> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetAll();
            keyword = keyword.Trim().ToLower();
            return _customers
                .Where(c => (c.FullName?.ToLower().Contains(keyword) ?? false)
                         || (c.PhoneNumber?.ToLower().Contains(keyword) ?? false))
                .OrderBy(c => c.Id)
                .ToList();
        }

        /// <summary>
        /// Thêm khách hàng mới.
        /// LUÔN gán LoyaltyPoints = 0 và RegisterDate = thời điểm hiện tại
        /// bất kể tham số truyền vào là gì -> đảm bảo đúng yêu cầu nghiệp vụ,
        /// tránh lỗi logic nếu sau này có người vô tình truyền điểm khởi tạo khác 0.
        /// </summary>
        public Customer AddCustomer(string fullName, string phoneNumber, string address)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Tên khách hàng không được để trống.");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Số điện thoại không được để trống.");

            if (IsPhoneNumberExists(phoneNumber, excludeId: null))
                throw new InvalidOperationException("Số điện thoại này đã được đăng ký cho khách hàng khác.");

            int newId = _customers.Count == 0 ? 1 : _customers.Max(c => c.Id) + 1;

            var customer = new Customer(newId, fullName.Trim(), phoneNumber.Trim(), address?.Trim())
            {
                // Ghi rõ ràng lại để không ai hiểu nhầm - đúng yêu cầu: điểm tích lũy = 0 khi tạo mới
                LoyaltyPoints = 0
            };

            _customers.Add(customer);
            Save();
            return customer;
        }

        /// <summary>
        /// Cập nhật thông tin khách hàng.
        /// KHÔNG cho phép sửa RegisterDate và KHÔNG reset LoyaltyPoints về 0
        /// (chỉ sửa thông tin cá nhân: tên, sđt, địa chỉ).
        /// </summary>
        public void UpdateCustomer(int id, string fullName, string phoneNumber, string address)
        {
            var customer = GetById(id);
            if (customer == null)
                throw new InvalidOperationException("Không tìm thấy khách hàng.");

            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Tên khách hàng không được để trống.");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Số điện thoại không được để trống.");

            if (IsPhoneNumberExists(phoneNumber, excludeId: id))
                throw new InvalidOperationException("Số điện thoại này đã được đăng ký cho khách hàng khác.");

            customer.FullName = fullName.Trim();
            customer.PhoneNumber = phoneNumber.Trim();
            customer.Address = address?.Trim();
            // RegisterDate và LoyaltyPoints giữ nguyên - không đụng vào khi Sửa

            Save();
        }

        public void DeleteCustomer(int id)
        {
            var customer = GetById(id);
            if (customer == null)
                throw new InvalidOperationException("Không tìm thấy khách hàng.");

            _customers.Remove(customer);
            Save();
        }

        /// <summary>
        /// Cộng điểm tích lũy cho khách hàng (gọi từ TransactionService khi có giao dịch mới).
        /// Không cho phép cộng điểm âm ngoài ý muốn.
        /// </summary>
        public void AddLoyaltyPoints(int customerId, int points)
        {
            if (points < 0)
                throw new ArgumentException("Điểm cộng thêm không được âm.");

            var customer = GetById(customerId);
            if (customer == null)
                throw new InvalidOperationException("Không tìm thấy khách hàng.");

            customer.LoyaltyPoints += points;
            Save();
        }

        private bool IsPhoneNumberExists(string phoneNumber, int? excludeId)
        {
            return _customers.Any(c =>
                c.PhoneNumber.Equals(phoneNumber.Trim(), StringComparison.OrdinalIgnoreCase)
                && (excludeId == null || c.Id != excludeId.Value));
        }
    }
}
