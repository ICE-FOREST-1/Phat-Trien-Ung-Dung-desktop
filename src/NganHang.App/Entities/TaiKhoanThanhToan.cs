using CommunityToolkit.Mvvm.ComponentModel;

namespace NganHang.Core.Entities;

/// <summary>Bảng dbo.TaiKhoanThanhToan – tài khoản thanh toán của khách hàng.</summary>
public partial class TaiKhoanThanhToan : ObservableObject
{
    public int MaTaiKhoan { get; set; }
    public string SoTaiKhoan { get; set; } = string.Empty;
    public int MaKhachHang { get; set; }
    public string LoaiTaiKhoan { get; set; } = "THANH_TOAN";
    public decimal SoDuHienTai { get; set; }
    public string? DonViTienTe { get; set; } = "VND";
    [ObservableProperty] private string? trangThaiTaiKhoan = Constants.TrangThaiTaiKhoan.HoatDong;
    public string? HangTaiKhoan { get; set; } = Constants.HangHoiVien.Chuan;
    public decimal? LaiSuatKhongKyHan { get; set; }
    public bool? ChoPhepThauChi { get; set; }
    public decimal? HanMucThauChi { get; set; }
    public DateTime? NgayMoTaiKhoan { get; set; }
    public DateTime? NgayDongTaiKhoan { get; set; }

    public KhachHang? KhachHang { get; set; }
}
