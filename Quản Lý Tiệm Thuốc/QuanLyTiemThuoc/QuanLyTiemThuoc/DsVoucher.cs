using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Quản lý danh sách voucher giảm giá dùng chung cho cửa hàng (đọc/ghi voucher.txt).
    /// Nếu file chưa tồn tại, tự tạo sẵn vài voucher mẫu để có dữ liệu dùng thử.
    /// </summary>
    public class DsVoucher
    {
        private readonly string duongDanFile;
        private List<Voucher> danhSach;

        public DsVoucher(string duongDanFile = "voucher.txt")
        {
            this.duongDanFile = duongDanFile;
            danhSach = new List<Voucher>();
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
                Voucher vc = Voucher.TuDongFile(dong);
                if (vc != null)
                {
                    danhSach.Add(vc);
                }
            }
        }

        /// <summary>
        /// Tạo sẵn vài voucher mẫu khi file chưa tồn tại, để cửa hàng có dữ liệu
        /// dùng thử ngay. Có thể xóa/sửa trực tiếp trong voucher.txt sau đó.
        /// </summary>
        private void TaoFileMauMacDinh()
        {
            List<string> mau = new List<string>
            {
                new Voucher("GIAM10", LoaiVoucher.PhanTram, 10m).ToDongFile(),
                new Voucher("GIAM20", LoaiVoucher.PhanTram, 20m).ToDongFile(),
                new Voucher("GIAM20K", LoaiVoucher.SoTienCoDinh, 20000m).ToDongFile(),
                new Voucher("GIAM50K", LoaiVoucher.SoTienCoDinh, 50000m).ToDongFile(),
            };
            File.WriteAllLines(duongDanFile, mau, Encoding.UTF8);
        }

        public void GhiFile()
        {
            List<string> cacDong = danhSach
                .Select(vc => vc.ToDongFile())
                .ToList();

            File.WriteAllLines(duongDanFile, cacDong, Encoding.UTF8);
        }

        public List<Voucher> LayDanhSach()
        {
            return danhSach;
        }

        public Voucher TimTheoMa(string maVoucher)
        {
            if (string.IsNullOrWhiteSpace(maVoucher))
                return null;

            string ma = maVoucher.Trim();
            return danhSach.FirstOrDefault(vc => vc.MaVoucher == ma);
        }
    }
}
