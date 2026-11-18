namespace NganHang.Core.Constants;

// Các mã danh mục khớp 1-1 với ràng buộc CHECK trong database CoSoDuLieuCoreBanking.
// Muốn thêm/sửa giá trị: cập nhật ràng buộc CHECK trong DB và hằng số tương ứng tại đây.

public static class VaiTro
{
    public const string GiaoDichVien = "GIAO_DICH_VIEN";
    public const string QuanTriVien = "QUAN_TRI_VIEN";
    public const string KiemSoatVien = "KIEM_SOAT_VIEN";
    public const string ChuyenVienTinDung = "CHUYEN_VIEN_TIN_DUNG";
}

public static class HangHoiVien
{
    public const string Chuan = "CHUAN";
    public const string Bac = "BAC";
    public const string Vang = "VANG";
    public const string BachKim = "BACH_KIM";
    public const string KimCuong = "KIM_CUONG";
}

public static class GioiTinh
{
    public const string Nam = "NAM";
    public const string Nu = "NU";
    public const string Khac = "KHAC";
    public static readonly string[] TatCa = [Nam, Nu, Khac];
}

public static class TrangThaiTaiKhoan
{
    public const string HoatDong = "HOAT_DONG";
    public const string TamKhoa = "TAM_KHOA";
    public const string DongSo = "DONG_SO";
}

public static class LoaiGiaoDich
{
    public const string NapTienMat = "NAP_TIEN_MAT";
    public const string RutTienMat = "RUT_TIEN_MAT";
    public const string ChuyenKhoanNoiBo = "CHUYEN_KHOAN_NOI_BO";
    public const string ChuyenKhoanLienNganHang = "CHUYEN_KHOAN_LIEN_NGAN_HANG";
    public const string GiaiNganKhoanVay = "GIAI_NGAN_KHOAN_VAY";
    public const string ThanhToanGocLaiVay = "THANH_TOAN_GOC_LAI_VAY";
    public const string MoSoTietKiem = "MO_SO_TIET_KIEM";
    public const string GuiThemTietKiem = "GUI_THEM_TIET_KIEM";
    public const string TatToanSoTietKiem = "TAT_TOAN_SO_TIET_KIEM";
}

public static class TrangThaiGiaoDich
{
    public const string ThanhCong = "THANH_CONG";
    public const string ThatBai = "THAT_BAI";
    public const string DangXuLy = "DANG_XU_LY";
}

public static class TrangThaiSoTietKiem
{
    public const string DangHoatDong = "DANG_HOAT_DONG";
    public const string DaTatToan = "DA_TAT_TOAN";
    public const string DaDaoHan = "DA_DAO_HAN";
}

public static class GoiVay
{
    public const string TheChapSoTietKiem = "THE_CHAP_SO_TIET_KIEM";
    public const string VayMuaNhaBds = "VAY_MUA_NHA_BDS";
    public const string TieuDungTinChap = "TIEU_DUNG_TIN_CHAP";
    public const string SanXuatKinhDoanh = "SAN_XUAT_KINH_DOANH";
    public static readonly string[] TatCa = [TieuDungTinChap, TheChapSoTietKiem, VayMuaNhaBds, SanXuatKinhDoanh];
}

public static class TrangThaiKhoanVay
{
    public const string ChoPheDuyet = "CHO_PHE_DUYET";
    public const string DaDuyet = "DA_DUYET";
    public const string DangVay = "DANG_VAY";
    public const string TuChoi = "TU_CHOI";
    public const string DaTatToan = "DA_TAT_TOAN";
    public const string QuaHanNoXau = "QUA_HAN_NO_XAU";
}

public static class TrangThaiAml
{
    public const string ChoKiemDuyet = "CHO_KIEM_DUYET";
    public const string DaDuyetAnToan = "DA_DUYET_AN_TOAN";
    public const string KhoaDongBang = "KHOA_DONG_BANG_TAI_KHOAN";
}

public static class MucDoRuiRo
{
    public const string Thap = "THAP";
    public const string TrungBinh = "TRUNG_BINH";
    public const string Cao = "CAO";
    public const string DacBietNghiemTrong = "DAC_BIET_NGHIEM_TRONG";
}
