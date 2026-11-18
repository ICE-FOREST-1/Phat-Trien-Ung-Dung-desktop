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

public partial class SavingsViewModel(SavingsService savingsService, AccountService accountService, CustomerService customerService, PhienLamViec phien, DialogService dialog) : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<SoTietKiem> danhSach = [];
    [ObservableProperty] private ObservableCollection<TaiKhoanThanhToan> danhSachTaiKhoan = [];
    [ObservableProperty] private ObservableCollection<KhachHang> danhSachKhachHang = [];
    [ObservableProperty] private SoTietKiem? soChon;

    // Form mở sổ
    [ObservableProperty] private bool dangMoSo;
    [ObservableProperty] private bool guiTienMat;
    [ObservableProperty] private bool trichTaiKhoan = true;

    partial void OnGuiTienMatChanged(bool value) => TrichTaiKhoan = !value;
    partial void OnTrichTaiKhoanChanged(bool value) => GuiTienMat = !value;

    [ObservableProperty] private TaiKhoanThanhToan? taiKhoanNguonChon;
    [ObservableProperty] private double soTienGui = 50_000_000;
    [ObservableProperty] private int kyHanThang = 12;
    [ObservableProperty] private decimal laiSuatApDung = 6.2m;
    [ObservableProperty] private decimal tienLaiDuKien;

    // Trường hợp nộp tiền mặt (không cần tài khoản thanh toán)
    [ObservableProperty] private bool laKhachHangMoi;
    [ObservableProperty] private bool laKhachHangHienHuu = true;

    partial void OnLaKhachHangMoiChanged(bool value) => LaKhachHangHienHuu = !value;
    partial void OnLaKhachHangHienHuuChanged(bool value) => LaKhachHangMoi = !value;

    [ObservableProperty] private KhachHang? khachHangChon;
    [ObservableProperty] private string formHoTen = string.Empty;
    [ObservableProperty] private string formCccd = string.Empty;
    [ObservableProperty] private string formSdt = string.Empty;
    [ObservableProperty] private string formDiaChi = string.Empty;

    // Mã số sổ tiết kiệm / Mã định danh sổ
    [ObservableProperty] private string formMaSoSo = string.Empty;

    public bool DuocPhepMoSo => phien.CoQuyen(ChucNang.TietKiem);
    public IReadOnlyList<int> DanhSachKyHan => ChinhSach.LaiSuatTietKiem.Keys.ToList();

    public override Task KhiMoTrangAsync(object? thamSo) => TaiDuLieuAsync();

    [RelayCommand]
    public Task TaiDuLieuAsync() => ChayAsync(async () =>
    {
        var ds = await savingsService.LayDanhSachAsync();
        DanhSach = new ObservableCollection<SoTietKiem>(ds);

        var dsTk = await accountService.LayDanhSachAsync(chiHoatDong: true);
        DanhSachTaiKhoan = new ObservableCollection<TaiKhoanThanhToan>(dsTk);
        if (DanhSachTaiKhoan.Count > 0 && TaiKhoanNguonChon is null)
            TaiKhoanNguonChon = DanhSachTaiKhoan[0];

        var dsKh = await customerService.TimKiemAsync(null);
        DanhSachKhachHang = new ObservableCollection<KhachHang>(dsKh);
        if (DanhSachKhachHang.Count > 0 && KhachHangChon is null)
            KhachHangChon = DanhSachKhachHang[0];

        CapNhatLaiSuatVaLai();
    });

    partial void OnKyHanThangChanged(int value) => CapNhatLaiSuatVaLai();
    partial void OnSoTienGuiChanged(double value) => CapNhatLaiSuatVaLai();

    private void CapNhatLaiSuatVaLai()
    {
        if (ChinhSach.LaiSuatTietKiem.TryGetValue(KyHanThang, out var ls))
            LaiSuatApDung = ls;
        TienLaiDuKien = ChinhSach.TinhLaiTietKiem(SangTien(SoTienGui), LaiSuatApDung, KyHanThang);
    }

    [RelayCommand]
    public void TaoMaSoNgauNhien()
    {
        var nam = DateTime.Today.Year;
        var ngauNhien = Random.Shared.Next(1000, 9999);
        FormMaSoSo = $"STK-{nam}-{ngauNhien}";
    }

    [RelayCommand]
    private void BatDauMoSo()
    {
        DangMoSo = true;
        GuiTienMat = false;
        TrichTaiKhoan = true;
        LaKhachHangHienHuu = true;
        LaKhachHangMoi = false;
        FormHoTen = string.Empty;
        FormCccd = string.Empty;
        FormSdt = string.Empty;
        FormDiaChi = string.Empty;
        SoTienGui = 50_000_000;
        KyHanThang = 12;
        TaoMaSoNgauNhien();
        CapNhatLaiSuatVaLai();
    }

    [RelayCommand]
    private void HuyMoSo() => DangMoSo = false;

    [RelayCommand]
    private Task XacNhanMoSoAsync() => ChayAsync(async () =>
    {
        var st = SangTien(SoTienGui);
        if (st < 1_000_000)
        {
            HienLoi("Số tiền gửi tiết kiệm tối thiểu là 1.000.000 đ.");
            return;
        }

        var maSoSo = string.IsNullOrWhiteSpace(FormMaSoSo) ? null : FormMaSoSo.Trim().ToUpper();

        if (GuiTienMat)
        {
            // Mở sổ tiền mặt tại quầy (không cần STK)
            if (LaKhachHangMoi)
            {
                if (string.IsNullOrWhiteSpace(FormHoTen))
                {
                    HienLoi("Vui lòng nhập họ tên người gửi tiền.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(FormCccd) || FormCccd.Trim().Length != 12)
                {
                    HienLoi("Số CCCD người gửi phải gồm đúng 12 chữ số.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(FormSdt) || FormSdt.Trim().Length != 10)
                {
                    HienLoi("Số điện thoại người gửi phải gồm 10 chữ số.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(FormDiaChi))
                {
                    HienLoi("Vui lòng nhập địa chỉ thường trú.");
                    return;
                }

                var kh = new KhachHang
                {
                    HoTenKhachHang = FormHoTen.Trim(),
                    SoCCCD = FormCccd.Trim(),
                    SoDienThoai = FormSdt.Trim(),
                    DiaChiThuongTru = FormDiaChi.Trim(),
                    NgaySinh = new DateTime(1990, 1, 1),
                    GioiTinh = GioiTinh.Nam,
                };

                var kq = await savingsService.MoSoKhachHangMoiTienMatAsync(kh, st, KyHanThang, customerService, maSoSo);
                HienKetQua(kq);
                if (kq.ThanhCong)
                {
                    DangMoSo = false;
                    await TaiDuLieuAsync();
                }
            }
            else
            {
                if (KhachHangChon is null)
                {
                    HienLoi("Vui lòng chọn khách hàng gửi tiền.");
                    return;
                }

                var kq = await savingsService.MoSoTienMatAsync(KhachHangChon.MaKhachHang, st, KyHanThang, maSoSo);
                HienKetQua(kq);
                if (kq.ThanhCong)
                {
                    DangMoSo = false;
                    await TaiDuLieuAsync();
                }
            }
        }
        else
        {
            // Trích từ tài khoản thanh toán
            if (TaiKhoanNguonChon is null)
            {
                HienLoi("Vui lòng chọn tài khoản nguồn.");
                return;
            }

            var kq = await savingsService.MoSoAsync(TaiKhoanNguonChon.MaTaiKhoan, st, KyHanThang, maSoSo);
            HienKetQua(kq);
            if (kq.ThanhCong)
            {
                DangMoSo = false;
                await TaiDuLieuAsync();
            }
        }
    });

    [RelayCommand]
    private async Task TatToanAsync(SoTietKiem? so)
    {
        if (so is null) return;
        var dungHan = DateTime.Today >= so.NgayDaoHan;
        var phuongThucNhan = so.TaiKhoanNguon is not null
            ? $"về tài khoản nguồn {so.TaiKhoanNguon.SoTaiKhoan}"
            : "bằng tiền mặt tại quầy";

        var canhBao = dungHan
            ? $"Sổ {so.MaSoSo} đã đến hạn tất toán. Toàn bộ tiền gốc và lãi sẽ chi trả {phuongThucNhan}."
            : $"Sổ {so.MaSoSo} CHƯA đến hạn (đáo hạn ngày {so.NgayDaoHan:dd/MM/yyyy}). Tất toán trước hạn sẽ chỉ được hưởng lãi không kỳ hạn ({ChinhSach.LaiSuatKhongKyHan}%/năm)! Tiền sẽ chi trả {phuongThucNhan}.";

        var xacNhan = await dialog.XacNhanAsync(
            "Xác nhận tất toán sổ tiết kiệm",
            canhBao,
            "Tất toán ngay");

        if (!xacNhan) return;

        await ChayAsync(async () =>
        {
            var kq = await savingsService.TatToanAsync(so.MaSoTietKiem);
            HienKetQua(kq);
            if (kq.ThanhCong) await TaiDuLieuAsync();
        });
    }
}
