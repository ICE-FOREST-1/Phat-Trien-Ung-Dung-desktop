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
    public partial class MuaHang : Form
    {
        private readonly KhachHang khachHang;
        private readonly DsKhachHang dsKhachHang;
        private readonly DsGiaoDich dsGiaoDich;

        public MuaHang(KhachHang khachHang, DsKhachHang dsKhachHang, DsGiaoDich dsGiaoDich)
        {
            InitializeComponent();
            this.khachHang = khachHang;
            this.dsKhachHang = dsKhachHang;
            this.dsGiaoDich = dsGiaoDich;

            Load += MuaHang_Load;
            btThemGiaoDich.Click += btThemGiaoDich_Click;
        }

        private void MuaHang_Load(object sender, EventArgs e)
        {
            lblKhachHang.Text = khachHang.HoTen + " - " + khachHang.SoDienThoai;
            HienThiLichSu();
        }

        /// <summary>
        /// Đổ lịch sử giao dịch của khách hàng đang mua hàng lên dataGridView1.
        /// Thứ tự cột: clMaDG, clNgayMua, clNoiDung, clSoTien, clDiemNhan.
        /// </summary>
        private void HienThiLichSu()
        {
            dataGridView1.Rows.Clear();

            List<GiaoDich> lichSu = dsGiaoDich.LayDanhSachTheoSoDienThoai(khachHang.SoDienThoai);
            foreach (GiaoDich gd in lichSu)
            {
                dataGridView1.Rows.Add(
                    gd.MaGiaoDich,
                    gd.NgayMua.ToString("dd/MM/yyyy HH:mm"),
                    gd.NoiDung,
                    gd.SoLuong,
                    gd.SoTien.ToString("N0"),
                    gd.DiemNhan);
            }
        }

        private void btThemGiaoDich_Click(object sender, EventArgs e)
        {
            string noiDung = cbChon.Text.Trim();
            string chuoiSoTien = textBox1.Text.Trim();
            string chuoiSoLuong = txtSoLuong.Text.Trim();

            if (string.IsNullOrEmpty(noiDung))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập loại thuốc.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbChon.Focus();
                return;
            }

            decimal soTien;
            if (!decimal.TryParse(chuoiSoTien, out soTien) || soTien <= 0)
            {
                MessageBox.Show("Số tiền không hợp lệ. Vui lòng nhập một số lớn hơn 0.",
                    "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }
            int soLuong;
            if (!int.TryParse(chuoiSoLuong, out soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ. Vui lòng nhập số nguyên lớn hơn 0.",
                    "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }

            // 5. Tính tổng tiền = Đơn giá * Số lượng
            decimal tongTien = soTien * soLuong;

            GiaoDich giaoDichMoi = dsGiaoDich.ThemGiaoDich(khachHang.SoDienThoai, noiDung, tongTien, soLuong);
            if (giaoDichMoi == null)
            {
                MessageBox.Show("Không thể thêm giao dịch. Vui lòng kiểm tra lại thông tin.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cộng điểm tích lũy (5% giá trị hóa đơn) vào hồ sơ khách hàng
            dsKhachHang.CongDiem(khachHang.SoDienThoai, giaoDichMoi.DiemNhan);

            MessageBox.Show(
                $"Thêm giao dịch thành công. Khách hàng nhận thêm {giaoDichMoi.DiemNhan} điểm tích lũy.",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Xóa trắng ô nhập để chuẩn bị cho lần bán hàng tiếp theo
            cbChon.Text = string.Empty;
            textBox1.Text = string.Empty;
            HienThiLichSu();
        }

        
    }
}
