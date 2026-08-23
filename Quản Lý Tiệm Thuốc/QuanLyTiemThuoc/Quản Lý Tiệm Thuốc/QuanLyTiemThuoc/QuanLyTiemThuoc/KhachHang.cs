using System;
using System.Globalization;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Đại diện cho một khách hàng trong tiệm thuốc.
    /// Khóa chính (không được trùng) là SoDienThoai.
    /// </summary>
    public class KhachHang
    {
        /// <summary>
        /// Định dạng ngày giờ dùng thống nhất khi đọc/ghi data.txt.
        /// Phải dùng chung 1 định dạng để tránh lỗi lệch múi/khác định dạng
        /// giữa lúc ghi và lúc đọc lại.
        /// </summary>
        public const string DINH_DANG_NGAY = "dd/MM/yyyy HH:mm:ss";

        public string SoDienThoai { get; set; }
        public string HoTen { get; set; }
        public DateTime NgayDangKy { get; set; }
        public int DiemTichLuy { get; set; }

        public KhachHang()
        {
        }

        public KhachHang(string soDienThoai, string hoTen, DateTime ngayDangKy, int diemTichLuy = 0)
        {
            SoDienThoai = soDienThoai;
            HoTen = hoTen;
            NgayDangKy = ngayDangKy;
            DiemTichLuy = diemTichLuy;
        }

        /// <summary>
        /// Chuyển khách hàng thành 1 dòng text để lưu vào data.txt.
        /// Định dạng: SoDienThoai|HoTen|NgayDangKy|DiemTichLuy
        /// </summary>
        public string ToDongFile()
        {
            return string.Join("|",
                SoDienThoai,
                HoTen,
                NgayDangKy.ToString(DINH_DANG_NGAY, CultureInfo.InvariantCulture),
                DiemTichLuy.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Phân tích 1 dòng text đọc từ data.txt thành đối tượng KhachHang.
        /// Trả về null nếu dòng rỗng hoặc sai định dạng (để DsKhachHang tự bỏ qua
        /// những dòng lỗi thay vì làm crash cả chương trình).
        /// </summary>
        public static KhachHang TuDongFile(string dong)
        {
            if (string.IsNullOrWhiteSpace(dong))
                return null;

            string[] cot = dong.Split('|');
            if (cot.Length < 4)
                return null;

            string soDienThoai = cot[0].Trim();
            string hoTen = cot[1].Trim();

            if (string.IsNullOrEmpty(soDienThoai))
                return null;

            DateTime ngayDangKy;
            if (!DateTime.TryParseExact(
                    cot[2].Trim(),
                    DINH_DANG_NGAY,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out ngayDangKy))
            {
                // Dòng dữ liệu cũ/lỗi định dạng ngày: dùng thời điểm hiện tại thay vì bỏ cả dòng
                ngayDangKy = DateTime.Now;
            }

            int diemTichLuy;
            if (!int.TryParse(cot[3].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out diemTichLuy))
            {
                diemTichLuy = 0;
            }

            return new KhachHang(soDienThoai, hoTen, ngayDangKy, diemTichLuy);
        }

        public override string ToString()
        {
            return HoTen;
        }
    }
}
