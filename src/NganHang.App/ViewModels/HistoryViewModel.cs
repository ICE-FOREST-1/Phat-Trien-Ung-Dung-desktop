using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NganHang.Core.Constants;
using NganHang.Core.Entities;
using NganHang.Core.Services;

namespace NganHang.App.ViewModels;

public partial class HistoryViewModel(ReportService report) : ViewModelBase
{
    [ObservableProperty] private string tuKhoa = string.Empty;
    [ObservableProperty] private LuaChon loaiChon = new(string.Empty, "— Tất cả loại giao dịch —");
    [ObservableProperty] private DateTimeOffset? tuNgay;
    [ObservableProperty] private DateTimeOffset? denNgay;
    [ObservableProperty] private ObservableCollection<GiaoDich> danhSach = [];

    public IReadOnlyList<LuaChon> DanhSachLoai { get; } =
    [
        new(string.Empty, "— Tất cả loại giao dịch —"),
        new(LoaiGiaoDich.ChuyenKhoanNoiBo, "Chuyển khoản nội bộ"),
        new(LoaiGiaoDich.ChuyenKhoanLienNganHang, "Chuyển khoản liên ngân hàng"),
        new(LoaiGiaoDich.NapTienMat, "Nộp tiền mặt"),
        new(LoaiGiaoDich.RutTienMat, "Rút tiền mặt"),
        new(LoaiGiaoDich.MoSoTietKiem, "Mở sổ tiết kiệm"),
        new(LoaiGiaoDich.TatToanSoTietKiem, "Tất toán sổ tiết kiệm"),
        new(LoaiGiaoDich.GiaiNganKhoanVay, "Giải ngân khoản vay"),
        new(LoaiGiaoDich.ThanhToanGocLaiVay, "Thanh toán gốc lãi vay"),
    ];

    public override Task KhiMoTrangAsync(object? thamSo) => TimKiemAsync();

    [RelayCommand]
    public Task TimKiemAsync() => ChayAsync(async () =>
    {
        var loai = string.IsNullOrEmpty(LoaiChon.Ma) ? null : LoaiChon.Ma;
        var ds = await report.TraCuuGiaoDichAsync(TuKhoa, loai, TuNgay?.Date, DenNgay?.Date);
        DanhSach = new ObservableCollection<GiaoDich>(ds);
    });

    [RelayCommand]
    private void DatLaiBoLoc()
    {
        TuKhoa = string.Empty;
        LoaiChon = DanhSachLoai[0];
        TuNgay = null;
        DenNgay = null;
        _ = TimKiemAsync();
    }
}
