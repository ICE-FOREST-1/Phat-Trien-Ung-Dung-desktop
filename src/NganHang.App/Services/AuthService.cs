using Microsoft.EntityFrameworkCore;
using NganHang.Core.Data;
using NganHang.Core.Entities;

namespace NganHang.Core.Services;

public class AuthService(IDbContextFactory<CoreBankingDbContext> dbFactory, PhienLamViec phien)
{
    /// <summary>Mật khẩu mặc định cấp cho tài khoản có mật khẩu chưa được băm BCrypt.</summary>
    public const string MatKhauMacDinh = "123456";

    /// <summary>Kiểm tra kết nối tới SQL Server.</summary>
    public async Task<KetQua> KiemTraKetNoiAsync(CancellationToken ct = default)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync(ct);
            return await db.Database.CanConnectAsync(ct)
                ? KetQua.Ok($"Đã kết nối {db.Database.GetDbConnection().DataSource} / {db.Database.GetDbConnection().Database}")
                : KetQua.Loi("Không kết nối được tới SQL Server.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi(LoiHeThong.ThongBao(ex));
        }
    }

    /// <summary>
    /// Khởi tạo mật khẩu BCrypt cho các tài khoản đang lưu mật khẩu dạng cũ (không phải BCrypt).
    /// Chạy mỗi lần khởi động; tài khoản đã có BCrypt sẽ không bị ảnh hưởng.
    /// </summary>
    public async Task<int> KhoiTaoMatKhauAsync(CancellationToken ct = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var canCapNhat = await db.NguoiDungs.Where(u => !u.MatKhauMaHoa.StartsWith("$2")).ToListAsync(ct);
        foreach (var u in canCapNhat)
            u.MatKhauMaHoa = BCrypt.Net.BCrypt.HashPassword(MatKhauMacDinh);
        return await db.SaveChangesAsync(ct);
    }

    public async Task<KetQua> DangNhapAsync(string tenDangNhap, string matKhau)
    {
        if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            return KetQua.Loi("Vui lòng nhập tên đăng nhập và mật khẩu.");
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var user = await db.NguoiDungs.FirstOrDefaultAsync(u => u.TenDangNhap == tenDangNhap.Trim());
            if (user is null || !user.MatKhauMaHoa.StartsWith("$2") || !BCrypt.Net.BCrypt.Verify(matKhau, user.MatKhauMaHoa))
                return KetQua.Loi("Tên đăng nhập hoặc mật khẩu không đúng.");
            if (user.TrangThaiKichHoat == false)
                return KetQua.Loi("Tài khoản đã bị vô hiệu hóa. Liên hệ quản trị viên.");

            user.DangNhapLanCuoi = DateTime.Now;
            await db.SaveChangesAsync();
            phien.BatDau(user);
            return KetQua.Ok($"Xin chào {user.HoTen}!");
        }
        catch (Exception ex)
        {
            return KetQua.Loi(LoiHeThong.ThongBao(ex));
        }
    }

    public async Task<KetQua> DoiMatKhauAsync(string matKhauCu, string matKhauMoi)
    {
        if (phien.NguoiDung is null) return KetQua.Loi("Chưa đăng nhập.");
        if (matKhauMoi.Length < 6) return KetQua.Loi("Mật khẩu mới phải có ít nhất 6 ký tự.");
        await using var db = await dbFactory.CreateDbContextAsync();
        var user = await db.NguoiDungs.FindAsync(phien.NguoiDung.MaNguoiDung);
        if (user is null || !BCrypt.Net.BCrypt.Verify(matKhauCu, user.MatKhauMaHoa))
            return KetQua.Loi("Mật khẩu hiện tại không đúng.");
        user.MatKhauMaHoa = BCrypt.Net.BCrypt.HashPassword(matKhauMoi);
        await db.SaveChangesAsync();
        return KetQua.Ok("Đã đổi mật khẩu thành công.");
    }

    public void DangXuat() => phien.KetThuc();

    public Task<List<NguoiDung>> LayDanhSachNguoiDungAsync() =>
        Task.Run(async () =>
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            return await db.NguoiDungs.AsNoTracking().OrderBy(u => u.MaNguoiDung).ToListAsync();
        });
}
