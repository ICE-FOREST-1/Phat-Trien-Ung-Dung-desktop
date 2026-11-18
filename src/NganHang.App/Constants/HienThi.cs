namespace NganHang.Core.Constants;

/// <summary>
/// Chuyển mã danh mục (lưu trong DB) thành nhãn tiếng Việt để hiển thị.
/// Mọi nhãn hiển thị được tập trung tại đây để dễ chỉnh sửa.
/// </summary>
public static class HienThi
{
    private static readonly Dictionary<string, string> Nhan = new(StringComparer.OrdinalIgnoreCase)
    {
        // Vai trò
        [VaiTro.GiaoDichVien] = "Giao dịch viên",
        [VaiTro.QuanTriVien] = "Quản trị viên / Giám đốc",
        [VaiTro.KiemSoatVien] = "Kiểm soát rủi ro & AML",
        [VaiTro.ChuyenVienTinDung] = "Chuyên viên tín dụng",

        // Hạng hội viên
        [HangHoiVien.Chuan] = "Chuẩn",
        [HangHoiVien.Bac] = "Bạc",
        [HangHoiVien.Vang] = "Vàng",
        [HangHoiVien.BachKim] = "Bạch Kim",
        [HangHoiVien.KimCuong] = "Kim Cương",

        // Giới tính
        [GioiTinh.Nam] = "Nam",
        [GioiTinh.Nu] = "Nữ",
        [GioiTinh.Khac] = "Khác",

        // Trạng thái tài khoản
        [TrangThaiTaiKhoan.HoatDong] = "Hoạt động",
        [TrangThaiTaiKhoan.TamKhoa] = "Tạm khóa",
        [TrangThaiTaiKhoan.DongSo] = "Đã đóng",

        // Loại giao dịch
        [LoaiGiaoDich.NapTienMat] = "Nộp tiền mặt",
        [LoaiGiaoDich.RutTienMat] = "Rút tiền mặt",
        [LoaiGiaoDich.ChuyenKhoanNoiBo] = "Chuyển khoản nội bộ",
        [LoaiGiaoDich.ChuyenKhoanLienNganHang] = "Chuyển khoản liên ngân hàng",
        [LoaiGiaoDich.GiaiNganKhoanVay] = "Giải ngân khoản vay",
        [LoaiGiaoDich.ThanhToanGocLaiVay] = "Thanh toán gốc lãi vay",
        [LoaiGiaoDich.MoSoTietKiem] = "Mở sổ tiết kiệm",
        [LoaiGiaoDich.GuiThemTietKiem] = "Gửi thêm tiết kiệm",
        [LoaiGiaoDich.TatToanSoTietKiem] = "Tất toán sổ tiết kiệm",

        // Trạng thái giao dịch
        [TrangThaiGiaoDich.ThanhCong] = "Thành công",
        [TrangThaiGiaoDich.ThatBai] = "Thất bại",
        [TrangThaiGiaoDich.DangXuLy] = "Đang xử lý",

        // Sổ tiết kiệm
        [TrangThaiSoTietKiem.DangHoatDong] = "Đang hoạt động",
        [TrangThaiSoTietKiem.DaTatToan] = "Đã tất toán",
        [TrangThaiSoTietKiem.DaDaoHan] = "Đã đáo hạn",

        // Gói vay
        [GoiVay.TheChapSoTietKiem] = "Cầm cố sổ tiết kiệm",
        [GoiVay.VayMuaNhaBds] = "Vay mua nhà / BĐS",
        [GoiVay.TieuDungTinChap] = "Tiêu dùng tín chấp",
        [GoiVay.SanXuatKinhDoanh] = "Sản xuất kinh doanh",

        // Trạng thái khoản vay
        [TrangThaiKhoanVay.ChoPheDuyet] = "Chờ phê duyệt",
        [TrangThaiKhoanVay.DaDuyet] = "Đã duyệt",
        [TrangThaiKhoanVay.DangVay] = "Đang vay",
        [TrangThaiKhoanVay.TuChoi] = "Từ chối",
        [TrangThaiKhoanVay.DaTatToan] = "Đã tất toán",
        [TrangThaiKhoanVay.QuaHanNoXau] = "Quá hạn / Nợ xấu",

        // AML
        [TrangThaiAml.ChoKiemDuyet] = "Chờ kiểm duyệt",
        [TrangThaiAml.DaDuyetAnToan] = "Đã duyệt an toàn",
        [TrangThaiAml.KhoaDongBang] = "Đã khóa tài khoản",
        [MucDoRuiRo.Thap] = "Thấp",
        [MucDoRuiRo.TrungBinh] = "Trung bình",
        [MucDoRuiRo.Cao] = "Cao",
        [MucDoRuiRo.DacBietNghiemTrong] = "Đặc biệt nghiêm trọng",
    };

    /// <summary>Trả về nhãn tiếng Việt của một mã; nếu không có thì trả lại chính mã đó.</summary>
    public static string Ma(string? ma) =>
        string.IsNullOrEmpty(ma) ? string.Empty : Nhan.TryGetValue(ma, out var nhan) ? nhan : ma;
}
