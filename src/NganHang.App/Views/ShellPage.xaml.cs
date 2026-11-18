using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using NganHang.App.Services;
using NganHang.App.ViewModels;

namespace NganHang.App.Views;

public sealed partial class ShellPage : Page
{
    private readonly NavigationService _nav = App.GetService<NavigationService>();

    public ShellViewModel ViewModel { get; } = App.GetService<ShellViewModel>();

    private bool _dongBoMenu;

    public ShellPage()
    {
        DataContext = ViewModel;
        InitializeComponent();
        _nav.KhoiTaoKhungNoiDung(ContentFrame);
        _nav.DaDieuHuong += Nav_DaDieuHuong;
        Loaded += ShellPage_Loaded;
        Unloaded += (_, _) => _nav.DaDieuHuong -= Nav_DaDieuHuong;
    }

    private void ShellPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Loaded -= ShellPage_Loaded;
        if (_nav.TrangHienTai is null)
        {
            var trangDau = ViewModel.XemTongQuan ? TrangDich.TongQuan : TrangDich.KhachHang;
            _nav.DieuHuong(trangDau);
        }
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await ViewModel.KhiMoTrangAsync(e.Parameter);
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (_dongBoMenu) return;
        if (args.SelectedItemContainer?.Tag is string trang)
            _nav.DieuHuong(trang);
    }

    /// <summary>Đồng bộ mục menu đang chọn khi điều hướng từ nơi khác (vd: nút trên Dashboard).</summary>
    private void Nav_DaDieuHuong(object? sender, string trang)
    {
        var muc = NavView.MenuItems.Concat(NavView.FooterMenuItems)
            .OfType<NavigationViewItem>()
            .FirstOrDefault(i => i.Tag as string == trang);
        if (muc is not null && !ReferenceEquals(NavView.SelectedItem, muc))
        {
            _dongBoMenu = true;
            try
            {
                NavView.SelectedItem = muc;
            }
            finally
            {
                _dongBoMenu = false;
            }
        }
    }
}
