using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NganHang.App.Services;
using NganHang.Core.Constants;
using NganHang.Core.Entities;
using NganHang.Core.Policies;
using NganHang.Core.Services;

namespace NganHang.App.ViewModels;

public partial class TransferViewModel(AccountService accountService, DialogService dialog) : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<TaiKhoanThanhToan> danhSachTaiKhoanNguon = [];
    [ObservableProperty] private TaiKhoanThanhToan? taiKhoanNguon;
    [ObservableProperty] private string soTkDich = string.Empty;
    [ObservableProperty] private string tenNguoiNhan = string.Empty;
    [ObservableProperty] private double soTien = 50_000_000;
    [ObservableProperty] private string noiDung = "Chuyển tiền";
    [ObservableProperty] private decimal phiDuKien;
    [ObservableProperty] private bool canhBaoAml;

    public override Task KhiMoTrangAsync(object? thamSo) => TaiDanhSachNguonAsync();

    [RelayCommand]
    public Task TaiDanhSachNguonAsync() => ChayAsync(async () =>
    {
        var ds = await accountService.LayDanhSachAsync(chiHoatDong: true);
        DanhSachTaiKhoanNguon = new ObservableCollection<TaiKhoanThanhToan>(ds);
        if (DanhSachTaiKhoanNguon.Count > 0 && TaiKhoanNguon is null)
            TaiKhoanNguon = DanhSachTaiKhoanNguon[0];
        CapNhatPhiVaCanhBao();
    });

    partial void OnTaiKhoanNguonChanged(TaiKhoanThanhToan? value) => CapNhatPhiVaCanhBao();
    partial void OnSoTienChanged(double value) => CapNhatPhiVaCanhBao();

    private void CapNhatPhiVaCanhBao()
    {
        PhiDuKien = ChinhSach.TinhPhiChuyenKhoan(TaiKhoanNguon?.HangTaiKhoan);
        CanhBaoAml = SangTien(SoTien) >= ChinhSach.NguongCanhBaoAml;
    }

    [RelayCommand]
    public async Task TraCuuNguoiNhanAsync()
    {
        if (string.IsNullOrWhiteSpace(SoTkDich))
        {
            TenNguoiNhan = string.Empty;
            return;
        }

        var tk = await accountService.TimTheoSoAsync(SoTkDich.Trim());
        if (tk is not null)
        {
            TenNguoiNhan = tk.KhachHang?.HoTenKhachHang ?? "Tài khoản Techcombank";
        }
        else
        {
            TenNguoiNhan = "Không tìm thấy tài khoản thụ hưởng";
        }
    }

    [RelayCommand]
    private async Task ThucHienChuyenKhoanAsync()
    {
        if (TaiKhoanNguon is null)
        {
            HienLoi("Vui lòng chọn tài khoản nguồn.");
            return;
        }
        if (string.IsNullOrWhiteSpace(SoTkDich))
        {
            HienLoi("Vui lòng nhập tài khoản thụ hưởng.");
            return;
        }
        var st = SangTien(SoTien);
        if (st <= 0)
        {
            HienLoi("Số tiền chuyển phải lớn hơn 0 đ.");
            return;
        }

        var xacNhan = await dialog.XacNhanAsync(
            "Xác nhận chuyển khoản",
            $"Chuyển {st:N0} đ đến {TenNguoiNhan} ({SoTkDich})?\nPhí giao dịch: {PhiDuKien:N0} đ.",
            "Xác nhận chuyển");

        if (!xacNhan) return;

        await ChayAsync(async () =>
        {
            var kq = await accountService.ChuyenKhoanAsync(TaiKhoanNguon.SoTaiKhoan, SoTkDich, st, NoiDung);
            HienKetQua(kq);
            if (kq.ThanhCong)
            {
                await TaiDanhSachNguonAsync();
                SoTkDich = string.Empty;
                TenNguoiNhan = string.Empty;
                NoiDung = "Chuyển tiền";
            }
        });
    }
}
