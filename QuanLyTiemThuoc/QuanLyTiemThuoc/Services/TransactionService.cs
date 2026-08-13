using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyTiemThuoc.Models;

namespace QuanLyTiemThuoc.Services
{
    /// <summary>
    /// Service quản lý giao dịch mua hàng và tính điểm tích lũy.
    /// Phụ thuộc vào CustomerService để cộng điểm cho đúng khách hàng sau mỗi giao dịch
    /// (composition, không kế thừa - đúng nguyên tắc OOP "has-a").
    /// </summary>
    public class TransactionService
    {
        private readonly string _filePath;
        private readonly CustomerService _customerService;
        private List<Transaction> _transactions;

        // Quy tắc tích điểm: cứ 10,000 VNĐ = 1 điểm (làm tròn xuống)
        private const decimal VND_PER_POINT = 10000m;

        public TransactionService(string filePath, CustomerService customerService)
        {
            _filePath = filePath;
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            _transactions = new List<Transaction>();
            Load();
        }

        public void Load()
        {
            var lines = FileDataManager.ReadAllLines(_filePath);
            _transactions = lines
                .Select(Transaction.FromFileLine)
                .Where(t => t != null)
                .ToList();
        }

        private void Save()
        {
            var lines = _transactions.Select(t => t.ToFileLine());
            FileDataManager.WriteAllLines(_filePath, lines);
        }

        public List<Transaction> GetAll()
        {
            return _transactions.OrderByDescending(t => t.TransactionDate).ToList();
        }

        /// <summary>Lấy lịch sử giao dịch của 1 khách hàng cụ thể, mới nhất lên đầu.</summary>
        public List<Transaction> GetByCustomer(int customerId)
        {
            return _transactions
                .Where(t => t.CustomerId == customerId)
                .OrderByDescending(t => t.TransactionDate)
                .ToList();
        }

        /// <summary>
        /// Tạo giao dịch mới cho khách hàng, tự động tính điểm tích lũy và cộng dồn
        /// vào tài khoản khách hàng đó thông qua CustomerService.
        /// </summary>
        public Transaction AddTransaction(int customerId, string description, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Số tiền giao dịch phải lớn hơn 0.");

            var customer = _customerService.GetById(customerId);
            if (customer == null)
                throw new InvalidOperationException("Không tìm thấy khách hàng để lập giao dịch.");

            int pointsEarned = (int)Math.Floor(amount / VND_PER_POINT);

            int newId = _transactions.Count == 0 ? 1 : _transactions.Max(t => t.Id) + 1;

            var transaction = new Transaction(newId, customerId, DateTime.Now,
                description?.Trim() ?? string.Empty, amount, pointsEarned);

            _transactions.Add(transaction);
            Save();

            // Cộng điểm tích lũy cho khách hàng sau khi giao dịch được lưu thành công
            if (pointsEarned > 0)
                _customerService.AddLoyaltyPoints(customerId, pointsEarned);

            return transaction;
        }

        /// <summary>
        /// Xóa toàn bộ giao dịch của 1 khách hàng - gọi khi khách hàng bị xóa
        /// để tránh tồn tại "giao dịch mồ côi" (orphan record) không có chủ trong file txt.
        /// </summary>
        public void DeleteAllByCustomer(int customerId)
        {
            int before = _transactions.Count;
            _transactions.RemoveAll(t => t.CustomerId == customerId);
            if (_transactions.Count != before)
                Save();
        }

        public decimal GetTotalSpent(int customerId)
        {
            return _transactions.Where(t => t.CustomerId == customerId).Sum(t => t.Amount);
        }
    }
}
