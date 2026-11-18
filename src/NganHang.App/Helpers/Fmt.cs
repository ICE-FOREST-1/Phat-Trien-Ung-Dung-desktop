using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using NganHang.Core.Constants;

namespace NganHang.App.Helpers;

/// <summary>
/// Hàm định dạng dùng trực tiếp trong XAML qua x:Bind, ví dụ: Text="{x:Bind h:Fmt.Tien(SoDuHienTai)}".
/// Thay cho việc viết nhiều Converter.
/// </summary>
public static class Fmt
{
    // ---- Tiền tệ ----
    public static string Tien(decimal v) => $"{v:N0} đ";
    public static string TienN(decimal? v) => $"{v ?? 0:N0} đ";
    public static string TienRutGon(decimal v) => v switch
    {
        >= 1_000_000_000 => $"{v / 1_000_000_000m:0.##} tỷ",
        >= 1_000_000 => $"{v / 1_000_000m:0.##} triệu",
        _ => $"{v:N0} đ",
    };
    public static string PhanTram(decimal v) => $"{v:0.00}%";

    // ---- Ngày giờ ----
    public static string Ngay(DateTime v) => v.ToString("dd/MM/yyyy");
    public static string NgayN(DateTime? v) => v?.ToString("dd/MM/yyyy") ?? "—";
    public static string NgayGio(DateTime? v) => v?.ToString("dd/MM/yyyy HH:mm") ?? "—";

    // ---- Danh mục ----
    public static string Ma(string? ma) => HienThi.Ma(ma);
    public static string So(int? v) => v?.ToString() ?? "—";
    public static string Chu(string? v) => string.IsNullOrWhiteSpace(v) ? "—" : v;
    public static string ChuCaiDau(string? hoTen)
    {
        if (string.IsNullOrWhiteSpace(hoTen)) return "?";
        var tu = hoTen.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return tu.Length == 1 ? tu[0][..1].ToUpper() : $"{tu[0][0]}{tu[^1][0]}".ToUpper();
    }

    // ---- Hiển thị ----
    public static Visibility Hien(bool v) => v ? Visibility.Visible : Visibility.Collapsed;
    public static Visibility An(bool v) => v ? Visibility.Collapsed : Visibility.Visible;
    public static Visibility HienNeuCo(object? v) => v is null ? Visibility.Collapsed : Visibility.Visible;
    public static Visibility AnNeuCo(object? v) => v is null ? Visibility.Visible : Visibility.Collapsed;
    public static Visibility HienNeuBang(string? a, string b) => a == b ? Visibility.Visible : Visibility.Collapsed;
    public static bool Khong(bool v) => !v;

    // ---- Màu nhãn trạng thái (tông trung tính, đọc được trên cả nền sáng và tối) ----
    private static readonly SolidColorBrush XanhLa = new(ColorHelper.FromArgb(255, 0x10, 0xB9, 0x81));
    private static readonly SolidColorBrush Vang = new(ColorHelper.FromArgb(255, 0xF5, 0x9E, 0x0B));
    private static readonly SolidColorBrush Do = new(ColorHelper.FromArgb(255, 0xEF, 0x44, 0x44));
    private static readonly SolidColorBrush XanhDuong = new(ColorHelper.FromArgb(255, 0x3B, 0x82, 0xF6));
    private static readonly SolidColorBrush Xam = new(ColorHelper.FromArgb(255, 0x94, 0xA3, 0xB8));

    public static Brush MauTrangThai(string? ma) => ma switch
    {
        TrangThaiTaiKhoan.HoatDong or TrangThaiGiaoDich.ThanhCong or TrangThaiKhoanVay.DangVay or TrangThaiKhoanVay.DaDuyet
            or TrangThaiAml.DaDuyetAnToan or TrangThaiSoTietKiem.DangHoatDong or MucDoRuiRo.Thap => XanhLa,
        TrangThaiKhoanVay.ChoPheDuyet or TrangThaiAml.ChoKiemDuyet or TrangThaiGiaoDich.DangXuLy or MucDoRuiRo.TrungBinh => Vang,
        TrangThaiTaiKhoan.TamKhoa or TrangThaiKhoanVay.TuChoi or TrangThaiKhoanVay.QuaHanNoXau or TrangThaiAml.KhoaDongBang
            or TrangThaiGiaoDich.ThatBai or MucDoRuiRo.Cao or MucDoRuiRo.DacBietNghiemTrong => Do,
        TrangThaiSoTietKiem.DaTatToan or TrangThaiKhoanVay.DaTatToan or TrangThaiTaiKhoan.DongSo => Xam,
        _ => XanhDuong,
    };

    private static readonly SolidColorBrush Bac = new(ColorHelper.FromArgb(255, 0x94, 0xA3, 0xB8));
    private static readonly SolidColorBrush VangKim = new(ColorHelper.FromArgb(255, 0xEA, 0xB3, 0x08));
    private static readonly SolidColorBrush BachKim = new(ColorHelper.FromArgb(255, 0x8B, 0x5C, 0xF6));
    private static readonly SolidColorBrush KimCuong = new(ColorHelper.FromArgb(255, 0x06, 0xB6, 0xD4));

    public static Brush MauHang(string? hang) => hang switch
    {
        HangHoiVien.Bac => Bac,
        HangHoiVien.Vang => VangKim,
        HangHoiVien.BachKim => BachKim,
        HangHoiVien.KimCuong => KimCuong,
        _ => Xam,
    };
}
