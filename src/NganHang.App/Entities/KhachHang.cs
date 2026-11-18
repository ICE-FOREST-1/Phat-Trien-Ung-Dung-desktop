namespace NganHang.Core.Entities;

/// <summary>Bảng dbo.KhachHang – hồ sơ khách hàng (CIF).</summary>
public class KhachHang
{
    public int MaKhachHang { get; set; }
    public string MaCIF { get; set; } = string.Empty;
    public string HoTenKhachHang { get; set; } = string.Empty;
    public string SoCCCD { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string DiaChiThuongTru { get; set; } = string.Empty;
    public DateTime NgaySinh { get; set; } = new(1990, 1, 1);
    public string? GioiTinh { get; set; }
    public string? NgheNghiep { get; set; }
    public decimal? ThuNhapHangThang { get; set; }
    public int? DiemTinDung { get; set; } = 700;
    public string? HangHoiVien { get; set; } = Constants.HangHoiVien.Chuan;
    public decimal? DiemTichLuyChiTieu { get; set; } = 0;
    public DateTime? NgayGiaNhap { get; set; }

    public List<TaiKhoanThanhToan> TaiKhoans { get; set; } = [];
    public List<SoTietKiem> SoTietKiems { get; set; } = [];
    public List<KhoanVay> KhoanVays { get; set; } = [];
}
