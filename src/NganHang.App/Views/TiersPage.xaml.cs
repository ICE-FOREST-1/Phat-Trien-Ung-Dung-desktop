using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using NganHang.App.ViewModels;

namespace NganHang.App.Views;

public sealed partial class TiersPage : Page
{
    public TiersViewModel ViewModel { get; } = App.GetService<TiersViewModel>();

    public TiersPage()
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
