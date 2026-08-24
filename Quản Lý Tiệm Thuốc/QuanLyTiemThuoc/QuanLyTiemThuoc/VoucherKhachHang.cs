using System;
using System.Globalization;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Đại diện cho 1 voucher mà MỘT khách hàng cụ thể đang sở hữu, có được
    /// do đã đổi điểm lấy quà (form DoiQua) chứ KHÔNG phải voucher tự do ai
    /// cũng dùng được. Mỗi dòng là 1 voucher cụ thể của 1 khách hàng, dùng
    /// xong (DaSuDung = true) thì không thể dùng lại lần 2.
    /// </summary>
    public class VoucherKhachHang
    {
        public string SoDienThoai { get; set; }
        public string MaVoucher { get; set; }
        public DateTime ThoiGianNhan { get; set; }
        public bool DaSuDung { get; set; }

        public VoucherKhachHang()
        {
        }

        public VoucherKhachHang(string soDienThoai, string maVoucher, DateTime thoiGianNhan, bool daSuDung)
        {
            SoDienThoai = soDienThoai;
            MaVoucher = maVoucher;
            ThoiGianNhan = thoiGianNhan;
            DaSuDung = daSuDung;
        }

        /// <summary>
        /// Định dạng: SoDienThoai|MaVoucher|ThoiGianNhan|DaSuDung
        /// </summary>
        public string ToDongFile()
        {
            return string.Join("|",
                SoDienThoai,
                MaVoucher,
                ThoiGianNhan.ToString(KhachHang.DINH_DANG_NGAY, CultureInfo.InvariantCulture),
                DaSuDung ? "1" : "0");
        }

        public static VoucherKhachHang TuDongFile(string dong)
        {
            if (string.IsNullOrWhiteSpace(dong))
                return null;

            string[] cot = dong.Split('|');
            if (cot.Length < 4)
                return null;

            string soDienThoai = cot[0].Trim();
            string maVoucher = cot[1].Trim();
            if (string.IsNullOrEmpty(soDienThoai) || string.IsNullOrEmpty(maVoucher))
                return null;

            DateTime thoiGianNhan;
            DateTime.TryParseExact(cot[2].Trim(), KhachHang.DINH_DANG_NGAY, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out thoiGianNhan);

            bool daSuDung = cot[3].Trim() == "1";

            return new VoucherKhachHang(soDienThoai, maVoucher, thoiGianNhan, daSuDung);
        }
    }
}
