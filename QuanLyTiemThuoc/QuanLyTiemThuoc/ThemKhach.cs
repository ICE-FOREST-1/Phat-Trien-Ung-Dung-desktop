using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyTiemThuoc
{
    public partial class ThemKhach : Form
    {
        // Danh sách khách hàng dùng chung, được MainForm truyền vào khi mở form này,
        // để form này thêm/sửa khách trực tiếp trên cùng 1 danh sách/1 file data.txt.
        private readonly DsKhachHang dsKhachHang;

        // Nếu khác null: form đang ở chế độ SỬA khách hàng đã tồn tại (không phải thêm mới).
        private readonly KhachHang khachHangCanSua;

        /// <summary>
        /// Mở form ở chế độ THÊM khách hàng mới.
        /// </summary>
        public ThemKhach(DsKhachHang dsKhachHang) : this(dsKhachHang, null)
        {
        }

        /// <summary>
        /// Mở form ở chế độ SỬA khách hàng đã tồn tại (khi khachHangCanSua khác null),
        /// hoặc THÊM khách hàng mới (khi khachHangCanSua là null).
        /// </summary>
        public ThemKhach(DsKhachHang dsKhachHang, KhachHang khachHangCanSua)
        {
            InitializeComponent();
            this.dsKhachHang = dsKhachHang;
            this.khachHangCanSua = khachHangCanSua;

            btLuu.Click += btLuu_Click;
            btHuy.Click += btHuy_Click;
            Load += ThemKhach_Load;
        }

        private void ThemKhach_Load(object sender, EventArgs e)
        {
            if (khachHangCanSua != null)
            {
                // Chế độ SỬA: khóa ô số điện thoại vì đây là khóa chính, không cho đổi
                Text = "Sửa Khách Hàng";
                txtNhapTen.Text = khachHangCanSua.HoTen;
                txtThemSDT.Text = khachHangCanSua.SoDienThoai;
                txtThemSDT.Enabled = false;
                btLuu.Text = "Lưu";
            }
            else
            {
                Text = "Thêm Khách Hàng Mới";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            string hoTen = txtNhapTen.Text.Trim();
            string soDienThoai = txtThemSDT.Text.Trim();

            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập họ tên khách hàng.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNhapTen.Focus();
                return;
            }

            if (string.IsNullOrEmpty(soDienThoai))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtThemSDT.Focus();
                return;
            }

            if (khachHangCanSua != null)
            {
                // ==== Chế độ SỬA ====
                bool thanhCongSua = dsKhachHang.SuaKhachHang(soDienThoai, hoTen);
                if (thanhCongSua)
                {
                    MessageBox.Show("Cập nhật thông tin khách hàng thành công.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật. Vui lòng thử lại.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            // ==== Chế độ THÊM MỚI ====
            if (dsKhachHang.DaTonTai(soDienThoai))
            {
                MessageBox.Show("Số điện thoại này đã được đăng ký cho khách hàng khác.",
                    "Trùng số điện thoại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtThemSDT.Focus();
                return;
            }

            bool thanhCong = dsKhachHang.ThemKhachHang(soDienThoai, hoTen);
            if (thanhCong)
            {
                MessageBox.Show("Thêm khách hàng thành công.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Không thể thêm khách hàng. Vui lòng kiểm tra lại thông tin.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btHuy_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
