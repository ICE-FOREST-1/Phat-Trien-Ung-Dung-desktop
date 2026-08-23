using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Quản lý danh sách phần quà đổi điểm (đọc/ghi qua.txt).
    /// Nếu file chưa tồn tại, tự tạo sẵn vài phần quà mẫu rải theo nhiều tháng
    /// khác nhau để có dữ liệu dùng thử ngay.
    /// </summary>
    public class DsQua
    {
        private readonly string duongDanFile;
        private List<PhanQua> danhSach;

        public DsQua(string duongDanFile = "qua.txt")
        {
            this.duongDanFile = duongDanFile;
            danhSach = new List<PhanQua>();
            DocFile();
        }

        public void DocFile()
        {
            danhSach.Clear();

            if (!File.Exists(duongDanFile))
            {
                TaoFileMauMacDinh();
            }

            string[] cacDong = File.ReadAllLines(duongDanFile, Encoding.UTF8);
            foreach (string dong in cacDong)
            {
                PhanQua qua = PhanQua.TuDongFile(dong);
                if (qua != null)
                {
                    danhSach.Add(qua);
                }
            }
        }

        /// <summary>
        /// Tạo sẵn vài phần quà mẫu, rải ra các tháng khác nhau trong năm,
        /// để cửa hàng có dữ liệu dùng thử. Có thể chỉnh/xóa qua form Quản Lý Quà
        /// hoặc sửa trực tiếp trong qua.txt.
        /// </summary>
        private void TaoFileMauMacDinh()
        {
            List<string> mau = new List<string>
            {
                new PhanQua("Ly giữ nhiệt Tiệm Thuốc", 50, new List<int>{1,2,3,4,5,6,7,8,9,10,11,12}).ToDongFile(),
                new PhanQua("Túi vải đựng thuốc", 80, new List<int>{1,2,3,4,5,6,7,8,9,10,11,12}).ToDongFile(),
                new PhanQua("Hộp đựng thuốc cá nhân", 12, new List<int>{1,2,3,4,5,6,7,8,9,10,11,12}).ToDongFile(),
                new PhanQua("Phiếu giảm giá 20.000đ", 30, new List<int>{1,2,3,4,5,6,7,8,9,10,11,12}, "GIAM20K").ToDongFile(),
                new PhanQua("Nhiệt kế điện tử", 20, new List<int>{1,2,3,4,5,6,7,8,9,10,11,12}).ToDongFile(),
                new PhanQua("Phiếu giảm giá 50.000đ", 30, new List<int>{1,2,3,4,5,6,7,8,9,10,11,12}, "GIAM50K").ToDongFile(),
                new PhanQua("Máy đo huyết áp mini", 80, new List<int>{1,2,3,4,5,6,7,8,9,10,11,12}).ToDongFile(),
                new PhanQua("Quà Trung Thu - Hộp bánh mini", 40, new List<int>{9}).ToDongFile(),
                new PhanQua("Quà tháng 10 - Bình giữ nhiệt mini", 45, new List<int>{10}).ToDongFile(),
            };
            File.WriteAllLines(duongDanFile, mau, Encoding.UTF8);
        }

        public void GhiFile()
        {
            List<string> cacDong = danhSach
                .Select(q => q.ToDongFile())
                .ToList();

            File.WriteAllLines(duongDanFile, cacDong, Encoding.UTF8);
        }

        public List<PhanQua> LayDanhSach()
        {
            return danhSach;
        }

        /// <summary>
        /// Lấy danh sách quà đang áp dụng cho 1 tháng cụ thể (VD tháng hiện tại).
        /// </summary>
        public List<PhanQua> LayTheoThang(int thang)
        {
            return danhSach.Where(q => q.ApDungChoThang(thang)).ToList();
        }

        public void ThemQua(PhanQua quaMoi)
        {
            danhSach.Add(quaMoi);
            GhiFile();
        }

        public bool XoaQua(PhanQua qua)
        {
            bool ketQua = danhSach.Remove(qua);
            if (ketQua) GhiFile();
            return ketQua;
        }

        /// <summary>
        /// Điểm thấp nhất cần để đổi được BẤT KỲ phần quà nào trong toàn bộ
        /// danh sách (không phân biệt tháng) — dùng để cảnh báo trước khi reset điểm.
        /// Trả về null nếu danh sách quà đang trống.
        /// </summary>
        public int? DiemThapNhatDeCoTheDoiQua()
        {
            if (danhSach.Count == 0) return null;
            return danhSach.Min(q => q.DiemCanDoi);
        }
    }
}
