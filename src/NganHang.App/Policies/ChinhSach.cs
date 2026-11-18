using NganHang.Core.Constants;

namespace NganHang.Core.Policies;

/// <summary>Thông tin một hạng hội viên (đồng bộ với trigger trg_TuDongThangHangHoiVien).</summary>
public sealed record ThongTinHang(string Ma, string Ten, decimal NguongTichLuy, decimal HanMucThauChi, decimal PhiChuyenKhoan, string UuDai);

/// <summary>
/// Chính sách nghiệp vụ dùng chung. Các con số phải khớp với trigger / stored procedure trong DB:
///  - Ngưỡng hạng & hạn mức thấu chi: trg_TuDongThangHangHoiVien
///  - Phí chuyển khoản: sp_ChuyenKhoanNoiBo
///  - Ngưỡng AML: trg_PhatHienGiaoDichDangNgoAML
/// </summary>
public static class ChinhSach
{
    public const decimal NguongCanhBaoAml = 400_000_000m;
    public const decimal PhiChuyenKhoanCoBan = 3_300m;
    public const decimal LaiSuatKhongKyHan = 0.20m; // %/năm

    public static readonly IReadOnlyList<ThongTinHang> CacHang =
    [
        new(HangHoiVien.Chuan,    "Chuẩn",     0m,             0m,             PhiChuyenKhoanCoBan, "Dịch vụ ngân hàng cơ bản"),
        new(HangHoiVien.Bac,      "Bạc",       20_000_000m,    10_000_000m,    PhiChuyenKhoanCoBan, "Thấu chi 10 triệu"),
        new(HangHoiVien.Vang,     "Vàng",      100_000_000m,   50_000_000m,    0m, "Miễn phí chuyển khoản, thấu chi 50 triệu"),
        new(HangHoiVien.BachKim,  "Bạch Kim",  500_000_000m,   200_000_000m,   0m, "Miễn phí chuyển khoản, thấu chi 200 triệu, ưu tiên quầy"),
        new(HangHoiVien.KimCuong, "Kim Cương", 2_000_000_000m, 1_000_000_000m, 0m, "Miễn phí trọn gói, thấu chi 1 tỷ, chuyên viên riêng"),
    ];

    /// <summary>Lãi suất tiết kiệm (%/năm) theo kỳ hạn (tháng). Kỳ hạn hợp lệ theo CHECK của DB.</summary>
    public static readonly IReadOnlyDictionary<int, decimal> LaiSuatTietKiem = new Dictionary<int, decimal>
    {
        [1] = 3.00m, [3] = 4.50m, [6] = 5.60m, [12] = 6.20m, [24] = 6.80m, [36] = 7.00m,
    };

    /// <summary>Lãi suất cho vay tham khảo (%/năm) theo gói vay.</summary>
    public static readonly IReadOnlyDictionary<string, decimal> LaiSuatVay = new Dictionary<string, decimal>
    {
        [GoiVay.TieuDungTinChap] = 11.50m,
        [GoiVay.TheChapSoTietKiem] = 7.50m,
        [GoiVay.VayMuaNhaBds] = 8.90m,
        [GoiVay.SanXuatKinhDoanh] = 9.50m,
    };

    public static decimal TinhPhiChuyenKhoan(string? hang) =>
        CacHang.FirstOrDefault(h => h.Ma == hang)?.PhiChuyenKhoan ?? PhiChuyenKhoanCoBan;

    /// <summary>Tiền lãi dự kiến khi gửi đủ kỳ hạn (lãi đơn, trả cuối kỳ).</summary>
    public static decimal TinhLaiTietKiem(decimal soTien, decimal laiSuatNam, int kyHanThang) =>
        Math.Round(soTien * laiSuatNam / 100m * kyHanThang / 12m, 0);

    /// <summary>Số tiền trả góp đều hằng tháng (gốc + lãi, phương pháp niên kim).</summary>
    public static decimal TinhTraGopHangThang(decimal soTien, decimal laiSuatNam, int soThang)
    {
        if (soThang <= 0) return 0;
        var r = (double)laiSuatNam / 100d / 12d;
        if (r == 0) return Math.Round(soTien / soThang, 0);
        var pmt = (double)soTien * r / (1 - Math.Pow(1 + r, -soThang));
        return Math.Round((decimal)pmt, 0);
    }

    /// <summary>Lịch trả nợ chi tiết theo phương pháp niên kim.</summary>
    public static IReadOnlyList<KyTraNo> LapLichTraNo(decimal soTien, decimal laiSuatNam, int soThang, DateTime ngayBatDau)
    {
        var ketQua = new List<KyTraNo>(soThang);
        var traHangThang = TinhTraGopHangThang(soTien, laiSuatNam, soThang);
        var conLai = soTien;
        for (var ky = 1; ky <= soThang; ky++)
        {
            var lai = Math.Round(conLai * laiSuatNam / 100m / 12m, 0);
            var goc = ky == soThang ? conLai : traHangThang - lai;
            conLai -= goc;
            ketQua.Add(new KyTraNo(ky, ngayBatDau.AddMonths(ky), goc, lai, goc + lai, Math.Max(conLai, 0)));
        }
        return ketQua;
    }
}

public sealed record KyTraNo(int Ky, DateTime NgayTra, decimal TienGoc, decimal TienLai, decimal TongTra, decimal DuNoConLai);
