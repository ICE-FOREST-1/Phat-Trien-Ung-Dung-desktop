using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NganHang.App.Services;
using NganHang.Core.Constants;
using NganHang.Core.Entities;
using NganHang.Core.Security;
using NganHang.Core.Services;

namespace NganHang.App.ViewModels;

public partial class AccountsViewModel(AccountService accountService, CustomerService customerService, PhienLamViec phien, DialogService dialog) : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<TaiKhoanThanhToan> danhSach = [];
    [ObservableProperty] private ObservableCollection<KhachHang> danhSachKhachHang = [];
    [ObservableProperty] private TaiKhoanThanhToan? taiKhoanChon;
    [ObservableProperty] private bool chiHienHoatDong;

    // Form mở tài khoản
    [ObservableProperty] private bool dangMoTaiKhoan;
    [ObservableProperty] private bool laKhachHangMoi = true;
    [ObservableProperty] private bool laKhachHangHienHuu;
    [ObservableProperty] private KhachHang? khachHangChon;

    partial void OnLaKhachHangMoiChanged(bool value) => LaKhachHangHienHuu = !value;
    partial void OnLaKhachHangHienHuuChanged(bool value) => LaKhachHangMoi = !value;

    // Thông tin khách hàng mới
    [ObservableProperty] private string formHoTen = string.Empty;
    [ObservableProperty] private string formCccd = string.Empty;
    [ObservableProperty] private string formSdt = string.Empty;
    [ObservableProperty] private string formDiaChi = string.Empty;
    [ObservableProperty] private DateTimeOffset formNgaySinh = new(1995, 1, 1, 0, 0, 0, TimeSpan.Zero);
    [ObservableProperty] private LuaChon formGioiTinh = new(GioiTinh.Nam, "Nam");

    // Số tài khoản và số tiền nộp
    [ObservableProperty] private string formSoTaiKhoan = string.Empty;
    [ObservableProperty] private double soTienNapBanDau = 1_000_000;

    public bool DuocPhepMoTaiKhoan => phien.CoQuyen(ChucNang.MoTaiKhoan);
    public IReadOnlyList<LuaChon> DanhSachGioiTinh { get; } =
    [
        new(GioiTinh.Nam, "Nam"),
        new(GioiTinh.Nu, "Nữ"),
        new(GioiTinh.Khac, "Khác"),
    ];

    public override Task KhiMoTrangAsync(object? thamSo) => TaiDuLieuAsync();

    [RelayCommand]
    public Task TaiDuLieuAsync() => ChayAsync(async () =>
    {
        var ds = await accountService.LayDanhSachAsync(ChiHienHoatDong);
        DanhSach = new ObservableCollection<TaiKhoanThanhToan>(ds);

        var dsKh = await customerService.TimKiemAsync(null);
        DanhSachKhachHang = new ObservableCollection<KhachHang>(dsKh);
        if (DanhSachKhachHang.Count > 0 && KhachHangChon is null)
            KhachHangChon = DanhSachKhachHang[0];
    });

    [RelayCommand]
    private void BatDauMoTaiKhoan()
    {
        DangMoTaiKhoan = true;
        LaKhachHangMoi = true;
        FormHoTen = string.Empty;
        FormCccd = string.Empty;
        FormSdt = string.Empty;
        FormDiaChi = string.Empty;
        FormNgaySinh = new DateTimeOffset(1995, 1, 1, 0, 0, 0, TimeSpan.Zero);
        FormGioiTinh = DanhSachGioiTinh[0];
        SoTienNapBanDau = 1_000_000;
        TaoSoTaiKhoanNgauNhien();
    }

    [RelayCommand]
    public void TaoSoTaiKhoanNgauNhien()
    {
        var rnd = new Random();
        var duoi = rnd.NextInt64(1000000000L, 9999999999L);
        FormSoTaiKhoan = $"1903{duoi}";
    }

    [RelayCommand]
    private void HuyMoTaiKhoan() => DangMoTaiKhoan = false;

    [RelayCommand]
    private Task XacNhanMoTaiKhoanAsync() => ChayAsync(async () =>
    {
        var soTien = SangTien(SoTienNapBanDau);
        var soTk = string.IsNullOrWhiteSpace(FormSoTaiKhoan) ? null : FormSoTaiKhoan.Trim();

        if (LaKhachHangMoi)
        {
            if (string.IsNullOrWhiteSpace(FormHoTen))
            {
                HienLoi("Vui lòng nhập họ tên khách hàng.");
                return;
            }
            if (string.IsNullOrWhiteSpace(FormCccd) || FormCccd.Trim().Length != 12 || !FormCccd.Trim().All(char.IsDigit))
            {
                HienLoi("Số CCCD phải gồm đúng 12 chữ số.");
                return;
            }
            if (string.IsNullOrWhiteSpace(FormSdt) || FormSdt.Trim().Length != 10 || !FormSdt.Trim().StartsWith("0"))
            {
                HienLoi("Số điện thoại phải gồm 10 chữ số và bắt đầu bằng 0.");
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
                NgaySinh = FormNgaySinh.Date,
                GioiTinh = FormGioiTinh?.Ma ?? GioiTinh.Nam,
                NgheNghiep = "Kinh doanh tự do",
                ThuNhapHangThang = 20_000_000,
                DiemTinDung = 720,
            };

            var kq = await accountService.MoTaiKhoanKhachHangMoiAsync(kh, soTien, soTk, customerService);
            HienKetQua(kq);
            if (kq.ThanhCong)
            {
                DangMoTaiKhoan = false;
                await TaiDuLieuAsync();
            }
        }
        else
        {
            if (KhachHangChon is null)
            {
                HienLoi("Vui lòng chọn khách hàng hiện hữu.");
                return;
            }

            var kq = await accountService.MoTaiKhoanAsync(KhachHangChon.MaKhachHang, soTien, soTk);
            HienKetQua(kq);
            if (kq.ThanhCong)
            {
                DangMoTaiKhoan = false;
                await TaiDuLieuAsync();
            }
        }
    });

    [RelayCommand]
    private async Task KhoaHoacMoKhoaAsync(TaiKhoanThanhToan? tk)
    {
        if (tk is null) return;
        var trangThaiMoi = tk.TrangThaiTaiKhoan == TrangThaiTaiKhoan.HoatDong ? TrangThaiTaiKhoan.TamKhoa : TrangThaiTaiKhoan.HoatDong;
        var hanhDong = trangThaiMoi == TrangThaiTaiKhoan.TamKhoa ? "tạm khóa" : "mở khóa";

        var xacNhan = await dialog.XacNhanAsync(
            "Xác nhận thay đổi trạng thái",
            $"Bạn có chắc chắn muốn {hanhDong} tài khoản {tk.SoTaiKhoan} của khách hàng {tk.KhachHang?.HoTenKhachHang}?",
            "Đồng ý");

        if (!xacNhan) return;

        await ChayAsync(async () =>
        {
            var kq = await accountService.DoiTrangThaiAsync(tk.MaTaiKhoan, trangThaiMoi);
            HienKetQua(kq);
            if (kq.ThanhCong)
            {
                tk.TrangThaiTaiKhoan = trangThaiMoi;
                await TaiDuLieuAsync();
            }
        });
    }
}
