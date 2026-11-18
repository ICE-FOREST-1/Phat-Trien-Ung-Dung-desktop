using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NganHang.App.Services;
using NganHang.Core.Constants;
using NganHang.Core.Entities;
using NganHang.Core.Security;
using NganHang.Core.Services;

namespace NganHang.App.ViewModels;

public partial class CustomersViewModel(CustomerService customerService, PhienLamViec phien) : ViewModelBase
{
    [ObservableProperty] private string tuKhoa = string.Empty;
    [ObservableProperty] private ObservableCollection<KhachHang> danhSach = [];
    [ObservableProperty] private KhachHang? khachHangChon;
    [ObservableProperty] private bool dangChinhSua;

    private CancellationTokenSource? _searchCts;

    partial void OnTuKhoaChanged(string value)
    {
        _searchCts?.Cancel();
        _searchCts = new CancellationTokenSource();
        var token = _searchCts.Token;

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(200, token);
                if (token.IsCancellationRequested) return;

                var ds = await customerService.TimKiemAsync(value);
                if (token.IsCancellationRequested) return;

                App.MainWindow.DispatcherQueue.TryEnqueue(() =>
                {
                    if (token.IsCancellationRequested) return;
                    DanhSach = new ObservableCollection<KhachHang>(ds);
                    if (DanhSach.Count > 0)
                        _ = ChonKhachHangAsync(DanhSach[0]);
                    else
                        KhachHangChon = null;
                });
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                App.MainWindow.DispatcherQueue.TryEnqueue(() => HienLoi(ex.Message));
            }
        }, token);
    }

    // Form thêm / sửa
    [ObservableProperty] private int formMaKhachHang;
    [ObservableProperty] private string formHoTen = string.Empty;
    [ObservableProperty] private string formCccd = string.Empty;
    [ObservableProperty] private string formSdt = string.Empty;
    [ObservableProperty] private string formEmail = string.Empty;
    [ObservableProperty] private string formDiaChi = string.Empty;
    [ObservableProperty] private DateTimeOffset formNgaySinh = new(1990, 1, 1, 0, 0, 0, TimeSpan.Zero);
    [ObservableProperty] private string formGioiTinh = GioiTinh.Nam;
    [ObservableProperty] private string formNgheNghiep = string.Empty;
    [ObservableProperty] private double formThuNhap;
    [ObservableProperty] private int formDiemTinDung = 700;

    public bool DuocPhepThemSua => phien.CoQuyen(ChucNang.ThemSuaKhachHang);
    public IReadOnlyList<string> DanhSachGioiTinh => GioiTinh.TatCa;

    public override Task KhiMoTrangAsync(object? thamSo) => TimKiemAsync();

    [RelayCommand]
    public Task TimKiemAsync()
    {
        _searchCts?.Cancel();
        return ChayAsync(async () =>
        {
            var ds = await customerService.TimKiemAsync(TuKhoa);
            DanhSach = new ObservableCollection<KhachHang>(ds);
            if (DanhSach.Count > 0)
                await ChonKhachHangAsync(DanhSach[0]);
            else
                KhachHangChon = null;
        });
    }

    [RelayCommand]
    public async Task ChonKhachHangAsync(KhachHang? kh)
    {
        if (kh is null)
        {
            KhachHangChon = null;
            return;
        }
        var chiTiet = await customerService.LayChiTietAsync(kh.MaKhachHang);
        KhachHangChon = chiTiet ?? kh;
    }

    [RelayCommand]
    private void BatDauThem()
    {
        FormMaKhachHang = 0;
        FormHoTen = string.Empty;
        FormCccd = string.Empty;
        FormSdt = string.Empty;
        FormEmail = string.Empty;
        FormDiaChi = string.Empty;
        FormNgaySinh = new DateTimeOffset(1995, 1, 1, 0, 0, 0, TimeSpan.Zero);
        FormGioiTinh = GioiTinh.Nam;
        FormNgheNghiep = string.Empty;
        FormThuNhap = 20_000_000;
        FormDiemTinDung = 700;
        DangChinhSua = true;
    }

    [RelayCommand]
    private void BatDauSua()
    {
        if (KhachHangChon is null) return;
        FormMaKhachHang = KhachHangChon.MaKhachHang;
        FormHoTen = KhachHangChon.HoTenKhachHang;
        FormCccd = KhachHangChon.SoCCCD;
        FormSdt = KhachHangChon.SoDienThoai;
        FormEmail = KhachHangChon.Email ?? string.Empty;
        FormDiaChi = KhachHangChon.DiaChiThuongTru;
        FormNgaySinh = new DateTimeOffset(KhachHangChon.NgaySinh.Year, KhachHangChon.NgaySinh.Month, KhachHangChon.NgaySinh.Day, 0, 0, 0, TimeSpan.Zero);
        FormGioiTinh = KhachHangChon.GioiTinh ?? GioiTinh.Nam;
        FormNgheNghiep = KhachHangChon.NgheNghiep ?? string.Empty;
        FormThuNhap = (double)(KhachHangChon.ThuNhapHangThang ?? 0);
        FormDiemTinDung = KhachHangChon.DiemTinDung ?? 700;
        DangChinhSua = true;
    }

    [RelayCommand]
    private void HuyChinhSua() => DangChinhSua = false;

    [RelayCommand]
    private Task LuuKhachHangAsync() => ChayAsync(async () =>
    {
        var kh = new KhachHang
        {
            MaKhachHang = FormMaKhachHang,
            HoTenKhachHang = FormHoTen,
            SoCCCD = FormCccd,
            SoDienThoai = FormSdt,
            Email = FormEmail,
            DiaChiThuongTru = FormDiaChi,
            NgaySinh = FormNgaySinh.Date,
            GioiTinh = FormGioiTinh,
            NgheNghiep = FormNgheNghiep,
            ThuNhapHangThang = SangTien(FormThuNhap),
            DiemTinDung = FormDiemTinDung,
        };

        var kq = await customerService.LuuAsync(kh);
        HienKetQua(kq);
        if (kq.ThanhCong)
        {
            DangChinhSua = false;
            await TimKiemAsync();
        }
    });
}
