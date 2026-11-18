using Microsoft.EntityFrameworkCore;
using NganHang.Core.Constants;
using NganHang.Core.Data;
using NganHang.Core.Entities;
using NganHang.Core.Security;

namespace NganHang.Core.Services;

/// <summary>Cảnh báo AML do trigger trong DB sinh ra; service chỉ đọc và xử lý kiểm duyệt.</summary>
public class AmlService(IDbContextFactory<CoreBankingDbContext> dbFactory, PhienLamViec phien)
{
    public async Task<List<CanhBaoAML>> LayDanhSachAsync(string? trangThai = null)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var q = db.CanhBaoAMLs.AsNoTracking().Include(c => c.GiaoDich).AsQueryable();
        if (!string.IsNullOrEmpty(trangThai)) q = q.Where(c => c.TrangThaiXuLy == trangThai);
        return await q.OrderBy(c => c.TrangThaiXuLy == TrangThaiAml.ChoKiemDuyet ? 0 : 1)
                      .ThenByDescending(c => c.ThoiGianPhatHien).ToListAsync();
    }

    public Task<KetQua> DuyetAnToanAsync(int maCanhBao) => XuLyAsync(maCanhBao, TrangThaiAml.DaDuyetAnToan);

    /// <summary>Khóa (đóng băng) tài khoản phát sinh giao dịch đáng ngờ.</summary>
    public Task<KetQua> KhoaTaiKhoanAsync(int maCanhBao) => XuLyAsync(maCanhBao, TrangThaiAml.KhoaDongBang);

    private async Task<KetQua> XuLyAsync(int maCanhBao, string trangThaiMoi)
    {
        if (!phien.CoQuyen(ChucNang.XuLyAml)) return KetQua.Loi("Bạn không có quyền xử lý cảnh báo AML.");
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            await using var tx = await db.Database.BeginTransactionAsync();
            var cb = await db.CanhBaoAMLs.FindAsync(maCanhBao);
            if (cb is null) return KetQua.Loi("Không tìm thấy cảnh báo.");
            if (cb.TrangThaiXuLy != TrangThaiAml.ChoKiemDuyet) return KetQua.Loi("Cảnh báo này đã được xử lý.");

            cb.TrangThaiXuLy = trangThaiMoi;
            cb.NguoiKiemDuyet = phien.TenHienThi;
            cb.ThoiGianKiemDuyet = DateTime.Now;

            var thongBao = $"Cảnh báo #{cb.MaCanhBao}: {HienThi.Ma(trangThaiMoi)}.";
            if (trangThaiMoi == TrangThaiAml.KhoaDongBang)
            {
                var tk = await db.TaiKhoans.FirstOrDefaultAsync(t => t.SoTaiKhoan == cb.SoTaiKhoanPhatSinh);
                if (tk is not null)
                {
                    tk.TrangThaiTaiKhoan = TrangThaiTaiKhoan.TamKhoa;
                    thongBao += $" Tài khoản {tk.SoTaiKhoan} đã bị tạm khóa.";
                }
            }
            await db.SaveChangesAsync();
            await tx.CommitAsync();
            return KetQua.Ok(thongBao);
        }
        catch (Exception ex)
        {
            return KetQua.Loi(LoiHeThong.ThongBao(ex));
        }
    }
}
