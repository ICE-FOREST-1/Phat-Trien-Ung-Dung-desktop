using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NganHang.App.Services;
using NganHang.Core.Constants;
using NganHang.Core.Security;
using NganHang.Core.Services;

namespace NganHang.App.ViewModels;

public partial class ShellViewModel(PhienLamViec phien, AuthService auth, NavigationService nav) : ViewModelBase
{
    [ObservableProperty] private bool daKetNoi = true;
    [ObservableProperty] private string trangThaiKetNoi = "Đang kiểm tra…";

    public string HoTen => phien.NguoiDung?.HoTen ?? string.Empty;
    public string VaiTro => HienThi.Ma(phien.NguoiDung?.VaiTro);
    public string ChiNhanh => phien.NguoiDung?.ChiNhanh ?? string.Empty;

    // Quyền hiển thị menu – lấy từ ma trận phân quyền tập trung
    public bool XemTongQuan => phien.CoQuyen(ChucNang.TongQuan);
    public bool XemKhachHang => phien.CoQuyen(ChucNang.KhachHang);
    public bool XemTaiKhoan => phien.CoQuyen(ChucNang.MoTaiKhoan);
    public bool XemTietKiem => phien.CoQuyen(ChucNang.TietKiem);
    public bool XemTinDung => phien.CoQuyen(ChucNang.TinDung);
    public bool XemChuyenKhoan => phien.CoQuyen(ChucNang.ChuyenKhoan);
    public bool XemNhatKy => phien.CoQuyen(ChucNang.NhatKyGiaoDich);
    public bool XemAml => phien.CoQuyen(ChucNang.Aml);
    public bool XemHangHoiVien => phien.CoQuyen(ChucNang.HangHoiVien);
    public bool XemNhomNghiepVu => XemTaiKhoan || XemTietKiem || XemTinDung || XemChuyenKhoan;

    public override async Task KhiMoTrangAsync(object? thamSo)
    {
        var kq = await auth.KiemTraKetNoiAsync();
        DaKetNoi = kq.ThanhCong;
        TrangThaiKetNoi = kq.ThongBao;
    }

    [RelayCommand]
    private void DieuHuong(string trang) => nav.DieuHuong(trang);

    [RelayCommand]
    private void DangXuat() => nav.DangXuat();
}
