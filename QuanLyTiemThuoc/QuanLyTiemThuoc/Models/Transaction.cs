using System;

namespace QuanLyTiemThuoc.Models
{
    /// <summary>
    /// Lớp đại diện cho 1 giao dịch mua hàng của khách hàng.
    /// Mỗi giao dịch gắn với 1 CustomerId (khóa ngoại logic).
    /// </summary>
    public class Transaction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }     // Ví dụ: "Mua thuốc cảm + vitamin C"
        public decimal Amount { get; set; }          // Số tiền giao dịch (VNĐ)
        public int PointsEarned { get; set; }        // Điểm tích lũy nhận được từ giao dịch này

        public Transaction() { }

        public Transaction(int id, int customerId, DateTime transactionDate,
                            string description, decimal amount, int pointsEarned)
        {
            Id = id;
            CustomerId = customerId;
            TransactionDate = transactionDate;
            Description = description;
            Amount = amount;
            PointsEarned = pointsEarned;
        }

        public string ToFileLine()
        {
            return string.Join("|",
                Id,
                CustomerId,
                TransactionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Sanitize(Description),
                Amount.ToString(System.Globalization.CultureInfo.InvariantCulture),
                PointsEarned);
        }

        public static Transaction FromFileLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;

            var parts = line.Split('|');
            if (parts.Length != 6) return null;

            try
            {
                int id = int.Parse(parts[0]);
                int customerId = int.Parse(parts[1]);
                DateTime date = DateTime.ParseExact(parts[2], "yyyy-MM-dd HH:mm:ss", null);
                string description = parts[3];
                decimal amount = decimal.Parse(parts[4], System.Globalization.CultureInfo.InvariantCulture);
                int points = int.Parse(parts[5]);

                return new Transaction(id, customerId, date, description, amount, points);
            }
            catch
            {
                return null;
            }
        }

        private static string Sanitize(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return input.Replace("|", "/").Replace("\r", " ").Replace("\n", " ").Trim();
        }
    }
}
