using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NganHang.App.Services;
using NganHang.Core.Constants;
using NganHang.Core.Entities;
using NganHang.Core.Policies;
using NganHang.Core.Security;
using NganHang.Core.Services;

namespace NganHang.App.ViewModels;

public partial class LoansViewModel(LoanService loanService, AccountService accountService, PhienLamViec phien, DialogService dialog) : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<KhoanVay> danhSach = [];
    [ObservableProperty] private ObservableCollection<TaiKhoanThanhToan> danhSachTaiKhoan = [];
    [ObservableProperty] private ObservableCollection<TaiSanDamBao> danhSachTaiSan = [];
    [ObservableProperty] private KhoanVay? khoanVayChon;

    // Xem lịch trả nợ
    [ObservableProperty] private ObservableCollection<KyTraNo> lichTraNo = [];
    [ObservableProperty] private bool dangXemLich;

    // Form lập hồ sơ
    [ObservableProperty] private bool dangLapHoSo;
    [ObservableProperty] private TaiKhoanThanhToan? taiKhoanGiaiNganChon;
    [ObservableProperty] private string goiVayChon = GoiVay.TieuDungTinChap;
    [ObservableProperty] private double soTienVay = 100_000_000;
    [ObservableProperty] private int thoiHanThang = 12;
    [ObservableProperty] private double laiSuatVayNam = 11.5;
    [ObservableProperty] private string mucDich = "Tiêu dùng cá nhân";
    [ObservableProperty] private TaiSanDamBao? taiSanChon;
    [ObservableProperty] private decimal traGopHangThangDuKien;

    public bool DuocPhepLapHoSo => phien.CoQuyen(ChucNang.LapHoSoVay);
    public bool DuocPhepPheDuyet => phien.CoQuyen(ChucNang.PheDuyetVay);
    public Microsoft.UI.Xaml.Visibility HienThiNutPheDuyet => DuocPhepPheDuyet ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
    public IReadOnlyList<string> DanhSachGoiVay => GoiVay.TatCa;

    public override Task KhiMoTrangAsync(object? thamSo) => TaiDuLieuAsync();

    [RelayCommand]
    public Task TaiDuLieuAsync() => ChayAsync(async () =>
    {
        var ds = await loanService.LayDanhSachAsync();
        DanhSach = new ObservableCollection<KhoanVay>(ds);

        var dsTk = await accountService.LayDanhSachAsync(chiHoatDong: true);
        DanhSachTaiKhoan = new ObservableCollection<TaiKhoanThanhToan>(dsTk);
        if (DanhSachTaiKhoan.Count > 0 && TaiKhoanGiaiNganChon is null)
            TaiKhoanGiaiNganChon = DanhSachTaiKhoan[0];

        var dsTs = await loanService.LayTaiSanDamBaoAsync();
        DanhSachTaiSan = new ObservableCollection<TaiSanDamBao>(dsTs);

        CapNhatLaiSuatVaTraGop();
    });

    partial void OnGoiVayChonChanged(string value)
    {
        if (ChinhSach.LaiSuatVay.TryGetValue(value, out var ls))
            LaiSuatVayNam = (double)ls;
        CapNhatLaiSuatVaTraGop();
    }
    partial void OnSoTienVayChanged(double value) => CapNhatLaiSuatVaTraGop();
    partial void OnThoiHanThangChanged(int value) => CapNhatLaiSuatVaTraGop();

    private void CapNhatLaiSuatVaTraGop()
    {
        TraGopHangThangDuKien = ChinhSach.TinhTraGopHangThang(SangTien(SoTienVay), (decimal)LaiSuatVayNam, ThoiHanThang);
    }

    [RelayCommand]
    private void BatDauLapHoSo()
    {
        DangLapHoSo = true;
        DangXemLich = false;
        GoiVayChon = GoiVay.TieuDungTinChap;
        SoTienVay = 100_000_000;
        ThoiHanThang = 12;
        CapNhatLaiSuatVaTraGop();
    }

    [RelayCommand]
    private void HuyLapHoSo() => DangLapHoSo = false;

    [RelayCommand]
    private Task XacNhanLapHoSoAsync() => ChayAsync(async () =>
    {
        if (TaiKhoanGiaiNganChon is null)
        {
            HienLoi("Vui lòng chọn tài khoản giải ngân.");
            return;
        }

        var hs = new HoSoVayMoi(
            TaiKhoanGiaiNganChon.MaTaiKhoan,
            GoiVayChon,
            SangTien(SoTienVay),
            ThoiHanThang,
            (decimal)LaiSuatVayNam,
            MucDich,
            TaiSanChon?.MaTaiSan);

        var kq = await loanService.LapHoSoAsync(hs);
        HienKetQua(kq);
        if (kq.ThanhCong)
        {
            DangLapHoSo = false;
            await TaiDuLieuAsync();
        }
    });

    [RelayCommand]
    private async Task PheDuyetAsync(KhoanVay? kv)
    {
        if (!DuocPhepPheDuyet)
        {
            HienLoi("Giao dịch viên không có quyền phê duyệt khoản vay. Chỉ Quản trị viên / Lãnh đạo mới được phép phê duyệt.");
            return;
        }
        if (kv is null) return;
        var xacNhan = await dialog.XacNhanAsync(
            "Phê duyệt và giải ngân",
            $"Phê duyệt hợp đồng {kv.MaSoHopDongVay}?\nSố tiền giải ngân: {kv.SoTienVayBanDau:N0} đ sẽ được chuyển vào TK {kv.TaiKhoanGiaiNgan?.SoTaiKhoan}.",
            "Phê duyệt & Giải ngân");

        if (!xacNhan) return;

        await ChayAsync(async () =>
        {
            var kq = await loanService.PheDuyetVaGiaiNganAsync(kv.MaKhoanVay);
            HienKetQua(kq);
            if (kq.ThanhCong) await TaiDuLieuAsync();
        });
    }

    [RelayCommand]
    private async Task TuChoiAsync(KhoanVay? kv)
    {
        if (!DuocPhepPheDuyet)
        {
            HienLoi("Giao dịch viên không có quyền từ chối hoặc phê duyệt khoản vay. Chỉ Quản trị viên / Lãnh đạo mới được phép thực hiện.");
            return;
        }
        if (kv is null) return;
        var lyDo = await dialog.NhapVanBanAsync("Từ chối hồ sơ vay", "Nhập lý do từ chối hồ sơ…", "Xác nhận từ chối");
        if (string.IsNullOrWhiteSpace(lyDo)) return;

        await ChayAsync(async () =>
        {
            var kq = await loanService.TuChoiAsync(kv.MaKhoanVay, lyDo);
            HienKetQua(kq);
            if (kq.ThanhCong) await TaiDuLieuAsync();
        });
    }

    [RelayCommand]
    private void XemLichTraNo(KhoanVay? kv)
    {
        if (kv is null) return;
        KhoanVayChon = kv;
        var lich = ChinhSach.LapLichTraNo(kv.SoTienVayBanDau, kv.LaiSuatVayNam, kv.ThoiHanVayThang, kv.NgayGiaiNgan);
        LichTraNo = new ObservableCollection<KyTraNo>(lich);
        DangXemLich = true;
        DangLapHoSo = false;
    }

    [RelayCommand]
    private void DongLichTraNo() => DangXemLich = false;
}
