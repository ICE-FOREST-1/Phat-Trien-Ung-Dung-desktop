using System;
using System.Globalization;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Đại diện cho một giao dịch mua hàng của khách hàng.
    /// Khóa chính là MaGiaoDich; liên kết tới khách hàng qua SoDienThoai.
    /// </summary>
    public class GiaoDich
    {
        /// <summary>
        /// Định dạng ngày giờ dùng thống nhất khi đọc/ghi giaodich.txt.
        /// </summary>
        public const string DINH_DANG_NGAY = "dd/MM/yyyy HH:mm:ss";

        /// <summary>
        /// Tỉ lệ điểm tích lũy = 10% tổng giá trị hóa đơn (làm tròn xuống).
        /// </summary>
        public const decimal TI_LE_DIEM = 0.1m / 1000;

        public string MaGiaoDich { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime NgayMua { get; set; }
        public string NoiDung { get; set; }
        public int SoLuong { get; set; }
        public decimal SoTien { get; set; }
        public int DiemNhan { get; set; }
        public GiaoDich()
        {
        }

        public GiaoDich(string maGiaoDich, string soDienThoai, DateTime ngayMua, string noiDung, int soLuong, decimal soTien, int diemNhan)
        {
            MaGiaoDich = maGiaoDich;
            SoDienThoai = soDienThoai;
            NgayMua = ngayMua;
            NoiDung = noiDung;
            SoLuong = soLuong;
            SoTien = soTien;
            DiemNhan = diemNhan;
            
        }

        /// <summary>
        /// Tính điểm nhận được từ 1 số tiền hóa đơn, theo tỉ lệ 5% (làm tròn xuống).
        /// </summary>
        public static int TinhDiem(decimal soTien)
        {
            return (int)Math.Floor(soTien * TI_LE_DIEM);
        }

        /// <summary>
        /// Chuyển giao dịch thành 1 dòng text để lưu vào giaodich.txt.
        /// Định dạng: MaGiaoDich|SoDienThoai|NgayMua|NoiDung|SoTien|DiemNhan
        /// </summary>
        public string ToDongFile()
        {
            return string.Join("|",
                MaGiaoDich,
                SoDienThoai,
                NgayMua.ToString(DINH_DANG_NGAY, CultureInfo.InvariantCulture),
                NoiDung,
                SoLuong.ToString(CultureInfo.InvariantCulture),
                SoTien.ToString(CultureInfo.InvariantCulture),
                DiemNhan.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Phân tích 1 dòng text đọc từ giaodich.txt thành đối tượng GiaoDich.
        /// Trả về null nếu dòng rỗng hoặc sai định dạng.
        /// </summary>
        public static GiaoDich TuDongFile(string dong)
        {
            if (string.IsNullOrWhiteSpace(dong))
                return null;

            string[] cot = dong.Split('|');
            if (cot.Length < 6)
                return null;

            string maGiaoDich = cot[0].Trim();
            string soDienThoai = cot[1].Trim();

            if (string.IsNullOrEmpty(maGiaoDich) || string.IsNullOrEmpty(soDienThoai))
                return null;

            DateTime ngayMua;
            if (!DateTime.TryParseExact(
                    cot[2].Trim(),
                    DINH_DANG_NGAY,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out ngayMua))
            {
                ngayMua = DateTime.Now;
            }

            string noiDung = cot[3].Trim();

            decimal soTien;
            decimal.TryParse(cot[4].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out soTien);

            int soLuong;
            int.TryParse(cot[6].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out soLuong);

            int diemNhan;
            int.TryParse(cot[5].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out diemNhan);

          
            return new GiaoDich(maGiaoDich, soDienThoai, ngayMua, noiDung, soLuong, soTien, diemNhan);
        }
    }
}
