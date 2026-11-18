using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace NganHang.App.Services;

/// <summary>Hiển thị hộp thoại (ContentDialog) dùng chung, tự gán XamlRoot và theme.</summary>
public class DialogService
{
    private bool _dangMo;

    public async Task<ContentDialogResult> HienAsync(ContentDialog dialog)
    {
        if (_dangMo) return ContentDialogResult.None; // WinUI chỉ cho mở 1 dialog tại một thời điểm
        var goc = App.MainWindow.RootElement;
        dialog.XamlRoot = goc.XamlRoot;
        dialog.RequestedTheme = goc.ActualTheme;
        _dangMo = true;
        try
        {
            return await dialog.ShowAsync();
        }
        finally
        {
            _dangMo = false;
        }
    }

    public async Task<bool> XacNhanAsync(string tieuDe, string noiDung, string nutDongY = "Đồng ý")
    {
        var dialog = new ContentDialog
        {
            Title = tieuDe,
            Content = new TextBlock { Text = noiDung, TextWrapping = TextWrapping.Wrap },
            PrimaryButtonText = nutDongY,
            CloseButtonText = "Hủy",
            DefaultButton = ContentDialogButton.Primary,
        };
        return await HienAsync(dialog) == ContentDialogResult.Primary;
    }

    public async Task ThongBaoAsync(string tieuDe, string noiDung)
    {
        var dialog = new ContentDialog
        {
            Title = tieuDe,
            Content = new TextBlock { Text = noiDung, TextWrapping = TextWrapping.Wrap },
            CloseButtonText = "Đóng",
        };
        await HienAsync(dialog);
    }

    /// <summary>Hộp thoại nhập văn bản (ví dụ lý do từ chối). Trả về null nếu người dùng hủy.</summary>
    public async Task<string?> NhapVanBanAsync(string tieuDe, string goiY, string nutDongY = "Xác nhận")
    {
        var hop = new TextBox { PlaceholderText = goiY, AcceptsReturn = true, TextWrapping = TextWrapping.Wrap, MinHeight = 80, Width = 380 };
        var dialog = new ContentDialog
        {
            Title = tieuDe,
            Content = hop,
            PrimaryButtonText = nutDongY,
            CloseButtonText = "Hủy",
            DefaultButton = ContentDialogButton.Primary,
        };
        return await HienAsync(dialog) == ContentDialogResult.Primary ? hop.Text : null;
    }
}
