using System.Globalization;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Đại diện cho MỘT dòng thuốc trong một hóa đơn (giao dịch).
    /// Nhiều ChiTietGiaoDich có thể cùng chung 1 MaGiaoDich (khi khách mua
    /// nhiều loại thuốc trong 1 lần thanh toán) — KHÔNG được sinh mã giao dịch
    /// khác nhau cho từng dòng thuốc trong cùng 1 lần thanh toán.
    /// </summary>
    public class ChiTietGiaoDich
    {
        public string MaGiaoDich { get; set; }
        public string TenThuoc { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }

        public ChiTietGiaoDich()
        {
        }

        public ChiTietGiaoDich(string maGiaoDich, string tenThuoc, int soLuong, decimal donGia, decimal thanhTien)
        {
            MaGiaoDich = maGiaoDich;
            TenThuoc = tenThuoc;
            SoLuong = soLuong;
            DonGia = donGia;
            ThanhTien = thanhTien;
        }

        /// <summary>
        /// Định dạng: MaGiaoDich|TenThuoc|SoLuong|DonGia|ThanhTien
        /// </summary>
        public string ToDongFile()
        {
            return string.Join("|",
                MaGiaoDich,
                TenThuoc,
                SoLuong.ToString(CultureInfo.InvariantCulture),
                DonGia.ToString(CultureInfo.InvariantCulture),
                ThanhTien.ToString(CultureInfo.InvariantCulture));
        }

        public static ChiTietGiaoDich TuDongFile(string dong)
        {
            if (string.IsNullOrWhiteSpace(dong))
                return null;

            string[] cot = dong.Split('|');
            if (cot.Length < 5)
                return null;

            string maGiaoDich = cot[0].Trim();
            string tenThuoc = cot[1].Trim();

            if (string.IsNullOrEmpty(maGiaoDich) || string.IsNullOrEmpty(tenThuoc))
                return null;

            int soLuong;
            int.TryParse(cot[2].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out soLuong);

            decimal donGia;
            decimal.TryParse(cot[3].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out donGia);

            decimal thanhTien;
            decimal.TryParse(cot[4].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out thanhTien);

            return new ChiTietGiaoDich(maGiaoDich, tenThuoc, soLuong, donGia, thanhTien);
        }
    }
}
