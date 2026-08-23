using System;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Đại diện cho một phần quà trong danh sách đổi điểm (form DoiQua).
    /// Đây là danh sách quà cố định khai báo sẵn trong code (không lưu file),
    /// vì cửa hàng chỉ cần một "case" quà đơn giản để đổi điểm, không cần
    /// quản lý tồn kho hay lịch sử chỉnh sửa danh sách quà.
    /// Muốn thêm/sửa/xóa phần quà: chỉnh trực tiếp danh sách trong DoiQua.cs
    /// (danhSachQua).
    /// </summary>
    public class PhanQua
    {
        /// <summary>Tên phần quà, hiển thị trong danh sách chọn (comboBox1).</summary>
        public string Ten { get; set; }

        /// <summary>Số điểm cần có để đổi được phần quà này.</summary>
        public int DiemCanDoi { get; set; }

        public PhanQua()
        {
        }

        public PhanQua(string ten, int diemCanDoi)
        {
            Ten = ten;
            DiemCanDoi = diemCanDoi;
        }

        /// <summary>
        /// ComboBox hiển thị theo ToString() khi không set DisplayMember,
        /// nên override để phòng trường hợp binding không dùng DisplayMember.
        /// </summary>
        public override string ToString()
        {
            return Ten;
        }
    }
}
