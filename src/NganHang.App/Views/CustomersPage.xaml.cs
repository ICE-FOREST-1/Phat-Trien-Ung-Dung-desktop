using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using NganHang.App.ViewModels;
using NganHang.Core.Entities;

namespace NganHang.App.Views;

public sealed partial class CustomersPage : Page
{
    public CustomersViewModel ViewModel { get; } = App.GetService<CustomersViewModel>();

    public CustomersPage()
    {
        DataContext = ViewModel;
        InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await ViewModel.KhiMoTrangAsync(e.Parameter);
    }

    private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0 && e.AddedItems[0] is KhachHang kh)
            await ViewModel.ChonKhachHangAsync(kh);
    }

    private void SearchBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            _ = ViewModel.TimKiemAsync();
        }
    }
}
