using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using NganHang.Core.Data;
using NganHang.Core.Entities;

namespace NganHang.Core.Services;

public partial class CustomerService(IDbContextFactory<CoreBankingDbContext> dbFactory)
{
    /// <summary>Tìm khách hàng theo tên, CIF, CCCD hoặc số điện thoại.</summary>
    public async Task<List<KhachHang>> TimKiemAsync(string? tuKhoa)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var q = db.KhachHangs.AsNoTracking().Include(k => k.TaiKhoans).AsQueryable();
        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var t = tuKhoa.Trim();
            q = q.Where(k => k.HoTenKhachHang.Contains(t) || k.MaCIF.Contains(t) || k.SoCCCD.Contains(t) || k.SoDienThoai.Contains(t));
        }
        return await q.OrderBy(k => k.MaKhachHang).ToListAsync();
    }

    /// <summary>Hồ sơ khách hàng 360°: tài khoản, sổ tiết kiệm, khoản vay.</summary>
    public async Task<KhachHang?> LayChiTietAsync(int maKhachHang)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.KhachHangs.AsNoTracking()
            .Include(k => k.TaiKhoans)
            .Include(k => k.SoTietKiems)
            .Include(k => k.KhoanVays)
            .AsSplitQuery()
            .FirstOrDefaultAsync(k => k.MaKhachHang == maKhachHang);
    }

    public async Task<KetQua> LuuAsync(KhachHang kh)
    {
        var loi = KiemTra(kh);
        if (loi is not null) return KetQua.Loi(loi);
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            if (kh.MaKhachHang == 0)
            {
                kh.MaCIF = await TaoMaCifAsync(db);
                kh.NgayGiaNhap = DateTime.Now;
                db.KhachHangs.Add(kh);
                await db.SaveChangesAsync();
                return KetQua.Ok($"Đã thêm khách hàng {kh.HoTenKhachHang} – mã CIF {kh.MaCIF}.");
            }

            var goc = await db.KhachHangs.FindAsync(kh.MaKhachHang);
            if (goc is null) return KetQua.Loi("Không tìm thấy khách hàng.");
            goc.HoTenKhachHang = kh.HoTenKhachHang.Trim();
            goc.SoCCCD = kh.SoCCCD.Trim();
            goc.SoDienThoai = kh.SoDienThoai.Trim();
            goc.Email = kh.Email?.Trim();
            goc.DiaChiThuongTru = kh.DiaChiThuongTru.Trim();
            goc.NgaySinh = kh.NgaySinh;
            goc.GioiTinh = kh.GioiTinh;
            goc.NgheNghiep = kh.NgheNghiep;
            goc.ThuNhapHangThang = kh.ThuNhapHangThang;
            goc.DiemTinDung = kh.DiemTinDung;
            await db.SaveChangesAsync();
            return KetQua.Ok($"Đã cập nhật hồ sơ {goc.HoTenKhachHang}.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi(LoiHeThong.ThongBao(ex));
        }
    }

    private static string? KiemTra(KhachHang kh)
    {
        if (string.IsNullOrWhiteSpace(kh.HoTenKhachHang)) return "Vui lòng nhập họ tên khách hàng.";
        if (!CccdRegex().IsMatch(kh.SoCCCD ?? "")) return "Số CCCD phải gồm đúng 12 chữ số.";
        if (!SdtRegex().IsMatch(kh.SoDienThoai ?? "")) return "Số điện thoại phải gồm 10 chữ số và bắt đầu bằng 0.";
        if (string.IsNullOrWhiteSpace(kh.DiaChiThuongTru)) return "Vui lòng nhập địa chỉ thường trú.";
        if (kh.NgaySinh > DateTime.Today.AddYears(-18)) return "Khách hàng phải đủ 18 tuổi.";
        if (kh.DiemTinDung is < 300 or > 850) return "Điểm tín dụng phải trong khoảng 300 – 850.";
        return null;
    }

    private static async Task<string> TaoMaCifAsync(CoreBankingDbContext db)
    {
        var dsMa = await db.KhachHangs.Select(k => k.MaCIF).ToListAsync();
        var max = dsMa.Select(m => int.TryParse(m.Replace("CIF", ""), out var n) ? n : 0).DefaultIfEmpty(100000).Max();
        return $"CIF{max + 1}";
    }

    [GeneratedRegex(@"^\d{12}$")] private static partial Regex CccdRegex();
    [GeneratedRegex(@"^0\d{9}$")] private static partial Regex SdtRegex();
}
