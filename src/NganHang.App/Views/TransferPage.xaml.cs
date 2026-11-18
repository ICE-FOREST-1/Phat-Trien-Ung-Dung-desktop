using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using NganHang.App.ViewModels;

namespace NganHang.App.Views;

public sealed partial class TransferPage : Page
{
    public TransferViewModel ViewModel { get; } = App.GetService<TransferViewModel>();

    public TransferPage()
    {
        DataContext = ViewModel;
        InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await ViewModel.KhiMoTrangAsync(e.Parameter);
    }

    private void QuickAmount_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string s && double.TryParse(s, out var tien))
            ViewModel.SoTien = tien;
    }
}
