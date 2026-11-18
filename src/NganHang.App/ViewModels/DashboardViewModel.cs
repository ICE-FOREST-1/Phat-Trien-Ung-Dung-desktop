using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NganHang.App.Services;
using NganHang.Core.Entities;
using NganHang.Core.Services;

namespace NganHang.App.ViewModels;

public sealed record CotDoanhSo(string Ngay, string DoanhSoHienThi, double ChieuCao, int SoGiaoDich, string ChiTiet);

public partial class DashboardViewModel(ReportService report, NavigationService nav) : ViewModelBase
{
    [ObservableProperty] private TongQuan? duLieu;
    [ObservableProperty] private ObservableCollection<CotDoanhSo> cacCotDoanhSo = [];
    [ObservableProperty] private string tongDoanhSo7Ngay = "0 đ";
    [ObservableProperty] private ObservableCollection<GiaoDich> giaoDichGanDay = [];

    public override Task KhiMoTrangAsync(object? thamSo) => TaiDuLieuAsync();

    [RelayCommand]
    public Task TaiDuLieuAsync() => ChayAsync(async () =>
    {
        DuLieu = await report.LayTongQuanAsync();
        GiaoDichGanDay = new ObservableCollection<GiaoDich>(DuLieu.GiaoDichGanDay);

        var maxTien = DuLieu.GiaoDich7Ngay.Any() ? DuLieu.GiaoDich7Ngay.Max(d => d.TongTien) : 0;
        if (maxTien <= 0) maxTien = 1;

        var danhSach = new ObservableCollection<CotDoanhSo>();
        foreach (var d in DuLieu.GiaoDich7Ngay)
        {
            var trieu = Math.Round(d.TongTien / 1_000_000m, 1);
            var chieuCao = Math.Max(12.0, (double)(d.TongTien / maxTien) * 150.0);
            danhSach.Add(new CotDoanhSo(
                Ngay: d.Ngay.ToString("dd/MM"),
                DoanhSoHienThi: trieu > 0 ? $"{trieu:N1} tr" : "0",
                ChieuCao: chieuCao,
                SoGiaoDich: d.SoGiaoDich,
                ChiTiet: $"{d.Ngay:dd/MM/yyyy}: {d.TongTien:N0} đ ({d.SoGiaoDich} giao dịch)"
            ));
        }

        CacCotDoanhSo = danhSach;
        TongDoanhSo7Ngay = $"{DuLieu.GiaoDich7Ngay.Sum(d => d.TongTien):N0} đ";
    });

    [RelayCommand]
    private void DieuHuong(string trang) => nav.DieuHuong(trang);
}
