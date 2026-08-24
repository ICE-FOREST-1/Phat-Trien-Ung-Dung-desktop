using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyTiemThuoc
{
    /// <summary>
    /// Form quản lý danh sách phần quà đổi điểm: xem, thêm, xóa quà và gán các
    /// tháng áp dụng cho từng phần quà. Dữ liệu lưu vào qua.txt qua lớp DsQua.
    /// </summary>
    public partial class QuanLyQua : Form
    {
        private readonly DsQua dsQua;
        private readonly DsVoucher dsVoucher;

        public QuanLyQua(DsQua dsQua, DsVoucher dsVoucher = null)
        {
            InitializeComponent();

            this.dsQua = dsQua;
            this.dsVoucher = dsVoucher;

            Load += QuanLyQua_Load;
            btThemQua.Click += BtThemQua_Click;
            btXoaQua.Click += BtXoaQua_Click;
            btDong.Click += (s, e) => Close();
        }

        private void QuanLyQua_Load(object sender, EventArgs e)
        {
            KhoiTaoDataGridView();
            HienThiDanhSach();
        }

        private void KhoiTaoDataGridView()
        {
            dgvQua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvQua.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvQua.MultiSelect = false;
            dgvQua.AllowUserToAddRows = false;
            dgvQua.ReadOnly = true;
        }

        private void HienThiDanhSach()
        {
            dgvQua.Rows.Clear();

            foreach (PhanQua qua in dsQua.LayDanhSach())
            {
                dgvQua.Rows.Add(qua.Ten, qua.DiemCanDoi, qua.ThangApDungToString(), qua.MaVoucher);
            }
        }

        /// <summary>
        /// Thêm 1 phần quà mới: kiểm tra tên, điểm, và danh sách tháng hợp lệ
        /// (các tháng cách nhau bởi dấu phẩy, VD: 9,10) rồi lưu vào qua.txt.
        /// </summary>
        private void BtThemQua_Click(object sender, EventArgs e)
        {
            string ten = txtTenQua.Text.Trim();
            if (string.IsNullOrEmpty(ten))
            {
                MessageBox.Show("Vui lòng nhập tên phần quà.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenQua.Focus();
                return;
            }

            int diemCanDoi;
            if (!int.TryParse(txtDiemQua.Text.Trim(), out diemCanDoi) || diemCanDoi <= 0)
            {
                MessageBox.Show("Số điểm cần đổi không hợp lệ. Vui lòng nhập số nguyên lớn hơn 0.",
                    "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiemQua.Focus();
                return;
            }

            List<int> cacThang = new List<int>();
            string[] phanThang = txtThangApDung.Text.Trim().Split(',');
            foreach (string phan in phanThang)
            {
                int thang;
                if (int.TryParse(phan.Trim(), out thang) && thang >= 1 && thang <= 12)
                {
                    if (!cacThang.Contains(thang))
                        cacThang.Add(thang);
                }
            }

            if (cacThang.Count == 0)
            {
                MessageBox.Show(
                    "Vui lòng nhập ít nhất 1 tháng áp dụng hợp lệ (1-12), cách nhau bởi dấu phẩy.\nVí dụ: 9,10",
                    "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtThangApDung.Focus();
                return;
            }

            // Mã voucher cấp kèm (không bắt buộc). Nếu có nhập, kiểm tra mã đó
            // phải tồn tại sẵn trong danh sách voucher của cửa hàng (voucher.txt)
            // thì khi đổi quà mới có voucher thật để cấp cho khách.
            string maVoucher = txtMaVoucher.Text.Trim();
            if (!string.IsNullOrEmpty(maVoucher) && dsVoucher != null && dsVoucher.TimTheoMa(maVoucher) == null)
            {
                MessageBox.Show(
                    $"Không tìm thấy voucher có mã \"{maVoucher}\" trong danh sách voucher của cửa hàng.\n" +
                    "Vui lòng để trống (nếu là quà vật lý) hoặc nhập đúng mã voucher đã có.",
                    "Sai mã voucher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaVoucher.Focus();
                return;
            }

            dsQua.ThemQua(new PhanQua(ten, diemCanDoi, cacThang, string.IsNullOrEmpty(maVoucher) ? null : maVoucher));
            HienThiDanhSach();

            txtTenQua.Text = string.Empty;
            txtDiemQua.Text = string.Empty;
            txtThangApDung.Text = string.Empty;
            txtMaVoucher.Text = string.Empty;
            txtTenQua.Focus();
        }

        /// <summary>
        /// Xóa phần quà đang được chọn trên lưới.
        /// </summary>
        private void BtXoaQua_Click(object sender, EventArgs e)
        {
            if (dgvQua.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một phần quà để xóa.", "Chưa chọn quà",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenDangChon = dgvQua.CurrentRow.Cells[0].Value?.ToString();
            PhanQua quaDangChon = dsQua.LayDanhSach().FirstOrDefault(q => q.Ten == tenDangChon);

            if (quaDangChon == null)
                return;

            DialogResult xacNhan = MessageBox.Show(
                $"Bạn có chắc muốn xóa phần quà \"{quaDangChon.Ten}\" không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes)
                return;

            dsQua.XoaQua(quaDangChon);
            HienThiDanhSach();
        }
    }
}
