namespace NganHang.Core.Entities;

/// <summary>Bảng dbo.TaiSanDamBao – tài sản thế chấp / cầm cố cho khoản vay.</summary>
public class TaiSanDamBao
{
    public int MaTaiSan { get; set; }
    public string LoaiTaiSan { get; set; } = string.Empty;
    public string TenTaiSan { get; set; } = string.Empty;
    public decimal GiaTriDinhGia { get; set; }
    public string SoGiayToPhapLy { get; set; } = string.Empty;
    public int? MaSoTietKiemThamChieu { get; set; }
    public string? TrangThaiTheChap { get; set; } = "DANG_CAM_CO";
}
