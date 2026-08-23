using System;
using System.Globalization;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Loại giảm giá của voucher: theo phần trăm hóa đơn, hoặc theo số tiền cố định.
    /// </summary>
    public enum LoaiVoucher
    {
        PhanTram,
        SoTienCoDinh
    }

    /// <summary>
    /// Đại diện cho một voucher giảm giá dùng khi thanh toán ở form MuaHang.
    /// Danh sách voucher lưu chung cho toàn cửa hàng (không gán riêng cho khách
    /// hàng nào) — khi thanh toán, nhân viên chọn voucher đang có sẵn (nếu có).
    /// </summary>
    public class Voucher
    {
        public string MaVoucher { get; set; }
        public LoaiVoucher Loai { get; set; }

        /// <summary>
        /// Nếu Loai = PhanTram: giá trị là % (VD 10 nghĩa là giảm 10%).
        /// Nếu Loai = SoTienCoDinh: giá trị là số tiền giảm trực tiếp (VNĐ).
        /// </summary>
        public decimal GiaTri { get; set; }

        public Voucher()
        {
        }

        public Voucher(string maVoucher, LoaiVoucher loai, decimal giaTri)
        {
            MaVoucher = maVoucher;
            Loai = loai;
            GiaTri = giaTri;
        }

        /// <summary>
        /// Tính số tiền được giảm dựa trên tổng tiền gốc của hóa đơn.
        /// Số tiền giảm không bao giờ vượt quá tổng tiền gốc (không cho hóa đơn âm).
        /// </summary>
        public decimal TinhSoTienGiam(decimal tongTienGoc)
        {
            decimal soTienGiam = Loai == LoaiVoucher.PhanTram
                ? Math.Round(tongTienGoc * GiaTri / 100m, 0, MidpointRounding.AwayFromZero)
                : GiaTri;

            if (soTienGiam < 0) soTienGiam = 0;
            if (soTienGiam > tongTienGoc) soTienGiam = tongTienGoc;

            return soTienGiam;
        }

        /// <summary>
        /// Mô tả ngắn gọn hiển thị trong danh sách chọn (comboBox).
        /// </summary>
        public string MoTa()
        {
            return Loai == LoaiVoucher.PhanTram
                ? $"{MaVoucher} (giảm {GiaTri:N0}%)"
                : $"{MaVoucher} (giảm {GiaTri:N0}đ)";
        }

        public override string ToString()
        {
            return MoTa();
        }

        /// <summary>
        /// Định dạng: MaVoucher|Loai|GiaTri
        /// Loai lưu dạng chữ: "PhanTram" hoặc "SoTienCoDinh".
        /// </summary>
        public string ToDongFile()
        {
            return string.Join("|",
                MaVoucher,
                Loai.ToString(),
                GiaTri.ToString(CultureInfo.InvariantCulture));
        }

        public static Voucher TuDongFile(string dong)
        {
            if (string.IsNullOrWhiteSpace(dong))
                return null;

            string[] cot = dong.Split('|');
            if (cot.Length < 3)
                return null;

            string maVoucher = cot[0].Trim();
            if (string.IsNullOrEmpty(maVoucher))
                return null;

            LoaiVoucher loai;
            if (!Enum.TryParse(cot[1].Trim(), true, out loai))
                loai = LoaiVoucher.SoTienCoDinh;

            decimal giaTri;
            decimal.TryParse(cot[2].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out giaTri);

            return new Voucher(maVoucher, loai, giaTri);
        }
    }
}
