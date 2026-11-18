using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using NganHang.App.ViewModels;

namespace NganHang.App.Views;

public sealed partial class LoansPage : Page
{
    public LoansViewModel ViewModel { get; } = App.GetService<LoansViewModel>();

    public LoansPage()
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
