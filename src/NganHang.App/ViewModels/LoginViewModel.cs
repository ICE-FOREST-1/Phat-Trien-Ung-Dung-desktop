using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NganHang.App.Services;
using NganHang.Core.Services;

namespace NganHang.App.ViewModels;

public partial class LoginViewModel(AuthService auth, NavigationService nav) : ViewModelBase
{
    [ObservableProperty] private string tenDangNhap = string.Empty;
    [ObservableProperty] private string matKhau = string.Empty;
    [ObservableProperty] private bool daKetNoi;
    [ObservableProperty] private string trangThaiKetNoi = "Đang kiểm tra kết nối SQL Server…";

    public override async Task KhiMoTrangAsync(object? thamSo)
    {
        var kq = await auth.KiemTraKetNoiAsync();
        DaKetNoi = kq.ThanhCong;
        TrangThaiKetNoi = kq.ThongBao;
        if (!kq.ThanhCong)
        {
            HienLoi("Không kết nối được cơ sở dữ liệu. Kiểm tra SQL Server và chuỗi kết nối trong appsettings.json.");
            return;
        }
        try
        {
            await auth.KhoiTaoMatKhauAsync();
        }
        catch (Exception ex)
        {
            HienCanhBao($"Không khởi tạo được mật khẩu mẫu: {ex.Message}");
        }
    }

    [RelayCommand]
    private Task DangNhapAsync() => ChayAsync(async () =>
    {
        var kq = await auth.DangNhapAsync(TenDangNhap, MatKhau);
        if (kq.ThanhCong) nav.VaoHeThong();
        else HienLoi(kq.ThongBao);
    });

    /// <summary>Điền nhanh tài khoản mẫu và đăng nhập.</summary>
    [RelayCommand]
    private Task DangNhapNhanhAsync(string tenDangNhap)
    {
        TenDangNhap = tenDangNhap;
        MatKhau = AuthService.MatKhauMacDinh;
        return DangNhapAsync();
    }
}
