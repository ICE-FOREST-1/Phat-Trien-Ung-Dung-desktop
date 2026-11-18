using NganHang.Core.Constants;

namespace NganHang.Core.Security;

/// <summary>Các chức năng của hệ thống dùng để phân quyền.</summary>
public enum ChucNang
{
    TongQuan,
    KhachHang,
    ThemSuaKhachHang,
    MoTaiKhoan,
    TietKiem,
    TinDung,
    LapHoSoVay,
    PheDuyetVay,
    ChuyenKhoan,
    NhatKyGiaoDich,
    Aml,
    XuLyAml,
    HangHoiVien,
    CaiDat,
}

/// <summary>
/// Ma trận phân quyền tập trung: chức năng -> danh sách vai trò được phép.
/// Thêm vai trò / đổi quyền chỉ cần sửa tại đây.
/// </summary>
public static class PhanQuyen
{
    private static readonly string[] TatCa = [VaiTro.GiaoDichVien, VaiTro.QuanTriVien];

    private static readonly Dictionary<ChucNang, string[]> MaTran = new()
    {
        [ChucNang.TongQuan] = [VaiTro.QuanTriVien],
        [ChucNang.KhachHang] = TatCa,
        [ChucNang.ThemSuaKhachHang] = [VaiTro.GiaoDichVien, VaiTro.QuanTriVien],
        [ChucNang.MoTaiKhoan] = [VaiTro.GiaoDichVien, VaiTro.QuanTriVien],
        [ChucNang.TietKiem] = [VaiTro.GiaoDichVien, VaiTro.QuanTriVien],
        [ChucNang.TinDung] = [VaiTro.GiaoDichVien, VaiTro.QuanTriVien],
        [ChucNang.LapHoSoVay] = [VaiTro.GiaoDichVien, VaiTro.QuanTriVien],
        [ChucNang.PheDuyetVay] = [VaiTro.QuanTriVien],
        [ChucNang.ChuyenKhoan] = [VaiTro.GiaoDichVien, VaiTro.QuanTriVien],
        [ChucNang.NhatKyGiaoDich] = TatCa,
        [ChucNang.Aml] = [VaiTro.QuanTriVien],
        [ChucNang.XuLyAml] = [VaiTro.QuanTriVien],
        [ChucNang.HangHoiVien] = TatCa,
        [ChucNang.CaiDat] = TatCa,
    };

    public static bool CoQuyen(string? vaiTro, ChucNang chucNang) =>
        vaiTro is not null && MaTran.TryGetValue(chucNang, out var ds) && ds.Contains(vaiTro);
}
