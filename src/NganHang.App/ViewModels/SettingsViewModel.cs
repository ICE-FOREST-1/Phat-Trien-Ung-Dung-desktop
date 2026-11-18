using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using NganHang.App.Services;
using NganHang.Core.Constants;
using NganHang.Core.Entities;
using NganHang.Core.Services;

namespace NganHang.App.ViewModels;

public partial class SettingsViewModel(AuthService auth, PhienLamViec phien, ThemeService themeService) : ViewModelBase
{
    [ObservableProperty] private string matKhauCu = string.Empty;
    [ObservableProperty] private string matKhauMoi = string.Empty;
    [ObservableProperty] private string xacNhanMatKhau = string.Empty;

    [ObservableProperty] private bool daKetNoi;
    [ObservableProperty] private string thongTinKetNoi = string.Empty;

    public NguoiDung? NguoiDungHienTai => phien.NguoiDung;
    public string TenVaiTro => HienThi.Ma(phien.NguoiDung?.VaiTro);

    public int ThemeIndex
    {
        get => themeService.ThemeHienTai switch
        {
            ElementTheme.Light => 1,
            ElementTheme.Dark => 2,
            _ => 0,
        };
        set
        {
            var t = value switch
            {
                1 => ElementTheme.Light,
                2 => ElementTheme.Dark,
                _ => ElementTheme.Default,
            };
            themeService.ApDung(t);
            OnPropertyChanged();
        }
    }

    public override Task KhiMoTrangAsync(object? thamSo) => KiemTraKetNoiAsync();

    [RelayCommand]
    public Task KiemTraKetNoiAsync() => ChayAsync(async () =>
    {
        var kq = await auth.KiemTraKetNoiAsync();
        DaKetNoi = kq.ThanhCong;
        ThongTinKetNoi = kq.ThongBao;
    });

    [RelayCommand]
    private Task DoiMatKhauAsync() => ChayAsync(async () =>
    {
        if (string.IsNullOrEmpty(MatKhauCu))
        {
            HienLoi("Vui lòng nhập mật khẩu hiện tại.");
            return;
        }
        if (MatKhauMoi.Length < 6)
        {
            HienLoi("Mật khẩu mới phải có ít nhất 6 ký tự.");
            return;
        }
        if (MatKhauMoi != XacNhanMatKhau)
        {
            HienLoi("Mật khẩu xác nhận không khớp.");
            return;
        }

        var kq = await auth.DoiMatKhauAsync(MatKhauCu, MatKhauMoi);
        HienKetQua(kq);
        if (kq.ThanhCong)
        {
            MatKhauCu = string.Empty;
            MatKhauMoi = string.Empty;
            XacNhanMatKhau = string.Empty;
        }
    });
}
