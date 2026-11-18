using Microsoft.Data.SqlClient;
using NganHang.Core.Entities;
using NganHang.Core.Security;

namespace NganHang.Core.Services;

/// <summary>Kết quả trả về của một thao tác nghiệp vụ.</summary>
public record KetQua(bool ThanhCong, string ThongBao)
{
    public static KetQua Ok(string thongBao) => new(true, thongBao);
    public static KetQua Loi(string thongBao) => new(false, thongBao);
}

/// <summary>Phiên đăng nhập hiện tại (singleton).</summary>
public class PhienLamViec
{
    public NguoiDung? NguoiDung { get; private set; }

    public bool DaDangNhap => NguoiDung is not null;
    public string TenHienThi => NguoiDung?.HoTen ?? "Hệ thống";

    public event EventHandler? DaThayDoi;

    public void BatDau(NguoiDung nguoiDung)
    {
        NguoiDung = nguoiDung;
        DaThayDoi?.Invoke(this, EventArgs.Empty);
    }

    public void KetThuc()
    {
        NguoiDung = null;
        DaThayDoi?.Invoke(this, EventArgs.Empty);
    }

    public bool CoQuyen(ChucNang chucNang) => PhanQuyen.CoQuyen(NguoiDung?.VaiTro, chucNang);
}

internal static class LoiHeThong
{
    /// <summary>Chuyển lỗi kỹ thuật thành thông báo dễ hiểu cho người dùng.</summary>
    public static string ThongBao(Exception ex) => ex switch
    {
        SqlException { Number: >= 50000 } sql => sql.Message,          // lỗi nghiệp vụ do THROW trong SP
        SqlException { Number: 2627 or 2601 } => "Dữ liệu bị trùng (mã CIF, CCCD, số tài khoản… đã tồn tại).",
        SqlException { Number: 547 } => "Dữ liệu không hợp lệ theo ràng buộc của cơ sở dữ liệu.",
        SqlException sql => $"Lỗi SQL Server: {sql.Message}",
        Microsoft.EntityFrameworkCore.DbUpdateException { InnerException: SqlException inner } => ThongBao(inner),
        _ => ex.Message,
    };
}
