using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using NganHang.App.ViewModels;
using Windows.System;

namespace NganHang.App.Views;

public sealed partial class LoginPage : Page
{
    public LoginViewModel ViewModel { get; } = App.GetService<LoginViewModel>();

    public LoginPage()
    {
        DataContext = ViewModel;
        InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await ViewModel.KhiMoTrangAsync(e.Parameter);
    }

    private void MatKhauBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter && ViewModel.DangNhapCommand.CanExecute(null))
            ViewModel.DangNhapCommand.Execute(null);
    }
}
