using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Graphics;

namespace NganHang.App;

public sealed partial class MainWindow : Window
{
    public Frame RootFrame => RootFrameElement;
    public FrameworkElement RootElement => RootGrid;

    public MainWindow()
    {
        InitializeComponent();
        Title = "Techcombank Core Banking";

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

        // Kích thước & căn giữa màn hình
        AppWindow.Resize(new SizeInt32(1440, 900));
        var vung = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Primary).WorkArea;
        AppWindow.Move(new PointInt32(vung.X + Math.Max(0, (vung.Width - 1440) / 2), vung.Y + Math.Max(0, (vung.Height - 900) / 2)));
    }

    /// <summary>Đổi màu nút thu nhỏ / phóng to / đóng theo theme.</summary>
    public void CapNhatMauNutTieuDe(ElementTheme theme)
    {
        var toi = theme == ElementTheme.Dark ||
                  (theme == ElementTheme.Default && Application.Current.RequestedTheme == ApplicationTheme.Dark);
        AppWindow.TitleBar.ButtonForegroundColor = toi ? Colors.White : Colors.Black;
        AppWindow.TitleBar.ButtonHoverBackgroundColor = toi ? ColorHelper.FromArgb(0x20, 0xFF, 0xFF, 0xFF) : ColorHelper.FromArgb(0x20, 0, 0, 0);
    }
}
