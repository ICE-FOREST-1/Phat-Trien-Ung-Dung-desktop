using System.Text.Json;
using Microsoft.UI.Xaml;

namespace NganHang.App.Services;

/// <summary>Quản lý chế độ Sáng / Tối, lưu lựa chọn vào %LocalAppData%\NganHang\settings.json.</summary>
public class ThemeService
{
    private static readonly string ThuMuc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NganHang");
    private static readonly string TepCaiDat = Path.Combine(ThuMuc, "settings.json");

    private MainWindow? _window;

    public ElementTheme ThemeHienTai { get; private set; } = ElementTheme.Default;

    public void KhoiTao(MainWindow window)
    {
        _window = window;
        ApDung(DocCaiDat());
    }

    public void ApDung(ElementTheme theme)
    {
        ThemeHienTai = theme;
        if (_window is null) return;
        _window.RootElement.RequestedTheme = theme;
        _window.CapNhatMauNutTieuDe(theme);
        LuuCaiDat(theme);
    }

    private static ElementTheme DocCaiDat()
    {
        try
        {
            if (!File.Exists(TepCaiDat)) return ElementTheme.Default;
            var cd = JsonSerializer.Deserialize<CaiDat>(File.ReadAllText(TepCaiDat));
            return Enum.TryParse<ElementTheme>(cd?.Theme, out var t) ? t : ElementTheme.Default;
        }
        catch
        {
            return ElementTheme.Default;
        }
    }

    private static void LuuCaiDat(ElementTheme theme)
    {
        try
        {
            Directory.CreateDirectory(ThuMuc);
            File.WriteAllText(TepCaiDat, JsonSerializer.Serialize(new CaiDat { Theme = theme.ToString() }));
        }
        catch
        {
            // Không lưu được cài đặt thì bỏ qua, không ảnh hưởng nghiệp vụ.
        }
    }

    private sealed class CaiDat
    {
        public string? Theme { get; set; }
    }
}
