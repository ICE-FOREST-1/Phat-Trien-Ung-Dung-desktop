using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Quản lý danh sách chi tiết từng dòng thuốc của tất cả các hóa đơn
    /// (đọc/ghi chitietgiaodich.txt). Mỗi hóa đơn (1 MaGiaoDich) có thể có
    /// nhiều dòng ChiTietGiaoDich tương ứng với từng loại thuốc đã mua.
    /// </summary>
    public class DsChiTietGiaoDich
    {
        private readonly string duongDanFile;
        private List<ChiTietGiaoDich> danhSach;

        public DsChiTietGiaoDich(string duongDanFile = "chitietgiaodich.txt")
        {
            this.duongDanFile = duongDanFile;
            danhSach = new List<ChiTietGiaoDich>();
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
                ChiTietGiaoDich ct = ChiTietGiaoDich.TuDongFile(dong);
                if (ct != null)
                {
                    danhSach.Add(ct);
                }
            }
        }

        public void GhiFile()
        {
            List<string> cacDong = danhSach
                .Select(ct => ct.ToDongFile())
                .ToList();

            File.WriteAllLines(duongDanFile, cacDong, Encoding.UTF8);
        }

        /// <summary>
        /// Thêm nhiều dòng chi tiết (thuộc cùng 1 hóa đơn) vào danh sách và ghi file.
        /// </summary>
        public void ThemNhieu(IEnumerable<ChiTietGiaoDich> cacDongMoi)
        {
            danhSach.AddRange(cacDongMoi);
            GhiFile();
        }

        /// <summary>
        /// Lấy toàn bộ dòng chi tiết thuộc 1 mã giao dịch (giữ đúng thứ tự đã thêm).
        /// </summary>
        public List<ChiTietGiaoDich> LayTheoMaGiaoDich(string maGiaoDich)
        {
            if (string.IsNullOrWhiteSpace(maGiaoDich))
                return new List<ChiTietGiaoDich>();

            string ma = maGiaoDich.Trim();
            return danhSach.Where(ct => ct.MaGiaoDich == ma).ToList();
        }
    }
}
