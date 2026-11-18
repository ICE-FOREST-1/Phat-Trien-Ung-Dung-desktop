namespace NganHang.Core.Entities;

/// <summary>Bảng dbo.KhoanVay – hợp đồng tín dụng.</summary>
public class KhoanVay
{
    public int MaKhoanVay { get; set; }
    public string MaSoHopDongVay { get; set; } = string.Empty;
    public int MaKhachHang { get; set; }
    public int MaTaiKhoanGiaiNgan { get; set; }
    public int? MaTaiSanDamBao { get; set; }
    public string GoiVay { get; set; } = string.Empty;
    public decimal SoTienVayBanDau { get; set; }
    public decimal DuNoGocHienTai { get; set; }
    public int ThoiHanVayThang { get; set; }
    public decimal LaiSuatVayNam { get; set; }
    public DateTime NgayGiaiNgan { get; set; }
    public DateTime NgayDaoHanHopDong { get; set; }
    public DateTime NgayTraNoTiepTheo { get; set; }
    public decimal SoTienPhaiTraHangThang { get; set; }
    public string? TrangThaiKhoanVay { get; set; } = Constants.TrangThaiKhoanVay.ChoPheDuyet;
    public int? SoNgayQuaHan { get; set; } = 0;
    public string MucDichVay { get; set; } = string.Empty;
    public int NhanVienLapHoSo { get; set; }
    public int? LanhDaoPheDuyet { get; set; }
    public DateTime? NgayPheDuyet { get; set; }
    public string? LyDoTuChoi { get; set; }
    public DateTime? NgayTao { get; set; }

    public KhachHang? KhachHang { get; set; }
    public TaiKhoanThanhToan? TaiKhoanGiaiNgan { get; set; }
    public TaiSanDamBao? TaiSanDamBao { get; set; }
}
