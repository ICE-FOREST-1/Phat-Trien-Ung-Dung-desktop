using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using NganHang.App.ViewModels;

namespace NganHang.App.Views;

public sealed partial class AmlPage : Page
{
    public AmlViewModel ViewModel { get; } = App.GetService<AmlViewModel>();

    public AmlPage()
    {
        DataContext = ViewModel;
        InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await ViewModel.KhiMoTrangAsync(e.Parameter);
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (IsLoaded) _ = ViewModel.TaiDuLieuAsync();
    }
}
