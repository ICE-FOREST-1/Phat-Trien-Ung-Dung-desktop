using System.Collections.Generic;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Danh sách thuốc mẫu, mỗi loại thuốc kèm liều lượng cụ thể (để phân biệt
    /// các thuốc cùng công dụng nhưng khác liều) và đơn giá tham khảo.
    /// Dùng để đổ lên cbChon (form MuaHang) và tự động điền giá khi chọn.
    ///
    /// LƯU Ý: Giá ở đây là giá MẪU do lập trình tự đặt. Nhân viên vẫn có thể
    /// sửa lại giá thực tế trên ô "Đơn Giá" trước khi bấm "Thêm".
    /// Muốn thêm/sửa/xóa thuốc: chỉnh trực tiếp danh sách DANH_SACH bên dưới.
    /// </summary>
    public static class ThuocInfo
    {
        /// <summary>
        /// Key = tên thuốc kèm liều lượng (hiển thị trong cbChon).
        /// Value = đơn giá mẫu (VNĐ / vỉ hoặc hộp).
        /// </summary>
        public static readonly Dictionary<string, decimal> DANH_SACH = new Dictionary<string, decimal>
        {
            // Giảm đau - hạ sốt
            { "Paracetamol 500mg (Panadol, Hapacol)", 15000m },
            { "Paracetamol 650mg (Panadol Extra)", 18000m },
            { "Ibuprofen 400mg", 20000m },
            { "Ibuprofen 600mg", 25000m },
            { "Aspirin 81mg", 10000m },
            { "Aspirin 500mg", 12000m },
            { "Cataflam 25mg (Diclofenac)", 13000m },
            { "Cataflam 50mg (Diclofenac)", 15000m },
            { "Alpha Choay 4.2mg", 40000m },

            // Kháng sinh
            { "Amoxicillin 250mg", 18000m },
            { "Amoxicillin 500mg", 25000m },
            { "Augmentin 625mg (Amoxicillin + Clavulanate)", 45000m },
            { "Augmentin 1g (Amoxicillin + Clavulanate)", 55000m },
            { "Cefixim 100mg", 30000m },
            { "Cefixim 200mg", 45000m },
            { "Azithromycin 250mg", 35000m },
            { "Azithromycin 500mg", 50000m },

            // Vitamin - bổ trợ
            { "Vitamin C 500mg (Efferalgan C)", 20000m },
            { "Vitamin C 1000mg (C sủi)", 25000m },
            { "Hoạt huyết dưỡng não 40mg (Ginkgo Biloba)", 60000m },
            { "Hoạt huyết dưỡng não 80mg (Ginkgo Biloba)", 80000m },
        };

        /// <summary>
        /// Lấy danh sách tên thuốc (dùng đổ vào cbChon.Items).
        /// </summary>
        public static string[] LayDanhSachTen()
        {
            string[] ten = new string[DANH_SACH.Count];
            DANH_SACH.Keys.CopyTo(ten, 0);
            return ten;
        }

        /// <summary>
        /// Tìm giá mẫu theo tên thuốc (khớp chính xác). Trả về null nếu không có
        /// trong danh sách (khi đó nhân viên phải tự nhập giá).
        /// </summary>
        public static decimal? TimGia(string tenThuoc)
        {
            if (string.IsNullOrWhiteSpace(tenThuoc))
                return null;

            decimal gia;
            if (DANH_SACH.TryGetValue(tenThuoc.Trim(), out gia))
                return gia;

            return null;
        }
    }
}
