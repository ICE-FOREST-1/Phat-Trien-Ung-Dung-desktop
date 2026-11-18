using Microsoft.EntityFrameworkCore;
using NganHang.Core.Constants;
using NganHang.Core.Data;
using NganHang.Core.Entities;
using NganHang.Core.Policies;
using NganHang.Core.Security;

namespace NganHang.Core.Services;

public record HoSoVayMoi(int MaTaiKhoan, string GoiVay, decimal SoTien, int ThoiHanThang, decimal LaiSuatNam, string MucDich, int? MaTaiSanDamBao);

public class LoanService(IDbContextFactory<CoreBankingDbContext> dbFactory, PhienLamViec phien)
{
    public async Task<List<KhoanVay>> LayDanhSachAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.KhoanVays.AsNoTracking()
            .Include(k => k.KhachHang).Include(k => k.TaiKhoanGiaiNgan).Include(k => k.TaiSanDamBao)
            .OrderBy(k => k.TrangThaiKhoanVay == TrangThaiKhoanVay.ChoPheDuyet ? 0 : 1)
            .ThenByDescending(k => k.MaKhoanVay)
            .ToListAsync();
    }

    public async Task<List<TaiSanDamBao>> LayTaiSanDamBaoAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.TaiSanDamBaos.AsNoTracking().OrderBy(t => t.TenTaiSan).ToListAsync();
    }

    public async Task<KetQua> LapHoSoAsync(HoSoVayMoi hs)
    {
        if (!phien.CoQuyen(ChucNang.LapHoSoVay)) return KetQua.Loi("Bạn không có quyền lập hồ sơ vay.");
        if (hs.SoTien < 10_000_000) return KetQua.Loi("Số tiền vay tối thiểu là 10.000.000 đ.");
        if (hs.ThoiHanThang is < 1 or > 360) return KetQua.Loi("Thời hạn vay phải từ 1 đến 360 tháng.");
        if (hs.LaiSuatNam is <= 0 or > 50) return KetQua.Loi("Lãi suất không hợp lệ.");
        if (string.IsNullOrWhiteSpace(hs.MucDich)) return KetQua.Loi("Vui lòng nhập mục đích vay.");
        if (hs.GoiVay != GoiVay.TieuDungTinChap && hs.MaTaiSanDamBao is null)
            return KetQua.Loi("Gói vay này yêu cầu chọn tài sản đảm bảo.");
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var tk = await db.TaiKhoans.FindAsync(hs.MaTaiKhoan);
            if (tk is null) return KetQua.Loi("Không tìm thấy tài khoản giải ngân.");

            var dsMa = await db.KhoanVays.Select(k => k.MaSoHopDongVay).ToListAsync();
            var tienTo = $"HDV-{DateTime.Today:yyyy}-";
            var stt = dsMa.Where(m => m.StartsWith(tienTo)).Select(m => int.TryParse(m[tienTo.Length..], out var n) ? n : 0).DefaultIfEmpty(0).Max() + 1;

            var kv = new KhoanVay
            {
                MaSoHopDongVay = $"{tienTo}{stt:D4}",
                MaKhachHang = tk.MaKhachHang,
                MaTaiKhoanGiaiNgan = tk.MaTaiKhoan,
                MaTaiSanDamBao = hs.MaTaiSanDamBao,
                GoiVay = hs.GoiVay,
                SoTienVayBanDau = hs.SoTien,
                DuNoGocHienTai = hs.SoTien,
                ThoiHanVayThang = hs.ThoiHanThang,
                LaiSuatVayNam = hs.LaiSuatNam,
                NgayGiaiNgan = DateTime.Today,
                NgayDaoHanHopDong = DateTime.Today.AddMonths(hs.ThoiHanThang),
                NgayTraNoTiepTheo = DateTime.Today.AddMonths(1),
                SoTienPhaiTraHangThang = ChinhSach.TinhTraGopHangThang(hs.SoTien, hs.LaiSuatNam, hs.ThoiHanThang),
                TrangThaiKhoanVay = TrangThaiKhoanVay.ChoPheDuyet,
                MucDichVay = hs.MucDich.Trim(),
                NhanVienLapHoSo = phien.NguoiDung!.MaNguoiDung,
                NgayTao = DateTime.Now,
            };
            db.KhoanVays.Add(kv);
            await db.SaveChangesAsync();
            return KetQua.Ok($"Đã lập hồ sơ {kv.MaSoHopDongVay}, chờ lãnh đạo phê duyệt.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi(LoiHeThong.ThongBao(ex));
        }
    }

    /// <summary>Phê duyệt và giải ngân: cộng tiền vào tài khoản, ghi bút toán GIAI_NGAN_KHOAN_VAY.</summary>
    public async Task<KetQua> PheDuyetVaGiaiNganAsync(int maKhoanVay)
    {
        if (!phien.CoQuyen(ChucNang.PheDuyetVay)) return KetQua.Loi("Chỉ lãnh đạo (Quản trị viên) được phê duyệt khoản vay.");
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            await using var tx = await db.Database.BeginTransactionAsync();

            var kv = await db.KhoanVays.Include(k => k.TaiKhoanGiaiNgan).ThenInclude(t => t!.KhachHang)
                .FirstOrDefaultAsync(k => k.MaKhoanVay == maKhoanVay);
            if (kv is null) return KetQua.Loi("Không tìm thấy hồ sơ vay.");
            if (kv.TrangThaiKhoanVay != TrangThaiKhoanVay.ChoPheDuyet) return KetQua.Loi("Hồ sơ không ở trạng thái chờ phê duyệt.");

            var tk = kv.TaiKhoanGiaiNgan!;
            if (tk.TrangThaiTaiKhoan != TrangThaiTaiKhoan.HoatDong) return KetQua.Loi("Tài khoản giải ngân đang bị khóa.");

            kv.TrangThaiKhoanVay = TrangThaiKhoanVay.DangVay;
            kv.LanhDaoPheDuyet = phien.NguoiDung!.MaNguoiDung;
            kv.NgayPheDuyet = DateTime.Now;
            kv.NgayGiaiNgan = DateTime.Today;
            kv.NgayDaoHanHopDong = DateTime.Today.AddMonths(kv.ThoiHanVayThang);
            kv.NgayTraNoTiepTheo = DateTime.Today.AddMonths(1);

            var soDuTruoc = tk.SoDuHienTai;
            tk.SoDuHienTai += kv.SoTienVayBanDau;

            db.GiaoDichs.Add(new GiaoDich
            {
                MaChungTu = MaChungTu.Tao("GN"),
                SoTaiKhoanNguon = "HOI_SO_GIAI_NGAN",
                TenNguoiGui = "Khối Tín dụng – Hội sở",
                SoTaiKhoanDich = tk.SoTaiKhoan,
                TenNguoiNhan = tk.KhachHang!.HoTenKhachHang,
                LoaiGiaoDich = LoaiGiaoDich.GiaiNganKhoanVay,
                SoTienGiaoDich = kv.SoTienVayBanDau,
                SoDuTruocGD = soDuTruoc,
                SoDuSauGD = tk.SoDuHienTai,
                NoiDungChuyenKhoan = $"Giải ngân hợp đồng {kv.MaSoHopDongVay}",
                NhanVienThucHien = phien.TenHienThi,
            });

            await db.SaveChangesAsync();
            await tx.CommitAsync();
            return KetQua.Ok($"Đã phê duyệt và giải ngân {kv.SoTienVayBanDau:N0} đ cho hợp đồng {kv.MaSoHopDongVay}.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi(LoiHeThong.ThongBao(ex));
        }
    }

    public async Task<KetQua> TuChoiAsync(int maKhoanVay, string lyDo)
    {
        if (!phien.CoQuyen(ChucNang.PheDuyetVay)) return KetQua.Loi("Chỉ lãnh đạo (Quản trị viên) được từ chối khoản vay.");
        if (string.IsNullOrWhiteSpace(lyDo)) return KetQua.Loi("Vui lòng nhập lý do từ chối.");
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var kv = await db.KhoanVays.FindAsync(maKhoanVay);
            if (kv is null) return KetQua.Loi("Không tìm thấy hồ sơ vay.");
            if (kv.TrangThaiKhoanVay != TrangThaiKhoanVay.ChoPheDuyet) return KetQua.Loi("Hồ sơ không ở trạng thái chờ phê duyệt.");
            kv.TrangThaiKhoanVay = TrangThaiKhoanVay.TuChoi;
            kv.LyDoTuChoi = lyDo.Trim();
            kv.LanhDaoPheDuyet = phien.NguoiDung!.MaNguoiDung;
            kv.NgayPheDuyet = DateTime.Now;
            await db.SaveChangesAsync();
            return KetQua.Ok($"Đã từ chối hồ sơ {kv.MaSoHopDongVay}.");
        }
        catch (Exception ex)
        {
            return KetQua.Loi(LoiHeThong.ThongBao(ex));
        }
    }
}
