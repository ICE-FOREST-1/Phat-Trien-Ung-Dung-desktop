using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using NganHang.App.Views;
using NganHang.Core.Services;

namespace NganHang.App.Services;

/// <summary>Khóa định danh các trang trong menu (dùng làm Tag của NavigationViewItem).</summary>
public static class TrangDich
{
    public const string TongQuan = "TongQuan";
    public const string KhachHang = "KhachHang";
    public const string TaiKhoan = "TaiKhoan";
    public const string TietKiem = "TietKiem";
    public const string TinDung = "TinDung";
    public const string ChuyenKhoan = "ChuyenKhoan";
    public const string NhatKy = "NhatKy";
    public const string Aml = "Aml";
    public const string HangHoiVien = "HangHoiVien";
    public const string CaiDat = "CaiDat";
}

/// <summary>
/// Điều hướng 2 cấp: khung gốc (Đăng nhập ↔ Shell) và khung nội dung bên trong Shell.
/// Thêm trang mới: tạo Page + ViewModel, rồi khai báo một dòng trong <see cref="BangTrang"/>.
/// </summary>
public class NavigationService(PhienLamViec phien)
{
    private static readonly Dictionary<string, Type> BangTrang = new()
    {
        [TrangDich.TongQuan] = typeof(DashboardPage),
        [TrangDich.KhachHang] = typeof(CustomersPage),
        [TrangDich.TaiKhoan] = typeof(AccountsPage),
        [TrangDich.TietKiem] = typeof(SavingsPage),
        [TrangDich.TinDung] = typeof(LoansPage),
        [TrangDich.ChuyenKhoan] = typeof(TransferPage),
        [TrangDich.NhatKy] = typeof(HistoryPage),
        [TrangDich.Aml] = typeof(AmlPage),
        [TrangDich.HangHoiVien] = typeof(TiersPage),
        [TrangDich.CaiDat] = typeof(SettingsPage),
    };

    private Frame? _khungGoc;
    private Frame? _khungNoiDung;

    public string? TrangHienTai { get; private set; }

    /// <summary>Phát sinh khi trang nội dung thay đổi (Shell dùng để đồng bộ mục menu đang chọn).</summary>
    public event EventHandler<string>? DaDieuHuong;

    public void KhoiTaoKhungGoc(Frame frame) => _khungGoc = frame;
    public void KhoiTaoKhungNoiDung(Frame frame) => _khungNoiDung = frame;

    public void VeManHinhDangNhap()
    {
        _khungNoiDung = null;
        TrangHienTai = null;
        _khungGoc?.Navigate(typeof(LoginPage), null, new SuppressNavigationTransitionInfo());
        _khungGoc?.BackStack.Clear();
    }

    public void VaoHeThong()
    {
        _khungGoc?.Navigate(typeof(ShellPage), null, new SuppressNavigationTransitionInfo());
        _khungGoc?.BackStack.Clear();
    }

    public void DangXuat()
    {
        phien.KetThuc();
        VeManHinhDangNhap();
    }

    public bool DieuHuong(string trang, object? thamSo = null)
    {
        if (trang == TrangDich.TongQuan && !phien.CoQuyen(NganHang.Core.Security.ChucNang.TongQuan)) return false;
        if (trang == TrangDich.Aml && !phien.CoQuyen(NganHang.Core.Security.ChucNang.Aml)) return false;

        if (_khungNoiDung is null || !BangTrang.TryGetValue(trang, out var kieu)) return false;
        if (TrangHienTai == trang && thamSo is null) return false;
        var ok = _khungNoiDung.Navigate(kieu, thamSo, new EntranceNavigationTransitionInfo());
        if (ok)
        {
            TrangHienTai = trang;
            _khungNoiDung.BackStack.Clear();
            DaDieuHuong?.Invoke(this, trang);
        }
        return ok;
    }
}
