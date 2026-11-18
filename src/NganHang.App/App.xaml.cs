using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using NganHang.App.Services;
using NganHang.App.ViewModels;
using NganHang.Core;

namespace NganHang.App;

public partial class App : Application
{
    /// <summary>Generic Host: cấu hình (appsettings.json) + Dependency Injection.</summary>
    public static IHost AppHost { get; private set; } = null!;

    public static MainWindow MainWindow { get; private set; } = null!;

    public static T GetService<T>() where T : class => AppHost.Services.GetRequiredService<T>();

    public App()
    {
        InitializeComponent();

        AppHost = Host.CreateDefaultBuilder()
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureServices((ctx, services) =>
            {
                var chuoiKetNoi = ctx.Configuration.GetConnectionString("CoreBanking")
                    ?? throw new InvalidOperationException("Thiếu ConnectionStrings:CoreBanking trong appsettings.json");

                // Tầng nghiệp vụ & dữ liệu
                services.AddNganHangCore(chuoiKetNoi);

                // Dịch vụ giao diện
                services.AddSingleton<ThemeService>();
                services.AddSingleton<NavigationService>();
                services.AddSingleton<DialogService>();

                // ViewModels (mỗi lần mở trang tạo mới)
                services.AddTransient<LoginViewModel>();
                services.AddTransient<ShellViewModel>();
                services.AddTransient<DashboardViewModel>();
                services.AddTransient<CustomersViewModel>();
                services.AddTransient<AccountsViewModel>();
                services.AddTransient<SavingsViewModel>();
                services.AddTransient<LoansViewModel>();
                services.AddTransient<TransferViewModel>();
                services.AddTransient<HistoryViewModel>();
                services.AddTransient<AmlViewModel>();
                services.AddTransient<TiersViewModel>();
                services.AddTransient<SettingsViewModel>();
            })
            .Build();

        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            GhiNhatKyLoi("AppDomain", e.ExceptionObject as Exception ?? new Exception(e.ExceptionObject?.ToString()));
        };

        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            GhiNhatKyLoi("TaskScheduler", e.Exception);
            e.SetObserved();
        };

        UnhandledException += (_, e) =>
        {
            GhiNhatKyLoi("UnhandledException", e.Exception);
            e.Handled = true;
            try
            {
                _ = GetService<DialogService>().ThongBaoAsync("Lỗi hệ thống", e.Exception.Message);
            }
            catch { }
        };
    }

    public static void GhiNhatKyLoi(string nguon, Exception? ex)
    {
        try
        {
            var noiDung = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{nguon}] {ex?.GetType().FullName}: {ex?.Message}\n{ex?.StackTrace}\n\n";
            File.AppendAllText(Path.Combine(AppContext.BaseDirectory, "crash.log"), noiDung);
            File.AppendAllText(@"E:\NganHang\crash.log", noiDung);
        }
        catch { }
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindow = new MainWindow();
        GetService<ThemeService>().KhoiTao(MainWindow);
        GetService<NavigationService>().KhoiTaoKhungGoc(MainWindow.RootFrame);
        GetService<NavigationService>().VeManHinhDangNhap();
        MainWindow.Activate();
    }
}
