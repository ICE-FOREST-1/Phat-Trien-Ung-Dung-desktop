using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NganHang.App.Services;
using NganHang.Core.Constants;
using NganHang.Core.Entities;
using NganHang.Core.Security;
using NganHang.Core.Services;

namespace NganHang.App.ViewModels;

public partial class AmlViewModel(AmlService amlService, PhienLamViec phien, DialogService dialog) : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<CanhBaoAML> danhSach = [];
    [ObservableProperty] private LuaChon trangThaiChon = new(string.Empty, "— Tất cả trạng thái —");

    public bool DuocPhepXuLy => phien.CoQuyen(ChucNang.XuLyAml);

    public IReadOnlyList<LuaChon> DanhSachTrangThai { get; } =
    [
        new(string.Empty, "— Tất cả trạng thái —"),
        new(TrangThaiAml.ChoKiemDuyet, "Chờ kiểm duyệt"),
        new(TrangThaiAml.DaDuyetAnToan, "Đã duyệt an toàn"),
        new(TrangThaiAml.KhoaDongBang, "Đã khóa đóng băng tài khoản"),
    ];

    public override Task KhiMoTrangAsync(object? thamSo) => TaiDuLieuAsync();

    [RelayCommand]
    public Task TaiDuLieuAsync() => ChayAsync(async () =>
    {
        var tt = string.IsNullOrEmpty(TrangThaiChon.Ma) ? null : TrangThaiChon.Ma;
        var ds = await amlService.LayDanhSachAsync(tt);
        DanhSach = new ObservableCollection<CanhBaoAML>(ds);
    });

    [RelayCommand]
    private async Task DuyetAnToanAsync(CanhBaoAML? cb)
    {
        if (cb is null) return;
        var xacNhan = await dialog.XacNhanAsync(
            "Phê duyệt an toàn AML",
            $"Xác nhận giao dịch {cb.SoTienPhatSinh:N0} đ của {cb.TenKhachHang} là hợp lệ và an toàn?",
            "Duyệt an toàn");

        if (!xacNhan) return;

        await ChayAsync(async () =>
        {
            var kq = await amlService.DuyetAnToanAsync(cb.MaCanhBao);
            HienKetQua(kq);
            if (kq.ThanhCong) await TaiDuLieuAsync();
        });
    }

    [RelayCommand]
    private async Task KhoaTaiKhoanAsync(CanhBaoAML? cb)
    {
        if (cb is null) return;
        var xacNhan = await dialog.XacNhanAsync(
            "KHÓA ĐÓNG BĂNG TÀI KHOẢN",
            $"CẢNH BÁO: Bạn có chắc chắn muốn khóa tài khoản {cb.SoTaiKhoanPhatSinh} của {cb.TenKhachHang} để điều tra rửa tiền?",
            "Khóa tài khoản ngay");

        if (!xacNhan) return;

        await ChayAsync(async () =>
        {
            var kq = await amlService.KhoaTaiKhoanAsync(cb.MaCanhBao);
            HienKetQua(kq);
            if (kq.ThanhCong) await TaiDuLieuAsync();
        });
    }
}
