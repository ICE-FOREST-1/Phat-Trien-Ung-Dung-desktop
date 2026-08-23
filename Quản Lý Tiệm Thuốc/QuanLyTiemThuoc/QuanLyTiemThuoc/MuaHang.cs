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
        private List<ChiTietDonHang> gioHang = new List<ChiTietDonHang>();

        public MuaHang(KhachHang khachHang, DsKhachHang dsKhachHang, DsGiaoDich dsGiaoDich)
        {
            InitializeComponent();
            this.khachHang = khachHang;
            this.dsKhachHang = dsKhachHang;
            this.dsGiaoDich = dsGiaoDich;

            Load += MuaHang_Load;
            btnThem.Click += BtnThem_Click;
            btThemGiaoDich.Click += BtThemGiaoDich_Click;
        }

        private void MuaHang_Load(object sender, EventArgs e)
        {
            lblKhachHang.Text = khachHang.HoTen + " - " + khachHang.SoDienThoai;
            HienThiLichSu();
            KhoiTaoDataGridView2();
        }

        /// <summary>
        /// Khởi tạo cấu hình cho dataGridView2 để hiển thị chi tiết hàng được thêm
        /// </summary>
        private void KhoiTaoDataGridView2()
        {
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.AllowUserToAddRows = false;
        }

        /// <summary>
        /// Đổ lịch sử giao dịch của khách hàng đang mua hàng lên dataGridView1.
        /// Thứ tự cột: clMaDG, clNgayMua, clNoiDung, clSoLuong, clSoTien, clDiemNhan.
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

        /// <summary>
        /// Sự kiện khi bấm nút "Thêm" - thêm mặt hàng vào dataGridView2 (giỏ hàng tạm)
        /// </summary>
        private void BtnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra loại thuốc
            string tenThuoc = cbChon.Text.Trim();
            if (string.IsNullOrEmpty(tenThuoc))
            {
                MessageBox.Show("Vui lòng chọn loại thuốc.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbChon.Focus();
                return;
            }

            // Kiểm tra số tiền (đơn giá)
            string chuoiSoTien = textBox1.Text.Trim();
            decimal donGia;
            if (!decimal.TryParse(chuoiSoTien, out donGia) || donGia <= 0)
            {
                MessageBox.Show("Số tiền không hợp lệ. Vui lòng nhập một số lớn hơn 0.",
                    "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            // Kiểm tra số lượng
            string chuoiSoLuong = txtSoLuong.Text.Trim();
            int soLuong;
            if (!int.TryParse(chuoiSoLuong, out soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ. Vui lòng nhập số nguyên lớn hơn 0.",
                    "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }

            // Tính tổng tiền
            decimal thanhTien = donGia * soLuong;

            // Thêm vào giỏ hàng tạm
            gioHang.Add(new ChiTietDonHang
            {
                TenThuoc = tenThuoc,
                SoLuong = soLuong,
                DonGia = donGia,
                ThanhTien = thanhTien
            });

            // Đổ lên dataGridView2
            HienThiGioHang();

            // Xóa trắng các ô nhập
            cbChon.Text = string.Empty;
            textBox1.Text = string.Empty;
            txtSoLuong.Text = string.Empty;
            cbChon.Focus();
        }

        /// <summary>
        /// Hiển thị danh sách giỏ hàng lên dataGridView2
        /// </summary>
        private void HienThiGioHang()
        {
            dataGridView2.Rows.Clear();

            foreach (var item in gioHang)
            {
                dataGridView2.Rows.Add(
                    item.TenThuoc,
                    item.SoLuong,
                    item.ThanhTien.ToString("N0"));
            }
        }

        /// <summary>
        /// Sự kiện khi bấm nút "Thanh Toán" - lưu lại tất cả các mặt hàng vào hóa đơn (dataGridView1)
        /// </summary>
        private void BtThemGiaoDich_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu giỏ hàng trống
            if (gioHang.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống. Vui lòng thêm ít nhất một mặt hàng.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Tính tổng giá trị hóa đơn
            decimal tongHoaDon = gioHang.Sum(x => x.ThanhTien);

            // Tạo chuỗi mô tả hóa đơn
            string moTaHoaDon = string.Join("; ", gioHang.Select(x => $"{x.TenThuoc} (SL: {x.SoLuong})"));

            // Tính tổng số lượng
            int tongSoLuong = gioHang.Sum(x => x.SoLuong);

            // Lưu giao dịch vào dữ liệu
            GiaoDich giaoDichMoi = dsGiaoDich.ThemGiaoDich(khachHang.SoDienThoai, moTaHoaDon, tongHoaDon, tongSoLuong);
            if (giaoDichMoi == null)
            {
                MessageBox.Show("Không thể lưu hóa đơn. Vui lòng kiểm tra lại thông tin.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cộng điểm tích lũy vào tài khoản khách hàng
            dsKhachHang.CongDiem(khachHang.SoDienThoai, giaoDichMoi.DiemNhan);

            // Hiển thị thông báo thành công
            MessageBox.Show(
                $"Thanh toán thành công!\n" +
                $"Tổng tiền: {tongHoaDon:N0} VNĐ\n" +
                $"Khách hàng nhận thêm {giaoDichMoi.DiemNhan} điểm tích lũy.",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Xóa giỏ hàng và cập nhật hiển thị
            gioHang.Clear();
            HienThiGioHang();
            HienThiLichSu();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Có thể thêm tính năng xóa dòng hoặc sửa sau này
        }
    }

    /// <summary>
    /// Class tạm thời để lưu chi tiết từng mặt hàng trong giỏ hàng
    /// </summary>
    public class ChiTietDonHang
    {
        public string TenThuoc { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
