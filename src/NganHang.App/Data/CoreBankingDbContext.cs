using Microsoft.EntityFrameworkCore;
using NganHang.Core.Entities;

namespace NganHang.Core.Data;

/// <summary>
/// Ánh xạ EF Core vào database có sẵn CoSoDuLieuCoreBanking (database-first, không dùng migration).
/// </summary>
public class CoreBankingDbContext(DbContextOptions<CoreBankingDbContext> options) : DbContext(options)
{
    public DbSet<NguoiDung> NguoiDungs => Set<NguoiDung>();
    public DbSet<KhachHang> KhachHangs => Set<KhachHang>();
    public DbSet<TaiKhoanThanhToan> TaiKhoans => Set<TaiKhoanThanhToan>();
    public DbSet<SoTietKiem> SoTietKiems => Set<SoTietKiem>();
    public DbSet<TaiSanDamBao> TaiSanDamBaos => Set<TaiSanDamBao>();
    public DbSet<KhoanVay> KhoanVays => Set<KhoanVay>();
    public DbSet<GiaoDich> GiaoDichs => Set<GiaoDich>();
    public DbSet<CanhBaoAML> CanhBaoAMLs => Set<CanhBaoAML>();

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<decimal>().HavePrecision(18, 2);
        builder.Properties<decimal?>().HavePrecision(18, 2);
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<NguoiDung>(e =>
        {
            e.ToTable("NguoiDung");
            e.HasKey(x => x.MaNguoiDung);
            e.HasIndex(x => x.TenDangNhap).IsUnique();
        });

        mb.Entity<KhachHang>(e =>
        {
            e.ToTable("KhachHang");
            e.HasKey(x => x.MaKhachHang);
            e.HasIndex(x => x.MaCIF).IsUnique();
            e.HasIndex(x => x.SoCCCD).IsUnique();
            e.Property(x => x.NgaySinh).HasColumnType("date");
        });

        mb.Entity<TaiKhoanThanhToan>(e =>
        {
            e.ToTable("TaiKhoanThanhToan");
            e.HasKey(x => x.MaTaiKhoan);
            e.HasIndex(x => x.SoTaiKhoan).IsUnique();
            e.Property(x => x.LaiSuatKhongKyHan).HasPrecision(5, 2);
            e.HasOne(x => x.KhachHang).WithMany(k => k.TaiKhoans).HasForeignKey(x => x.MaKhachHang);
        });

        mb.Entity<SoTietKiem>(e =>
        {
            e.ToTable("SoTietKiem");
            e.HasKey(x => x.MaSoTietKiem);
            e.HasIndex(x => x.MaSoSo).IsUnique();
            e.Property(x => x.LaiSuatNam).HasPrecision(5, 2);
            e.Property(x => x.NgayGui).HasColumnType("date");
            e.Property(x => x.NgayDaoHan).HasColumnType("date");
            e.HasOne(x => x.KhachHang).WithMany(k => k.SoTietKiems).HasForeignKey(x => x.MaKhachHang);
            e.HasOne(x => x.TaiKhoanNguon).WithMany().HasForeignKey(x => x.MaTaiKhoanNguon);
        });

        mb.Entity<TaiSanDamBao>(e =>
        {
            e.ToTable("TaiSanDamBao");
            e.HasKey(x => x.MaTaiSan);
        });

        mb.Entity<KhoanVay>(e =>
        {
            e.ToTable("KhoanVay");
            e.HasKey(x => x.MaKhoanVay);
            e.HasIndex(x => x.MaSoHopDongVay).IsUnique();
            e.Property(x => x.LaiSuatVayNam).HasPrecision(5, 2);
            e.Property(x => x.NgayGiaiNgan).HasColumnType("date");
            e.Property(x => x.NgayDaoHanHopDong).HasColumnType("date");
            e.Property(x => x.NgayTraNoTiepTheo).HasColumnType("date");
            e.HasOne(x => x.KhachHang).WithMany(k => k.KhoanVays).HasForeignKey(x => x.MaKhachHang);
            e.HasOne(x => x.TaiKhoanGiaiNgan).WithMany().HasForeignKey(x => x.MaTaiKhoanGiaiNgan);
            e.HasOne(x => x.TaiSanDamBao).WithMany().HasForeignKey(x => x.MaTaiSanDamBao);
        });

        mb.Entity<GiaoDich>(e =>
        {
            // Bảng có trigger: phải khai báo để EF không dùng mệnh đề OUTPUT khi INSERT
            e.ToTable("GiaoDich", tb =>
            {
                tb.HasTrigger("trg_TuDongThangHangHoiVien");
                tb.HasTrigger("trg_PhatHienGiaoDichDangNgoAML");
            });
            e.HasKey(x => x.MaGiaoDich);
            e.HasIndex(x => x.MaChungTu).IsUnique();
        });

        mb.Entity<CanhBaoAML>(e =>
        {
            e.ToTable("CanhBaoAML");
            e.HasKey(x => x.MaCanhBao);
            e.HasOne(x => x.GiaoDich).WithMany().HasForeignKey(x => x.MaGiaoDichThamChieu);
        });
    }
}
