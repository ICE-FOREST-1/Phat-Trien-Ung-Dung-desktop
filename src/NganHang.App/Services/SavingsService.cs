using Microsoft.EntityFrameworkCore;
using NganHang.Core.Constants;
using NganHang.Core.Data;
using NganHang.Core.Entities;
using NganHang.Core.Policies;

namespace NganHang.Core.Services;

public class SavingsService(IDbContextFactory<CoreBankingDbContext> dbFactory, PhienLamViec phien)
{
    public async Task<List<SoTietKiem>> LayDanhSachAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.SoTietKiems.AsNoTracking()
            .Include(s => s.KhachHang).Include(s => s.TaiKhoanNguon)
            .OrderByDescending(s => s.MaSoTietKiem).ToListAsync();
    }

    /// <summary>Mở sổ tiết kiệm: trích tiền từ tài khoản thanh toán, ghi bút toán MO_SO_TIET_KIEM.</summary>
    public async Task<KetQua> MoSoAsync(int maTaiKhoan, decimal soTien, int kyHanThang, string? maSoSoTuChon = null)
    {
        if (!ChinhSach.LaiSuatTietKiem.TryGetValue(kyHanThang, out var laiSuat))
            return KetQua.Loi("Kỳ hạn không hợp lệ.");
        if (soTien < 1_000_000) return KetQua.Loi("Số tiền gửi tối thiểu là 1.000.000 đ.");
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            await using var tx = await db.Database.BeginTransactionAsync();

            var (hopLe, maSo, loiMaSo) = await XacDinhMaSoAsync(db, maSoSoTuChon);
            if (!hopLe) return KetQua.Loi(loiMaSo!);

            var tk = await db.TaiKhoans.Include(t => t.KhachHang).FirstOrDefaultAsync(t => t.MaTaiKhoan == maTaiKhoan);
            if (tk is null) return KetQua.Loi("Không tìm thấy tài khoản nguồn.");
            if (tk.TrangThaiTaiKhoan != TrangThaiTaiKhoan.HoatDong) return KetQua.Loi("Tài khoản nguồn đang bị khóa.");
            if (tk.SoDuHienTai < soTien) return KetQua.Loi($"Số dư ({tk.SoDuHienTai:N0} đ) không đủ để mở sổ.");

            var soDuTruoc = tk.SoDuHienTai;
            tk.SoDuHienTai -= soTien;

            db.SoTietKiems.Add(new SoTietKiem
            {
                MaSoSo = maSo,
                MaTaiKhoanNguon = tk.MaTaiKhoan,
                MaKhachHang = tk.MaKhachHang,
                SoTienGocBanDau = soTien,
                SoTienHienTai = soTien,
                KyHanThang = kyHanThang,
                LaiSuatNam = laiSuat,
                NgayGui = DateTime.Today,
                NgayDaoHan = DateTime.Today.AddMonths(kyHanThang),
                TrangThaiSo = TrangThaiSoTietKiem.DangHoatDong,
            });

            db.GiaoDichs.Add(new GiaoDich
            {
                MaChungTu = MaChungTu.Tao("TK"),
                SoTaiKhoanNguon = tk.SoTaiKhoan,
                TenNguoiGui = tk.KhachHang!.HoTenKhachHang,
                SoTaiKhoanDich = maSo,
                TenNguoiNhan = "Quỹ tiết kiệm",
                LoaiGiaoDich = LoaiGiaoDich.MoSoTietKiem,
                SoTienGiaoDich = soTien,
                SoDuTruocGD = soDuTruoc,
                SoDuSauGD = tk.SoDuHienTai,
                NoiDungChuyenKhoan = $"Mở sổ {maSo} kỳ hạn {kyHanThang} tháng, lãi suất {laiSuat}%/năm",
                NhanVienThucHien = phien.TenHienThi,
            });

            await db.SaveChangesAsync();
            await tx.CommitAsync();
            return KetQua.Ok($"Mở sổ {maSo} thành công: {soTien:N0} đ, kỳ hạn {kyHanThang} tháng, lãi dự kiến {ChinhSach.TinhLaiTietKiem(soTien, laiSuat, kyHanThang):N0} đ.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi(LoiHeThong.ThongBao(ex));
        }
    }

    /// <summary>
    /// Mở sổ tiết kiệm nộp tiền mặt tại quầy (dành cho khách hàng chưa/không có tài khoản thanh toán).
    /// </summary>
    public async Task<KetQua> MoSoTienMatAsync(int maKhachHang, decimal soTien, int kyHanThang, string? maSoSoTuChon = null)
    {
        if (!ChinhSach.LaiSuatTietKiem.TryGetValue(kyHanThang, out var laiSuat))
            return KetQua.Loi("Kỳ hạn không hợp lệ.");
        if (soTien < 1_000_000) return KetQua.Loi("Số tiền gửi tối thiểu là 1.000.000 đ.");
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            await using var tx = await db.Database.BeginTransactionAsync();

            var (hopLe, maSo, loiMaSo) = await XacDinhMaSoAsync(db, maSoSoTuChon);
            if (!hopLe) return KetQua.Loi(loiMaSo!);

            var kh = await db.KhachHangs.FindAsync(maKhachHang);
            if (kh is null) return KetQua.Loi("Không tìm thấy thông tin khách hàng.");

            db.SoTietKiems.Add(new SoTietKiem
            {
                MaSoSo = maSo,
                MaTaiKhoanNguon = null,
                MaKhachHang = kh.MaKhachHang,
                SoTienGocBanDau = soTien,
                SoTienHienTai = soTien,
                KyHanThang = kyHanThang,
                LaiSuatNam = laiSuat,
                NgayGui = DateTime.Today,
                NgayDaoHan = DateTime.Today.AddMonths(kyHanThang),
                TrangThaiSo = TrangThaiSoTietKiem.DangHoatDong,
            });

            db.GiaoDichs.Add(new GiaoDich
            {
                MaChungTu = MaChungTu.Tao("TK"),
                SoTaiKhoanNguon = "QUAY_TIEN_MAT",
                TenNguoiGui = $"{kh.HoTenKhachHang} (Nộp tiền mặt tại quầy)",
                SoTaiKhoanDich = maSo,
                TenNguoiNhan = "Quỹ tiết kiệm Techcombank",
                LoaiGiaoDich = LoaiGiaoDich.MoSoTietKiem,
                SoTienGiaoDich = soTien,
                SoDuTruocGD = 0,
                SoDuSauGD = soTien,
                NoiDungChuyenKhoan = $"Mở sổ {maSo} nộp tiền mặt tại quầy, kỳ hạn {kyHanThang} tháng, lãi suất {laiSuat}%/năm",
                NhanVienThucHien = phien.TenHienThi,
            });

            await db.SaveChangesAsync();
            await tx.CommitAsync();
            return KetQua.Ok($"Mở sổ {maSo} (Tiền mặt) thành công: {soTien:N0} đ, kỳ hạn {kyHanThang} tháng, lãi dự kiến {ChinhSach.TinhLaiTietKiem(soTien, laiSuat, kyHanThang):N0} đ.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi(LoiHeThong.ThongBao(ex));
        }
    }

    /// <summary>
    /// Mở sổ tiết kiệm cho khách hàng mới nộp tiền mặt (tự tạo hồ sơ CIF trước, không cần mở tài khoản thanh toán).
    /// </summary>
    public async Task<KetQua> MoSoKhachHangMoiTienMatAsync(KhachHang khachHang, decimal soTien, int kyHanThang, CustomerService customerService, string? maSoSoTuChon = null)
    {
        var kqKh = await customerService.LuuAsync(khachHang);
        if (!kqKh.ThanhCong) return kqKh;

        return await MoSoTienMatAsync(khachHang.MaKhachHang, soTien, kyHanThang, maSoSoTuChon);
    }

    /// <summary>
    /// Tất toán sổ: đủ kỳ hạn hưởng lãi kỳ hạn; trước hạn hưởng lãi không kỳ hạn theo số ngày thực gửi.
    /// Hỗ trợ cả sổ trích tiền từ tài khoản và sổ nộp tiền mặt tại quầy.
    /// </summary>
    public async Task<KetQua> TatToanAsync(int maSoTietKiem)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            await using var tx = await db.Database.BeginTransactionAsync();

            var so = await db.SoTietKiems.Include(s => s.TaiKhoanNguon).ThenInclude(t => t!.KhachHang)
                .Include(s => s.KhachHang)
                .FirstOrDefaultAsync(s => s.MaSoTietKiem == maSoTietKiem);
            if (so is null) return KetQua.Loi("Không tìm thấy sổ tiết kiệm.");
            if (so.TrangThaiSo == TrangThaiSoTietKiem.DaTatToan) return KetQua.Loi("Sổ đã được tất toán trước đó.");
            if (so.DangTheChap == true) return KetQua.Loi("Sổ đang cầm cố cho khoản vay, không thể tất toán.");

            var dungHan = DateTime.Today >= so.NgayDaoHan;
            var tienLai = dungHan
                ? ChinhSach.TinhLaiTietKiem(so.SoTienHienTai, so.LaiSuatNam, so.KyHanThang)
                : Math.Round(so.SoTienHienTai * ChinhSach.LaiSuatKhongKyHan / 100m * (DateTime.Today - so.NgayGui).Days / 365m, 0);
            var tongNhan = so.SoTienHienTai + tienLai;

            so.TrangThaiSo = TrangThaiSoTietKiem.DaTatToan;
            so.LaiTichLuyTamTinh = tienLai;

            var tenKhach = so.KhachHang?.HoTenKhachHang ?? so.TaiKhoanNguon?.KhachHang?.HoTenKhachHang ?? "Khách hàng";

            if (so.TaiKhoanNguon is not null)
            {
                var tk = so.TaiKhoanNguon;
                var soDuTruoc = tk.SoDuHienTai;
                tk.SoDuHienTai += tongNhan;

                db.GiaoDichs.Add(new GiaoDich
                {
                    MaChungTu = MaChungTu.Tao("TT"),
                    SoTaiKhoanNguon = so.MaSoSo,
                    TenNguoiGui = "Quỹ tiết kiệm",
                    SoTaiKhoanDich = tk.SoTaiKhoan,
                    TenNguoiNhan = tenKhach,
                    LoaiGiaoDich = LoaiGiaoDich.TatToanSoTietKiem,
                    SoTienGiaoDich = tongNhan,
                    SoDuTruocGD = soDuTruoc,
                    SoDuSauGD = tk.SoDuHienTai,
                    NoiDungChuyenKhoan = $"Tất toán sổ {so.MaSoSo} ({(dungHan ? "đúng hạn" : "trước hạn")}), tiền lãi {tienLai:N0} đ",
                    NhanVienThucHien = phien.TenHienThi,
                });
            }
            else
            {
                // Nhận tiền mặt tại quầy
                db.GiaoDichs.Add(new GiaoDich
                {
                    MaChungTu = MaChungTu.Tao("TT"),
                    SoTaiKhoanNguon = so.MaSoSo,
                    TenNguoiGui = "Quỹ tiết kiệm",
                    SoTaiKhoanDich = "QUAY_TIEN_MAT",
                    TenNguoiNhan = $"{tenKhach} (Chi tiền mặt tại quầy)",
                    LoaiGiaoDich = LoaiGiaoDich.TatToanSoTietKiem,
                    SoTienGiaoDich = tongNhan,
                    SoDuTruocGD = so.SoTienHienTai,
                    SoDuSauGD = 0,
                    NoiDungChuyenKhoan = $"Tất toán sổ {so.MaSoSo} (chi tiền mặt tại quầy {(dungHan ? "đúng hạn" : "trước hạn")}), tiền lãi {tienLai:N0} đ",
                    NhanVienThucHien = phien.TenHienThi,
                });
            }

            await db.SaveChangesAsync();
            await tx.CommitAsync();

            var noiNhan = so.TaiKhoanNguon is not null ? $"về TK {so.TaiKhoanNguon.SoTaiKhoan}" : "bằng tiền mặt tại quầy";
            return KetQua.Ok($"Tất toán sổ {so.MaSoSo} {(dungHan ? "đúng hạn" : "trước hạn")}: gốc {so.SoTienHienTai:N0} đ + lãi {tienLai:N0} đ đã chi trả {noiNhan}.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi(LoiHeThong.ThongBao(ex));
        }
    }

    private static async Task<(bool HopLe, string MaSo, string? Loi)> XacDinhMaSoAsync(CoreBankingDbContext db, string? maSoSoTuChon)
    {
        if (!string.IsNullOrWhiteSpace(maSoSoTuChon))
        {
            var ma = maSoSoTuChon.Trim().ToUpper();
            if (ma.Length < 4)
                return (false, string.Empty, "Mã số sổ tiết kiệm phải có ít nhất 4 ký tự.");

            var tonTai = await db.SoTietKiems.AnyAsync(s => s.MaSoSo == ma);
            if (tonTai)
                return (false, string.Empty, $"Mã số sổ tiết kiệm '{ma}' đã tồn tại trong hệ thống. Vui lòng chọn mã khác hoặc bấm Tạo ngẫu nhiên.");

            return (true, ma, null);
        }

        var sinhMa = await TaoMaSoAsync(db);
        return (true, sinhMa, null);
    }

    private static async Task<string> TaoMaSoAsync(CoreBankingDbContext db)
    {
        var tienTo = $"STK-{DateTime.Today:yyyy}-";
        var dsMa = await db.SoTietKiems.Where(s => s.MaSoSo.StartsWith(tienTo)).Select(s => s.MaSoSo).ToListAsync();
        var max = dsMa.Select(m => int.TryParse(m[tienTo.Length..], out var n) ? n : 0).DefaultIfEmpty(0).Max();
        return $"{tienTo}{max + 1:D4}";
    }
}

/// <summary>Sinh mã chứng từ duy nhất cho bút toán.</summary>
internal static class MaChungTu
{
    public static string Tao(string tienTo) => $"{tienTo}{DateTime.Now:yyMMddHHmmssfff}{Random.Shared.Next(10, 99)}";
}
