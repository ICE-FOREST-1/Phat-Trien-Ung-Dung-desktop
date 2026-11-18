using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using NganHang.App.ViewModels;

namespace NganHang.App.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardViewModel ViewModel { get; } = App.GetService<DashboardViewModel>();

    public DashboardPage()
    {
        DataContext = ViewModel;
        InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await ViewModel.KhiMoTrangAsync(e.Parameter);
    }
}
