using Microsoft.EntityFrameworkCore;
using NganHang.Core.Constants;
using NganHang.Core.Data;
using NganHang.Core.Entities;

namespace NganHang.Core.Services;

public record DiemThongKeNgay(DateTime Ngay, decimal TongTien, int SoGiaoDich);

public record TongQuan(
    decimal TongSoDuThanhToan,
    decimal TongTienGuiTietKiem,
    decimal TongDuNoChoVay,
    int SoKhachHang,
    int SoTaiKhoan,
    int SoCanhBaoAmlCho,
    int SoHoSoVayCho,
    int SoGiaoDichHomNay,
    IReadOnlyList<DiemThongKeNgay> GiaoDich7Ngay,
    IReadOnlyDictionary<string, int> PhanBoHang,
    IReadOnlyList<GiaoDich> GiaoDichGanDay);

/// <summary>Tra cứu nhật ký giao dịch & số liệu tổng quan.</summary>
public class ReportService(IDbContextFactory<CoreBankingDbContext> dbFactory)
{
    public async Task<List<GiaoDich>> TraCuuGiaoDichAsync(string? tuKhoa, string? loai, DateTime? tuNgay, DateTime? denNgay, int toiDa = 500)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var q = db.GiaoDichs.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var t = tuKhoa.Trim();
            q = q.Where(g => g.MaChungTu.Contains(t) || g.SoTaiKhoanNguon.Contains(t) || g.SoTaiKhoanDich.Contains(t)
                          || g.TenNguoiGui.Contains(t) || g.TenNguoiNhan.Contains(t) || g.NoiDungChuyenKhoan.Contains(t));
        }
        if (!string.IsNullOrEmpty(loai)) q = q.Where(g => g.LoaiGiaoDich == loai);
        if (tuNgay is not null) q = q.Where(g => g.ThoiGianGiaoDich >= tuNgay.Value.Date);
        if (denNgay is not null) q = q.Where(g => g.ThoiGianGiaoDich < denNgay.Value.Date.AddDays(1));
        return await q.OrderByDescending(g => g.MaGiaoDich).Take(toiDa).ToListAsync();
    }

    public async Task<TongQuan> LayTongQuanAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var homNay = DateTime.Today;
        var tu7Ngay = homNay.AddDays(-6);

        var tongSoDu = await db.TaiKhoans.SumAsync(t => (decimal?)t.SoDuHienTai) ?? 0;
        var tongTienGui = await db.SoTietKiems.Where(s => s.TrangThaiSo == TrangThaiSoTietKiem.DangHoatDong)
                                              .SumAsync(s => (decimal?)s.SoTienHienTai) ?? 0;
        var tongDuNo = await db.KhoanVays.Where(k => k.TrangThaiKhoanVay == TrangThaiKhoanVay.DangVay || k.TrangThaiKhoanVay == TrangThaiKhoanVay.QuaHanNoXau)
                                         .SumAsync(k => (decimal?)k.DuNoGocHienTai) ?? 0;
        var soKh = await db.KhachHangs.CountAsync();
        var soTk = await db.TaiKhoans.CountAsync();
        var amlCho = await db.CanhBaoAMLs.CountAsync(c => c.TrangThaiXuLy == TrangThaiAml.ChoKiemDuyet);
        var vayCho = await db.KhoanVays.CountAsync(k => k.TrangThaiKhoanVay == TrangThaiKhoanVay.ChoPheDuyet);
        var gdHomNay = await db.GiaoDichs.CountAsync(g => g.ThoiGianGiaoDich >= homNay);

        var gd7 = await db.GiaoDichs.Where(g => g.ThoiGianGiaoDich >= tu7Ngay)
            .Select(g => new { g.ThoiGianGiaoDich, g.SoTienGiaoDich }).ToListAsync();
        var theoNgay = Enumerable.Range(0, 7).Select(i => tu7Ngay.AddDays(i)).Select(ngay =>
        {
            var trongNgay = gd7.Where(g => g.ThoiGianGiaoDich?.Date == ngay).ToList();
            return new DiemThongKeNgay(ngay, trongNgay.Sum(g => g.SoTienGiaoDich), trongNgay.Count);
        }).ToList();

        var phanBo = await db.KhachHangs.GroupBy(k => k.HangHoiVien ?? HangHoiVien.Chuan)
            .Select(g => new { g.Key, SoLuong = g.Count() }).ToDictionaryAsync(x => x.Key, x => x.SoLuong);

        var ganDay = await db.GiaoDichs.AsNoTracking().OrderByDescending(g => g.MaGiaoDich).Take(8).ToListAsync();

        return new TongQuan(tongSoDu, tongTienGui, tongDuNo, soKh, soTk, amlCho, vayCho, gdHomNay, theoNgay, phanBo, ganDay);
    }
}
