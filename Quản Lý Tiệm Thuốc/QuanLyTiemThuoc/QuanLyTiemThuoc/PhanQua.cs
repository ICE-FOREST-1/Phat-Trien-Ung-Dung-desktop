using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Đại diện cho một phần quà trong danh sách đổi điểm (form DoiQua).
    /// Từ bản cập nhật này, danh sách quà được lưu trong file qua.txt (xem DsQua)
    /// thay vì hard-code cố định trong code, và mỗi phần quà được gán các
    /// THÁNG áp dụng cụ thể — quà chỉ hiện ra để đổi trong đúng những tháng đó
    /// (không hiển thị quanh năm).
    /// </summary>
    public class PhanQua
    {
        /// <summary>Tên phần quà, hiển thị trong danh sách chọn (comboBox).</summary>
        public string Ten { get; set; }

        /// <summary>Số điểm cần có để đổi được phần quà này.</summary>
        public int DiemCanDoi { get; set; }

        /// <summary>
        /// Danh sách các tháng (1-12) mà phần quà này được phép đổi.
        /// VD: {9, 10} nghĩa là chỉ đổi được trong tháng 9 và tháng 10.
        /// Không được để trống — mỗi phần quà bắt buộc phải gán ít nhất 1 tháng.
        /// </summary>
        public List<int> CacThangApDung { get; set; } = new List<int>();

        /// <summary>
        /// Mã voucher (tham chiếu tới Voucher trong voucher.txt) sẽ được CẤP RIÊNG
        /// cho khách hàng khi họ đổi phần quà này. Để trống nếu phần quà không
        /// phải là voucher giảm giá (VD quà vật lý như ly giữ nhiệt). Đây là cách
        /// DUY NHẤT để khách hàng có được voucher — voucher không tự do dùng được.
        /// </summary>
        public string MaVoucher { get; set; }

        public PhanQua()
        {
        }

        public PhanQua(string ten, int diemCanDoi, List<int> cacThangApDung, string maVoucher = null)
        {
            Ten = ten;
            DiemCanDoi = diemCanDoi;
            CacThangApDung = cacThangApDung ?? new List<int>();
            MaVoucher = maVoucher;
        }

        /// <summary>
        /// Kiểm tra phần quà này có được áp dụng trong 1 tháng cụ thể hay không.
        /// </summary>
        public bool ApDungChoThang(int thang)
        {
            return CacThangApDung != null && CacThangApDung.Contains(thang);
        }

        /// <summary>
        /// Hiển thị danh sách tháng dạng chuỗi "9, 10" để show cho người dùng xem.
        /// </summary>
        public string ThangApDungToString()
        {
            return CacThangApDung == null || CacThangApDung.Count == 0
                ? string.Empty
                : string.Join(", ", CacThangApDung.OrderBy(t => t));
        }

        /// <summary>
        /// ComboBox hiển thị theo ToString() khi không set DisplayMember,
        /// nên override để phòng trường hợp binding không dùng DisplayMember.
        /// </summary>
        public override string ToString()
        {
            return Ten;
        }

        /// <summary>
        /// Định dạng lưu file: Ten|DiemCanDoi|Thang1,Thang2,...|MaVoucher
        /// VD: Ly giữ nhiệt Tiệm Thuốc|50|9,10|
        /// VD có gắn voucher: Phiếu giảm giá 20.000đ|30|1,2,3|GIAM20K
        /// Cột MaVoucher có thể để trống (quà vật lý, không cấp voucher).
        /// </summary>
        public string ToDongFile()
        {
            return string.Join("|",
                Ten,
                DiemCanDoi.ToString(CultureInfo.InvariantCulture),
                string.Join(",", CacThangApDung),
                MaVoucher ?? string.Empty);
        }

        public static PhanQua TuDongFile(string dong)
        {
            if (string.IsNullOrWhiteSpace(dong))
                return null;

            string[] cot = dong.Split('|');
            if (cot.Length < 3)
                return null;

            string ten = cot[0].Trim();
            if (string.IsNullOrEmpty(ten))
                return null;

            int diemCanDoi;
            int.TryParse(cot[1].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out diemCanDoi);

            List<int> cacThang = new List<int>();
            string[] phanThang = cot[2].Trim().Split(',');
            foreach (string t in phanThang)
            {
                int thang;
                if (int.TryParse(t.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out thang)
                    && thang >= 1 && thang <= 12)
                {
                    cacThang.Add(thang);
                }
            }

            // Cột 4 (MaVoucher) là dữ liệu mới thêm sau này, có thể chưa tồn tại
            // ở các dòng cũ (dữ liệu cũ trước khi có tính năng này) -> để trống.
            string maVoucher = cot.Length >= 4 ? cot[3].Trim() : string.Empty;
            if (string.IsNullOrEmpty(maVoucher)) maVoucher = null;

            return new PhanQua(ten, diemCanDoi, cacThang, maVoucher);
        }
    }
}
