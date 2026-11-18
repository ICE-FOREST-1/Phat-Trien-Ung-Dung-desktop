using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NganHang.Core.Constants;
using NganHang.Core.Data;
using NganHang.Core.Entities;
using NganHang.Core.Policies;

namespace NganHang.Core.Services;

/// <summary>Tài khoản thanh toán & chuyển khoản – dùng stored procedure có sẵn trong DB.</summary>
public class AccountService(IDbContextFactory<CoreBankingDbContext> dbFactory, PhienLamViec phien)
{
    public async Task<List<TaiKhoanThanhToan>> LayDanhSachAsync(bool chiHoatDong = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var q = db.TaiKhoans.AsNoTracking().Include(t => t.KhachHang).AsQueryable();
        if (chiHoatDong) q = q.Where(t => t.TrangThaiTaiKhoan == TrangThaiTaiKhoan.HoatDong);
        return await q.OrderBy(t => t.KhachHang!.HoTenKhachHang).ToListAsync();
    }

    public async Task<TaiKhoanThanhToan?> TimTheoSoAsync(string soTaiKhoan)
    {
        if (string.IsNullOrWhiteSpace(soTaiKhoan)) return null;
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.TaiKhoans.AsNoTracking().Include(t => t.KhachHang)
            .FirstOrDefaultAsync(t => t.SoTaiKhoan == soTaiKhoan.Trim());
    }

    /// <summary>Mở tài khoản thanh toán qua sp_MoTaiKhoanThanhToan (hỗ trợ số tự chọn hoặc random).</summary>
    public async Task<KetQua> MoTaiKhoanAsync(int maKhachHang, decimal soTienNap, string? soTaiKhoanTuChon = null)
    {
        if (phien.NguoiDung is null) return KetQua.Loi("Phiên đăng nhập đã hết hạn.");
        if (soTienNap < 0) return KetQua.Loi("Số tiền nộp ban đầu không hợp lệ.");
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var pSoTk = new SqlParameter("@SoTaiKhoanMoi", SqlDbType.NVarChar, 20) { Direction = ParameterDirection.Output };
            var pTuChon = new SqlParameter("@SoTaiKhoanTuChon", SqlDbType.NVarChar, 20)
            {
                Value = string.IsNullOrWhiteSpace(soTaiKhoanTuChon) ? DBNull.Value : soTaiKhoanTuChon.Trim()
            };

            await db.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_MoTaiKhoanThanhToan @MaKhachHang, @SoTienNapBanDau, @MaNguoiDungThucHien, @SoTaiKhoanMoi OUTPUT, @SoTaiKhoanTuChon",
                new SqlParameter("@MaKhachHang", maKhachHang),
                new SqlParameter("@SoTienNapBanDau", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = soTienNap },
                new SqlParameter("@MaNguoiDungThucHien", phien.NguoiDung.MaNguoiDung),
                pSoTk,
                pTuChon);
            return KetQua.Ok($"Mở tài khoản thành công. Số tài khoản mới: {pSoTk.Value}");
        }
        catch (Exception ex)
        {
            return KetQua.Loi(LoiHeThong.ThongBao(ex));
        }
    }

    /// <summary>Mở tài khoản cho khách hàng mới: tạo khách hàng trước, sau đó mở tài khoản thanh toán.</summary>
    public async Task<KetQua> MoTaiKhoanKhachHangMoiAsync(KhachHang khachHang, decimal soTienNap, string? soTaiKhoanTuChon, CustomerService customerService)
    {
        var kqKh = await customerService.LuuAsync(khachHang);
        if (!kqKh.ThanhCong) return kqKh;

        return await MoTaiKhoanAsync(khachHang.MaKhachHang, soTienNap, soTaiKhoanTuChon);
    }

    public async Task<KetQua> DoiTrangThaiAsync(int maTaiKhoan, string trangThai)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var tk = await db.TaiKhoans.FindAsync(maTaiKhoan);
            if (tk is null) return KetQua.Loi("Không tìm thấy tài khoản.");
            tk.TrangThaiTaiKhoan = trangThai;
            if (trangThai == TrangThaiTaiKhoan.DongSo) tk.NgayDongTaiKhoan = DateTime.Now;
            await db.SaveChangesAsync();
            return KetQua.Ok($"Tài khoản {tk.SoTaiKhoan}: {HienThi.Ma(trangThai)}.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi(LoiHeThong.ThongBao(ex));
        }
    }

    /// <summary>Chuyển khoản nội bộ qua sp_ChuyenKhoanNoiBo (ACID, tính phí theo hạng, trigger AML).</summary>
    public async Task<KetQua> ChuyenKhoanAsync(string soTkNguon, string soTkDich, decimal soTien, string noiDung)
    {
        if (string.IsNullOrWhiteSpace(soTkNguon) || string.IsNullOrWhiteSpace(soTkDich))
            return KetQua.Loi("Vui lòng chọn tài khoản nguồn và nhập tài khoản thụ hưởng.");
        if (soTien <= 0) return KetQua.Loi("Số tiền chuyển phải lớn hơn 0.");
        if (string.IsNullOrWhiteSpace(noiDung)) noiDung = "Chuyển khoản";
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var pMa = new SqlParameter("@MaChungTu", SqlDbType.NVarChar, 30) { Direction = ParameterDirection.Output };
            var pPhi = new SqlParameter("@PhiGiaoDich", SqlDbType.Decimal) { Precision = 18, Scale = 2, Direction = ParameterDirection.Output };
            await db.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_ChuyenKhoanNoiBo @SoTaiKhoanNguon, @SoTaiKhoanDich, @SoTien, @NoiDung, @NhanVienThucHien, @MaChungTu OUTPUT, @PhiGiaoDich OUTPUT",
                new SqlParameter("@SoTaiKhoanNguon", soTkNguon.Trim()),
                new SqlParameter("@SoTaiKhoanDich", soTkDich.Trim()),
                new SqlParameter("@SoTien", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = soTien },
                new SqlParameter("@NoiDung", noiDung.Trim()),
                new SqlParameter("@NhanVienThucHien", phien.TenHienThi),
                pMa, pPhi);

            var phi = pPhi.Value is decimal d ? d : 0m;
            var thongBao = $"Chuyển khoản thành công {soTien:N0} đ (phí {phi:N0} đ). Mã chứng từ: {pMa.Value}";
            if (soTien >= ChinhSach.NguongCanhBaoAml)
                thongBao += $". Giao dịch ≥ {ChinhSach.NguongCanhBaoAml:N0} đ đã được chuyển bộ phận AML kiểm duyệt.";
            return KetQua.Ok(thongBao);
        }
        catch (Exception ex)
        {
            return KetQua.Loi(LoiHeThong.ThongBao(ex));
        }
    }
}
