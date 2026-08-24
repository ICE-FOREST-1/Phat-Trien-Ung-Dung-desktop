using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Quản lý toàn bộ danh sách giao dịch mua hàng: đọc/ghi file giaodich.txt,
    /// thêm giao dịch mới, tra cứu lịch sử theo số điện thoại khách hàng.
    ///
    /// Lưu ý: lớp này CHỈ lo việc lưu trữ giao dịch và tính điểm của riêng giao dịch đó.
    /// Việc cộng điểm tích lũy vào hồ sơ khách hàng (DsKhachHang.CongDiem) do nơi gọi
    /// (ví dụ form MuaHang) tự thực hiện sau khi ThemGiaoDich thành công.
    /// </summary>
    public class DsGiaoDich
    {
        private readonly string duongDanFile;

        private List<GiaoDich> danhSach;

        public DsGiaoDich(string duongDanFile = "giaodich.txt")
        {
            this.duongDanFile = duongDanFile;
            danhSach = new List<GiaoDich>();
            DocFile();
        }

        // ==================== ĐỌC / GHI FILE ====================

        /// <summary>
        /// Đọc toàn bộ giao dịch từ giaodich.txt vào bộ nhớ.
        /// Nếu file chưa tồn tại thì tự tạo file rỗng.
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
                GiaoDich gd = GiaoDich.TuDongFile(dong);
                if (gd != null)
                {
                    danhSach.Add(gd);
                }
            }
        }

        /// <summary>
        /// Ghi đè toàn bộ danh sách giao dịch hiện có xuống giaodich.txt.
        /// </summary>
        public void GhiFile()
        {
            List<string> cacDong = danhSach
                .Select(gd => gd.ToDongFile())
                .ToList();

            File.WriteAllLines(duongDanFile, cacDong, Encoding.UTF8);
        }

        // ==================== TRUY XUẤT / TÌM KIẾM ====================

        /// <summary>
        /// Lấy toàn bộ danh sách giao dịch.
        /// </summary>
        public List<GiaoDich> LayDanhSach()
        {
            return danhSach;
        }

        /// <summary>
        /// Lấy lịch sử giao dịch của 1 khách hàng theo số điện thoại,
        /// sắp xếp theo ngày mua gần nhất lên trước.
        /// Dùng cho MuaHang (xem lịch sử trong lúc mua) và LichSuGiaoDich.
        /// </summary>
        public List<GiaoDich> LayDanhSachTheoSoDienThoai(string soDienThoai)
        {
            if (string.IsNullOrWhiteSpace(soDienThoai))
                return new List<GiaoDich>();

            string sdt = soDienThoai.Trim();
            return danhSach
                .Where(gd => gd.SoDienThoai == sdt)
                .OrderByDescending(gd => gd.NgayMua)
                .ToList();
        }

        // ==================== THÊM GIAO DỊCH ====================

        /// <summary>
        /// Thêm 1 giao dịch mua hàng mới cho khách hàng theo số điện thoại.
        /// Ngày mua tự động lấy thời điểm hiện tại. Điểm nhận tự tính = 5% số tiền (làm tròn xuống).
        /// Trả về đối tượng GiaoDich vừa tạo (đã có DiemNhan) để nơi gọi cộng điểm
        /// vào hồ sơ khách hàng (dsKhachHang.CongDiem).
        /// Trả về null nếu dữ liệu đầu vào không hợp lệ (thiếu SĐT, nội dung rỗng, số tiền &lt;= 0).
        /// </summary>
        public GiaoDich ThemGiaoDich(string soDienThoai, string noiDung, decimal soTien, int soLuong)
        {
            return ThemGiaoDich(soDienThoai, noiDung, soTien, soLuong, null);
        }

        /// <summary>
        /// Thêm 1 giao dịch mua hàng mới, cho phép truyền sẵn số điểm nhận được
        /// (diemNhanTuyChinh) thay vì để hàm tự tính từ soTien.
        /// Dùng khi hóa đơn có áp dụng voucher giảm giá: soTien lưu vào giao dịch
        /// là số tiền THỰC THU (sau khi đã giảm), nhưng điểm tích lũy vẫn phải
        /// tính theo giá gốc TRƯỚC khi áp voucher — nơi gọi (MuaHang) tự tính điểm
        /// theo giá gốc rồi truyền vào đây.
        /// Nếu diemNhanTuyChinh = null, điểm sẽ được tự tính theo soTien như cũ.
        /// </summary>
        public GiaoDich ThemGiaoDich(string soDienThoai, string noiDung, decimal soTien, int soLuong, int? diemNhanTuyChinh)
        {
            if (string.IsNullOrWhiteSpace(soDienThoai) || string.IsNullOrWhiteSpace(noiDung) || soTien <= 0)
                return null;

            int diemNhan = diemNhanTuyChinh ?? GiaoDich.TinhDiem(soTien);

            GiaoDich giaoDichMoi = new GiaoDich(
                TaoMaGiaoDich(),
                soDienThoai.Trim(),
                DateTime.Now,
                noiDung.Trim(),
                soLuong,
                soTien,
                diemNhan);

            danhSach.Add(giaoDichMoi);
            GhiFile();
            return giaoDichMoi;
        }

        /// <summary>
        /// Sinh mã giao dịch mới, không trùng với các mã đã có.
        /// Định dạng: GD + số thứ tự tăng dần, ví dụ GD00001, GD00002...
        /// </summary>
        private string TaoMaGiaoDich()
        {
            int soThuTuLonNhat = 0;

            foreach (GiaoDich gd in danhSach)
            {
                if (gd.MaGiaoDich != null && gd.MaGiaoDich.StartsWith("GD"))
                {
                    string phanSo = gd.MaGiaoDich.Substring(2);
                    int so;
                    if (int.TryParse(phanSo, out so) && so > soThuTuLonNhat)
                    {
                        soThuTuLonNhat = so;
                    }
                }
            }

            return "GD" + (soThuTuLonNhat + 1).ToString("D5");
        }
    }
}
