using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Quản lý toàn bộ danh sách khách hàng: đọc/ghi file data.txt,
    /// thêm/sửa/xóa/tìm kiếm, cộng điểm tích lũy, xếp hạng.
    ///
    /// Khóa chính của mỗi khách hàng là Số Điện Thoại (không được trùng).
    /// Lịch sử giao dịch KHÔNG lưu ở đây — sẽ nằm ở file/lớp riêng
    /// (giaodich.txt) và liên kết với khách hàng qua SoDienThoai.
    /// </summary>
    public class DsKhachHang
    {
        // Mặc định lưu cùng thư mục chạy chương trình (bin/Debug/net8.0-windows/data.txt).
        // Có thể truyền đường dẫn khác khi khởi tạo nếu cần.
        private readonly string duongDanFile;

        private List<KhachHang> danhSach;

        public DsKhachHang(string duongDanFile = "data.txt")
        {
            this.duongDanFile = duongDanFile;
            danhSach = new List<KhachHang>();
            DocFile();
        }

        // ==================== ĐỌC / GHI FILE ====================

        /// <summary>
        /// Đọc toàn bộ khách hàng từ data.txt vào bộ nhớ (nạp lại danhSach).
        /// Nếu file chưa tồn tại thì tự tạo file rỗng.
        /// Những dòng lỗi định dạng sẽ tự động bị bỏ qua (xem KhachHang.TuDongFile).
        /// </summary>
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
                KhachHang kh = KhachHang.TuDongFile(dong);
                if (kh != null)
                {
                    danhSach.Add(kh);
                }
            }
        }

        /// <summary>
        /// Ghi đè toàn bộ danh sách khách hàng hiện có xuống data.txt.
        /// Được gọi tự động sau mỗi lần Thêm/Sửa/Xóa/CộngĐiểm để dữ liệu
        /// trong file luôn đồng bộ với dữ liệu trong bộ nhớ.
        /// </summary>
        public void GhiFile()
        {
            List<string> cacDong = danhSach
                .Select(kh => kh.ToDongFile())
                .ToList();

            File.WriteAllLines(duongDanFile, cacDong, Encoding.UTF8);
        }

        // ==================== TRUY XUẤT / TÌM KIẾM ====================

        /// <summary>
        /// Lấy toàn bộ danh sách khách hàng (dùng để đổ dữ liệu lên DataGridView).
        /// </summary>
        public List<KhachHang> LayDanhSach()
        {
            return danhSach;
        }

        /// <summary>
        /// Tìm khách hàng theo số điện thoại khớp chính xác.
        /// Trả về null nếu không tìm thấy.
        /// </summary>
        public KhachHang TimTheoSoDienThoai(string soDienThoai)
        {
            if (string.IsNullOrWhiteSpace(soDienThoai))
                return null;

            string sdt = soDienThoai.Trim();
            return danhSach.FirstOrDefault(kh => kh.SoDienThoai == sdt);
        }

        /// <summary>
        /// Tìm gần đúng theo số điện thoại (chứa chuỗi nhập vào).
        /// Dùng cho ô tìm kiếm txtSoDienThoai + btTimKiem ở MainForm.
        /// Nếu từ khóa rỗng, trả về toàn bộ danh sách.
        /// </summary>
        public List<KhachHang> TimGanDungTheoSoDienThoai(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return danhSach;

            string tk = tuKhoa.Trim();
            return danhSach
                .Where(kh => kh.SoDienThoai.Contains(tk))
                .ToList();
        }

        /// <summary>
        /// Kiểm tra số điện thoại đã tồn tại trong danh sách hay chưa.
        /// </summary>
        public bool DaTonTai(string soDienThoai)
        {
            return TimTheoSoDienThoai(soDienThoai) != null;
        }

        // ==================== THÊM / SỬA / XÓA ====================

        /// <summary>
        /// Thêm khách hàng mới. Ngày đăng ký tự động lấy thời điểm hiện tại (DateTime.Now).
        /// Trả về false nếu số điện thoại rỗng hoặc đã tồn tại (không cho trùng khóa chính).
        /// </summary>
        public bool ThemKhachHang(string soDienThoai, string hoTen)
        {
            if (string.IsNullOrWhiteSpace(soDienThoai) || string.IsNullOrWhiteSpace(hoTen))
                return false;

            soDienThoai = soDienThoai.Trim();

            if (DaTonTai(soDienThoai))
                return false;

            KhachHang khachHangMoi = new KhachHang(soDienThoai, hoTen.Trim(), DateTime.Now, 0);
            danhSach.Add(khachHangMoi);
            GhiFile();
            return true;
        }

        /// <summary>
        /// Sửa họ tên của khách hàng theo số điện thoại.
        /// Trả về false nếu không tìm thấy khách hàng.
        /// </summary>
        public bool SuaKhachHang(string soDienThoai, string hoTenMoi)
        {
            KhachHang kh = TimTheoSoDienThoai(soDienThoai);
            if (kh == null || string.IsNullOrWhiteSpace(hoTenMoi))
                return false;

            kh.HoTen = hoTenMoi.Trim();
            GhiFile();
            return true;
        }

        /// <summary>
        /// Xóa khách hàng theo số điện thoại.
        /// Trả về false nếu không tìm thấy khách hàng.
        /// </summary>
        public bool XoaKhachHang(string soDienThoai)
        {
            KhachHang kh = TimTheoSoDienThoai(soDienThoai);
            if (kh == null)
                return false;

            danhSach.Remove(kh);
            GhiFile();
            return true;
        }

        // ==================== ĐIỂM TÍCH LŨY / XẾP HẠNG ====================

        /// <summary>
        /// Cộng điểm tích lũy cho khách hàng (gọi khi ghi nhận 1 giao dịch mua hàng mới).
        /// Trả về false nếu không tìm thấy khách hàng.
        /// </summary>
        public bool CongDiem(string soDienThoai, int soDiemCong)
        {
            KhachHang kh = TimTheoSoDienThoai(soDienThoai);
            if (kh == null)
                return false;

            kh.DiemTichLuy += soDiemCong;
            GhiFile();
            return true;
        }

        /// <summary>
        /// Lấy danh sách khách hàng đã sắp xếp theo điểm tích lũy giảm dần.
        /// Dùng cho chức năng bảng xếp hạng / tìm khách Top 1 trong tháng.
        /// </summary>
        public List<KhachHang> LayDanhSachXepHangTheoDiem()
        {
            return danhSach
                .OrderByDescending(kh => kh.DiemTichLuy)
                .ToList();
        }

        // ==================== RESET ĐIỂM ĐẦU NĂM MỚI ====================

        // File nhỏ chỉ lưu 1 dòng duy nhất: năm gần nhất đã thực hiện reset điểm.
        // Dùng để so sánh với năm hiện tại mỗi khi mở chương trình, tránh reset
        // lặp lại nhiều lần trong cùng 1 năm.
        private const string FILE_NAM_DA_RESET = "namreset.txt";

        /// <summary>
        /// Kiểm tra xem đã bước sang năm mới so với lần reset gần nhất chưa.
        /// Nếu có, tự động đưa điểm tích lũy của TẤT CẢ khách hàng về 0,
        /// ghi lại data.txt và cập nhật năm đã reset vào namreset.txt.
        /// Nên gọi 1 lần khi khởi động chương trình (MainForm_Load).
        /// Trả về true nếu vừa thực hiện reset, false nếu chưa cần reset.
        /// </summary>
        public bool KiemTraVaResetDiemDauNam()
        {
            int namHienTai = DateTime.Now.Year;
            int namDaResetGanNhat = DocNamDaReset();

            // Đã reset trong chính năm hiện tại rồi thì không làm gì thêm.
            if (namDaResetGanNhat == namHienTai)
                return false;

            foreach (KhachHang kh in danhSach)
            {
                kh.DiemTichLuy = 0;
            }

            GhiFile();
            GhiNamDaReset(namHienTai);
            return true;
        }

        private int DocNamDaReset()
        {
            if (!File.Exists(FILE_NAM_DA_RESET))
                return 0;

            string noiDung = File.ReadAllText(FILE_NAM_DA_RESET, Encoding.UTF8).Trim();

            int nam;
            return int.TryParse(noiDung, NumberStyles.Integer, CultureInfo.InvariantCulture, out nam)
                ? nam
                : 0;
        }

        private void GhiNamDaReset(int nam)
        {
            File.WriteAllText(FILE_NAM_DA_RESET, nam.ToString(CultureInfo.InvariantCulture), Encoding.UTF8);
        }
    }
}
