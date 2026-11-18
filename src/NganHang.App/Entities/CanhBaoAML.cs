namespace NganHang.Core.Entities;

/// <summary>Bảng dbo.CanhBaoAML – cảnh báo rửa tiền (do trigger trg_PhatHienGiaoDichDangNgoAML sinh ra).</summary>
public class CanhBaoAML
{
    public int MaCanhBao { get; set; }
    public long MaGiaoDichThamChieu { get; set; }
    public string SoTaiKhoanPhatSinh { get; set; } = string.Empty;
    public string TenKhachHang { get; set; } = string.Empty;
    public decimal SoTienPhatSinh { get; set; }
    public string? MucDoRuiRo { get; set; }
    public string LyDoTriggerPhatHien { get; set; } = string.Empty;
    public DateTime? ThoiGianPhatHien { get; set; }
    public string? TrangThaiXuLy { get; set; } = Constants.TrangThaiAml.ChoKiemDuyet;
    public string? NguoiKiemDuyet { get; set; }
    public DateTime? ThoiGianKiemDuyet { get; set; }

    public GiaoDich? GiaoDich { get; set; }
}
