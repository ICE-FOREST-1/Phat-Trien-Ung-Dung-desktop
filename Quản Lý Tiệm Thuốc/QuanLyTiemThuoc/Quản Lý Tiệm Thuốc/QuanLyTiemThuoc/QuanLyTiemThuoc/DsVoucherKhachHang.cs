using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Quản lý danh sách voucher RIÊNG của từng khách hàng (đọc/ghi voucherkh.txt).
    /// Khách hàng chỉ có voucher trong danh sách này sau khi đã đổi quà thành công
    /// (xem DoiQua.cs) — vì vậy khi thanh toán ở form MuaHang, khách chỉ được chọn
    /// những voucher đang sở hữu VÀ chưa dùng, không được dùng voucher tự do.
    /// </summary>
    public class DsVoucherKhachHang
    {
        private readonly string duongDanFile;
        private List<VoucherKhachHang> danhSach;

        public DsVoucherKhachHang(string duongDanFile = "voucherkh.txt")
        {
            this.duongDanFile = duongDanFile;
            danhSach = new List<VoucherKhachHang>();
            DocFile();
        }

        public void DocFile()
        {
            danhSach.Clear();

            if (!File.Exists(duongDanFile))
            {
                File.Create(duongDanFile).Dispose();
                return;
            }

            string[] cacDong = File.ReadAllLines(duongDanFile, Encoding.UTF8);
            foreach (string dong in cacDong)
            {
                VoucherKhachHang vc = VoucherKhachHang.TuDongFile(dong);
                if (vc != null)
                {
                    danhSach.Add(vc);
                }
            }
        }

        public void GhiFile()
        {
            List<string> cacDong = danhSach.Select(v => v.ToDongFile()).ToList();
            File.WriteAllLines(duongDanFile, cacDong, Encoding.UTF8);
        }

        /// <summary>
        /// Cấp 1 voucher mới cho khách hàng (gọi khi khách đổi quà thành công và
        /// phần quà đó có gắn mã voucher).
        /// </summary>
        public VoucherKhachHang CapVoucher(string soDienThoai, string maVoucher)
        {
            if (string.IsNullOrWhiteSpace(soDienThoai) || string.IsNullOrWhiteSpace(maVoucher))
                return null;

            VoucherKhachHang voucherMoi = new VoucherKhachHang(
                soDienThoai.Trim(), maVoucher.Trim(), DateTime.Now, false);

            danhSach.Add(voucherMoi);
            GhiFile();
            return voucherMoi;
        }

        /// <summary>
        /// Lấy danh sách voucher CÒN DÙNG ĐƯỢC (chưa sử dụng) của 1 khách hàng.
        /// </summary>
        public List<VoucherKhachHang> LayVoucherKhaDung(string soDienThoai)
        {
            if (string.IsNullOrWhiteSpace(soDienThoai))
                return new List<VoucherKhachHang>();

            string sdt = soDienThoai.Trim();
            return danhSach
                .Where(v => v.SoDienThoai == sdt && !v.DaSuDung)
                .OrderBy(v => v.ThoiGianNhan)
                .ToList();
        }

        /// <summary>
        /// Đánh dấu 1 voucher cụ thể của khách hàng là ĐÃ SỬ DỤNG (gọi sau khi
        /// thanh toán thành công ở form MuaHang có áp dụng voucher đó).
        /// </summary>
        public bool DanhDauDaSuDung(VoucherKhachHang voucher)
        {
            if (voucher == null || !danhSach.Contains(voucher))
                return false;

            voucher.DaSuDung = true;
            GhiFile();
            return true;
        }
    }
}
