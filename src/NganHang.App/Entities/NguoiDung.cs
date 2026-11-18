namespace NganHang.Core.Entities;

/// <summary>Bảng dbo.NguoiDung – tài khoản nhân viên đăng nhập hệ thống.</summary>
public class NguoiDung
{
    public int MaNguoiDung { get; set; }
    public string TenDangNhap { get; set; } = string.Empty;
    public string MatKhauMaHoa { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public string VaiTro { get; set; } = string.Empty;
    public string? ChiNhanh { get; set; }
    public bool? TrangThaiKichHoat { get; set; } = true;
    public DateTime? NgayTao { get; set; }
    public DateTime? DangNhapLanCuoi { get; set; }
}
