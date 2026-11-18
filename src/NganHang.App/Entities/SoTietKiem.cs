namespace NganHang.Core.Entities;

/// <summary>Bảng dbo.SoTietKiem – sổ tiết kiệm có kỳ hạn.</summary>
public class SoTietKiem
{
    public int MaSoTietKiem { get; set; }
    public string MaSoSo { get; set; } = string.Empty;
    public int? MaTaiKhoanNguon { get; set; }
    public int MaKhachHang { get; set; }
    public decimal SoTienGocBanDau { get; set; }
    public decimal SoTienHienTai { get; set; }
    public int KyHanThang { get; set; }
    public decimal LaiSuatNam { get; set; }
    public string? HinhThucNhanLai { get; set; } = "CUOI_KY";
    public DateTime NgayGui { get; set; }
    public DateTime NgayDaoHan { get; set; }
    public bool? TuDongTaiTuc { get; set; } = true;
    public bool? DangTheChap { get; set; } = false;
    public string? MaKhoanVayLienKet { get; set; }
    public string? TrangThaiSo { get; set; } = Constants.TrangThaiSoTietKiem.DangHoatDong;
    public decimal? LaiTichLuyTamTinh { get; set; } = 0;
    public DateTime? NgayTao { get; set; }

    public KhachHang? KhachHang { get; set; }
    public TaiKhoanThanhToan? TaiKhoanNguon { get; set; }
}
