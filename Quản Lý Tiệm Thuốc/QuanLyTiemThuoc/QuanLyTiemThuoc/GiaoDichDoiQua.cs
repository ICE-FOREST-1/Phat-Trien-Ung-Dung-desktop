using System;
using System.Globalization;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Đại diện cho 1 LẦN đổi quà của khách hàng (dùng cho màn hình Lịch Sử Đổi
    /// Quà). Không đặt tên trùng "LichSuDoiQua" vì tên đó đã là Form.
    /// </summary>
    public class GiaoDichDoiQua
    {
        public string MaDoiQua { get; set; }
        public string SoDienThoai { get; set; }
        public string TenQua { get; set; }
        public DateTime ThoiGian { get; set; }
        public int ChiPhi { get; set; }
        public string TrangThai { get; set; }

        public GiaoDichDoiQua()
        {
        }

        public GiaoDichDoiQua(string maDoiQua, string soDienThoai, string tenQua, DateTime thoiGian, int chiPhi, string trangThai)
        {
            MaDoiQua = maDoiQua;
            SoDienThoai = soDienThoai;
            TenQua = tenQua;
            ThoiGian = thoiGian;
            ChiPhi = chiPhi;
            TrangThai = trangThai;
        }

        /// <summary>
        /// Định dạng: MaDoiQua|SoDienThoai|TenQua|ThoiGian|ChiPhi|TrangThai
        /// </summary>
        public string ToDongFile()
        {
            return string.Join("|",
                MaDoiQua,
                SoDienThoai,
                TenQua,
                ThoiGian.ToString(KhachHang.DINH_DANG_NGAY, CultureInfo.InvariantCulture),
                ChiPhi.ToString(CultureInfo.InvariantCulture),
                TrangThai);
        }

        public static GiaoDichDoiQua TuDongFile(string dong)
        {
            if (string.IsNullOrWhiteSpace(dong))
                return null;

            string[] cot = dong.Split('|');
            if (cot.Length < 6)
                return null;

            string maDoiQua = cot[0].Trim();
            string soDienThoai = cot[1].Trim();
            if (string.IsNullOrEmpty(maDoiQua) || string.IsNullOrEmpty(soDienThoai))
                return null;

            string tenQua = cot[2].Trim();

            DateTime thoiGian;
            DateTime.TryParseExact(cot[3].Trim(), KhachHang.DINH_DANG_NGAY, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out thoiGian);

            int chiPhi;
            int.TryParse(cot[4].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out chiPhi);

            string trangThai = cot[5].Trim();

            return new GiaoDichDoiQua(maDoiQua, soDienThoai, tenQua, thoiGian, chiPhi, trangThai);
        }
    }
}
