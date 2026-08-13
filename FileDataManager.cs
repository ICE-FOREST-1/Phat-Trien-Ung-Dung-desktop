using System;
using System.Collections.Generic;
using System.IO;

namespace QuanLyTiemThuoc.Services
{
    /// <summary>
    /// Lớp tiện ích chịu trách nhiệm đọc/ghi dữ liệu dạng dòng text (.txt).
    /// Dùng chung cho cả Customer và Transaction để tránh lặp code (nguyên tắc DRY).
    /// </summary>
    public static class FileDataManager
    {
        /// <summary>
        /// Đọc toàn bộ các dòng hợp lệ trong file. Nếu file chưa tồn tại -> tự tạo file rỗng
        /// và trả về danh sách rỗng (tránh lỗi FileNotFoundException ở lần chạy đầu tiên).
        /// </summary>
        public static List<string> ReadAllLines(string filePath)
        {
            EnsureFileExists(filePath);

            var result = new List<string>();
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        result.Add(line);
                }
            }
            return result;
        }

        /// <summary>
        /// Ghi đè toàn bộ danh sách dòng vào file (dùng sau mỗi lần thêm/sửa/xóa
        /// để đảm bảo file txt luôn phản ánh đúng dữ liệu hiện tại trong bộ nhớ).
        /// </summary>
        public static void WriteAllLines(string filePath, IEnumerable<string> lines)
        {
            EnsureDirectoryExists(filePath);

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var writer = new StreamWriter(stream))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        private static void EnsureFileExists(string filePath)
        {
            EnsureDirectoryExists(filePath);
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)) { }
            }
        }

        private static void EnsureDirectoryExists(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
