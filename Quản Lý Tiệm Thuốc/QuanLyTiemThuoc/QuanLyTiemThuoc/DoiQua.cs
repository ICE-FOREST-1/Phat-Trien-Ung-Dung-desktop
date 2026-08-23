using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyTiemThuoc
{
    public partial class DoiQua : Form
    {
        private readonly KhachHang khachHang;
        private readonly DsKhachHang dsKhachHang;

        /// <summary>
        /// Danh sách phần quà cố định (case quà) để khách hàng đổi điểm.
        /// Không lưu file — muốn thêm/sửa/xóa quà thì chỉnh trực tiếp ở đây.
        /// </summary>
        private readonly List<PhanQua> danhSachQua = new List<PhanQua>
        {
            new PhanQua("Ly giữ nhiệt Tiệm Thuốc", 50),
            new PhanQua("Túi vải đựng thuốc", 80),
            new PhanQua("Hộp đựng thuốc cá nhân", 12),
            new PhanQua("Phiếu giảm giá 20.000đ",30),
            new PhanQua("Nhiệt kế điện tử", 20),
            new PhanQua("Phiếu giảm giá 50.000đ", 30),
            new PhanQua("Máy đo huyết áp mini", 80),
        };

        public DoiQua(KhachHang khachHang, DsKhachHang dsKhachHang)
        {
            InitializeComponent();

            this.khachHang = khachHang;
            this.dsKhachHang = dsKhachHang;

            Load += DoiQua_Load;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            btQuyDoi.Click += btQuyDoi_Click;
        }

        private void DoiQua_Load(object sender, EventArgs e)
        {
            lblKhachHang.Text = khachHang.HoTen + " - " + khachHang.SoDienThoai;
            CapNhatTongDiem();

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.DataSource = danhSachQua;
            comboBox1.DisplayMember = "Ten";
            comboBox1.ValueMember = "DiemCanDoi";

            HienThiSoDiemCanDoi();
        }

        /// <summary>
        /// Khi chọn 1 phần quà trong danh sách, tự động hiện số điểm cần đổi
        /// tương ứng của phần quà đó lên txtDiemCanDoi.
        /// </summary>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            HienThiSoDiemCanDoi();
        }

        private void HienThiSoDiemCanDoi()
        {
            PhanQua quaDangChon = comboBox1.SelectedItem as PhanQua;
            txtDiemCanDoi.Text = quaDangChon != null
                ? quaDangChon.DiemCanDoi + " điểm"
                : string.Empty;
        }

        private void CapNhatTongDiem()
        {
            lblTongDiem.Text = khachHang.DiemTichLuy + " điểm";
        }

        /// <summary>
        /// Thực hiện đổi quà: kiểm tra đủ điểm rồi trừ điểm tích lũy của khách hàng
        /// tương ứng với phần quà đã chọn. Không lưu lịch sử đổi quà.
        /// </summary>
        private void btQuyDoi_Click(object sender, EventArgs e)
        {
            PhanQua quaDangChon = comboBox1.SelectedItem as PhanQua;
            if (quaDangChon == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn một phần quà để đổi.",
                    "Chưa chọn quà",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (khachHang.DiemTichLuy < quaDangChon.DiemCanDoi)
            {
                MessageBox.Show(
                    $"Khách hàng không đủ điểm để đổi phần quà này.\n" +
                    $"Cần: {quaDangChon.DiemCanDoi} điểm - Hiện có: {khachHang.DiemTichLuy} điểm.",
                    "Không đủ điểm",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult xacNhan = MessageBox.Show(
                $"Xác nhận đổi \"{quaDangChon.Ten}\" ({quaDangChon.DiemCanDoi} điểm) " +
                $"cho khách hàng \"{khachHang.HoTen}\"?",
                "Xác nhận đổi quà",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes)
                return;

            // Trừ điểm tự động: CongDiem cộng dồn nên truyền số âm để trừ.
            bool thanhCong = dsKhachHang.CongDiem(khachHang.SoDienThoai, -quaDangChon.DiemCanDoi);

            if (!thanhCong)
            {
                MessageBox.Show(
                    "Có lỗi xảy ra khi trừ điểm. Vui lòng thử lại.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            CapNhatTongDiem();

            MessageBox.Show(
                $"Đổi quà \"{quaDangChon.Ten}\" thành công!\n" +
                $"Điểm còn lại: {khachHang.DiemTichLuy} điểm.",
                "Thành công",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
        }
    }
}
