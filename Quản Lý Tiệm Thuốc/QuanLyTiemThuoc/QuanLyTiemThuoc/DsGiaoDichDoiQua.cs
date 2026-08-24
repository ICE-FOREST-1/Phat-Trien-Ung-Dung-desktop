using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Quản lý lịch sử đổi quà của toàn bộ khách hàng (đọc/ghi lichsudoiqua.txt).
    /// Mỗi lần khách đổi quà thành công ở form DoiQua sẽ được ghi lại 1 dòng tại đây,
    /// dùng để hiển thị ở form LichSuDoiQua.
    /// </summary>
    public class DsGiaoDichDoiQua
    {
        private readonly string duongDanFile;
        private List<GiaoDichDoiQua> danhSach;

        public DsGiaoDichDoiQua(string duongDanFile = "lichsudoiqua.txt")
        {
            this.duongDanFile = duongDanFile;
            danhSach = new List<GiaoDichDoiQua>();
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
                GiaoDichDoiQua gd = GiaoDichDoiQua.TuDongFile(dong);
                if (gd != null)
                {
                    danhSach.Add(gd);
                }
            }
        }

        public void GhiFile()
        {
            List<string> cacDong = danhSach.Select(gd => gd.ToDongFile()).ToList();
            File.WriteAllLines(duongDanFile, cacDong, Encoding.UTF8);
        }

        public List<GiaoDichDoiQua> LayDanhSachTheoSoDienThoai(string soDienThoai)
        {
            if (string.IsNullOrWhiteSpace(soDienThoai))
                return new List<GiaoDichDoiQua>();

            string sdt = soDienThoai.Trim();
            return danhSach
                .Where(gd => gd.SoDienThoai == sdt)
                .OrderByDescending(gd => gd.ThoiGian)
                .ToList();
        }

        /// <summary>
        /// Ghi nhận 1 lần đổi quà mới thành công.
        /// </summary>
        public GiaoDichDoiQua ThemGiaoDich(string soDienThoai, string tenQua, int chiPhi, string trangThai = "Đã đổi")
        {
            if (string.IsNullOrWhiteSpace(soDienThoai) || string.IsNullOrWhiteSpace(tenQua))
                return null;

            GiaoDichDoiQua giaoDichMoi = new GiaoDichDoiQua(
                TaoMaDoiQua(), soDienThoai.Trim(), tenQua.Trim(), DateTime.Now, chiPhi, trangThai);

            danhSach.Add(giaoDichMoi);
            GhiFile();
            return giaoDichMoi;
        }

        /// <summary>
        /// Sinh mã đổi quà mới, không trùng: DQ00001, DQ00002...
        /// </summary>
        private string TaoMaDoiQua()
        {
            int soThuTuLonNhat = 0;

            foreach (GiaoDichDoiQua gd in danhSach)
            {
                if (gd.MaDoiQua != null && gd.MaDoiQua.StartsWith("DQ"))
                {
                    string phanSo = gd.MaDoiQua.Substring(2);
                    int so;
                    if (int.TryParse(phanSo, out so) && so > soThuTuLonNhat)
                    {
                        soThuTuLonNhat = so;
                    }
                }
            }

            return "DQ" + (soThuTuLonNhat + 1).ToString("D5");
        }
    }
}
