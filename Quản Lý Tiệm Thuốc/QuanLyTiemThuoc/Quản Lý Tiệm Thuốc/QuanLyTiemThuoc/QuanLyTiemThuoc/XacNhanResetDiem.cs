using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Form cảnh báo trước khi reset điểm: hiển thị danh sách khách hàng đang
    /// có đủ điểm để đổi được ít nhất 1 phần quà, để admin cân nhắc thông báo
    /// cho khách đổi quà trước khi điểm bị đưa về 0.
    /// DialogResult.Yes = Tiếp tục Reset, DialogResult.No / Cancel = Hủy.
    /// </summary>
    public partial class XacNhanResetDiem : Form
    {
        public XacNhanResetDiem(List<KhachHang> danhSachDuDiem, int diemToiThieu)
        {
            InitializeComponent();

            Load += (s, e) => HienThiDanhSach(danhSachDuDiem, diemToiThieu);
            btTiepTuc.Click += (s, e) => { DialogResult = DialogResult.Yes; Close(); };
            btHuy.Click += (s, e) => { DialogResult = DialogResult.No; Close(); };
        }

        private void HienThiDanhSach(List<KhachHang> danhSach, int diemToiThieu)
        {
            dgvKhachHang.Rows.Clear();

            if (danhSach.Count == 0)
            {
                lblGhiChu.Text = "Không có khách hàng nào đang đủ điểm để đổi quà. Có thể tiến hành reset an toàn.";
                return;
            }

            lblGhiChu.Text =
                $"Có {danhSach.Count} khách hàng đang đủ điểm (>= {diemToiThieu} điểm) để đổi được ít nhất 1 phần quà.\n" +
                "Vui lòng cân nhắc thông báo cho khách đổi quà trước khi bấm \"Tiếp Tục Reset\".";

            foreach (KhachHang kh in danhSach)
            {
                dgvKhachHang.Rows.Add(kh.HoTen, kh.SoDienThoai, kh.DiemTichLuy);
            }
        }
    }
}
