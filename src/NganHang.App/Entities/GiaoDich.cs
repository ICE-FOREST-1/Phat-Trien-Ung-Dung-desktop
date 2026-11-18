namespace NganHang.Core.Entities;

/// <summary>Bảng dbo.GiaoDich – nhật ký bút toán (bảng có trigger AML & thăng hạng).</summary>
public class GiaoDich
{
    public long MaGiaoDich { get; set; }
    public string MaChungTu { get; set; } = string.Empty;
    public string SoTaiKhoanNguon { get; set; } = string.Empty;
    public string TenNguoiGui { get; set; } = string.Empty;
    public string SoTaiKhoanDich { get; set; } = string.Empty;
    public string TenNguoiNhan { get; set; } = string.Empty;
    public string? NganHangDich { get; set; }
    public string LoaiGiaoDich { get; set; } = string.Empty;
    public decimal SoTienGiaoDich { get; set; }
    public decimal? PhiGiaoDich { get; set; } = 0;
    public decimal SoDuTruocGD { get; set; }
    public decimal SoDuSauGD { get; set; }
    public string NoiDungChuyenKhoan { get; set; } = string.Empty;
    public DateTime? ThoiGianGiaoDich { get; set; }
    public string? TrangThaiGiaoDich { get; set; } = Constants.TrangThaiGiaoDich.ThanhCong;
    public string NhanVienThucHien { get; set; } = string.Empty;
    public bool? CanhBaoRuaTienAML { get; set; } = false;
    public string? LyDoCanhBaoAML { get; set; }
}
