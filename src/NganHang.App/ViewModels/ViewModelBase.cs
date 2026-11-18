using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using NganHang.Core.Services;

namespace NganHang.App.ViewModels;

/// <summary>Lựa chọn hiển thị trong ComboBox (mã lưu DB + nhãn tiếng Việt).</summary>
public sealed record LuaChon(string Ma, string Ten)
{
    public override string ToString() => Ten;
}

/// <summary>
/// Lớp cơ sở cho mọi ViewModel: trạng thái đang tải và thanh thông báo (InfoBar).
/// </summary>
public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty] private bool dangTai;
    [ObservableProperty] private bool coThongBao;
    [ObservableProperty] private string thongBao = string.Empty;
    [ObservableProperty] private InfoBarSeverity mucDoThongBao = InfoBarSeverity.Informational;

    /// <summary>Được gọi khi trang được mở.</summary>
    public virtual Task KhiMoTrangAsync(object? thamSo) => Task.CompletedTask;

    protected void HienKetQua(KetQua kq)
    {
        if (kq.ThanhCong) HienThanhCong(kq.ThongBao);
        else HienLoi(kq.ThongBao);
    }

    protected void HienThanhCong(string noiDung) => Hien(noiDung, InfoBarSeverity.Success);
    protected void HienLoi(string noiDung) => Hien(noiDung, InfoBarSeverity.Error);
    protected void HienCanhBao(string noiDung) => Hien(noiDung, InfoBarSeverity.Warning);

    private void Hien(string noiDung, InfoBarSeverity mucDo)
    {
        ThongBao = noiDung;
        MucDoThongBao = mucDo;
        CoThongBao = false; // đóng rồi mở lại để InfoBar luôn hiển thị nội dung mới
        CoThongBao = true;
    }

    private int _dangTaiDem;

    /// <summary>Chạy một tác vụ bất đồng bộ với cờ DangTai (hỗ trợ gọi lồng nhau) và bắt lỗi chung.</summary>
    protected async Task ChayAsync(Func<Task> tacVu)
    {
        _dangTaiDem++;
        DangTai = true;
        try
        {
            await tacVu();
        }
        catch (Exception ex)
        {
            HienLoi(ex.InnerException?.Message ?? ex.Message);
        }
        finally
        {
            _dangTaiDem--;
            if (_dangTaiDem <= 0)
            {
                _dangTaiDem = 0;
                DangTai = false;
            }
        }
    }

    protected static decimal SangTien(double giaTri) => double.IsNaN(giaTri) || giaTri < 0 ? 0 : (decimal)Math.Round(giaTri, 0);
}
