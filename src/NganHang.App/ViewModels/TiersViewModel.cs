using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using NganHang.Core.Policies;

namespace NganHang.App.ViewModels;

public partial class TiersViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<ThongTinHang> danhSachHang = [];

    public override Task KhiMoTrangAsync(object? thamSo)
    {
        DanhSachHang = new ObservableCollection<ThongTinHang>(ChinhSach.CacHang);
        return Task.CompletedTask;
    }
}
