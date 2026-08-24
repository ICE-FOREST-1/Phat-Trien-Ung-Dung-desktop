using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyTiemThuoc
{
    public partial class DoiQua : Form
    {
        private readonly KhachHang khachHang;
        private readonly DsKhachHang dsKhachHang;
        private readonly DsQua dsQua;
        private readonly DsVoucher dsVoucher;
        private readonly DsVoucherKhachHang dsVoucherKhachHang;
        private readonly DsGiaoDichDoiQua dsGiaoDichDoiQua;

        public DoiQua(KhachHang khachHang, DsKhachHang dsKhachHang, DsQua dsQua,
            DsVoucher dsVoucher, DsVoucherKhachHang dsVoucherKhachHang, DsGiaoDichDoiQua dsGiaoDichDoiQua)
        {
            InitializeComponent();

            this.khachHang = khachHang;
            this.dsKhachHang = dsKhachHang;
            this.dsQua = dsQua;
            this.dsVoucher = dsVoucher;
            this.dsVoucherKhachHang = dsVoucherKhachHang;
            this.dsGiaoDichDoiQua = dsGiaoDichDoiQua;

            Load += DoiQua_Load;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            btQuyDoi.Click += btQuyDoi_Click;
            btQuanLyQua.Click += btQuanLyQua_Click;
            btLichSuDoiQua.Click += btLichSuDoiQua_Click;
        }

        private void DoiQua_Load(object sender, EventArgs e)
        {
            lblKhachHang.Text = khachHang.HoTen + " - " + khachHang.SoDienThoai;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            CapNhatTongDiem();
            NapDanhSachQua();
        }

        /// <summary>
        /// Nạp lại danh sách quà: chỉ lấy những phần quà đang áp dụng cho THÁNG
        /// HIỆN TẠI, và trong số đó chỉ hiện những quà có số điểm cần đổi nhỏ hơn
        /// hoặc bằng số điểm khách hàng đang có.
        /// </summary>
        private void NapDanhSachQua()
        {
            int thangHienTai = DateTime.Now.Month;

            List<PhanQua> quaTrongThang = dsQua.LayTheoThang(thangHienTai);
            List<PhanQua> quaDuDieuKien = quaTrongThang
                .Where(q => q.DiemCanDoi <= khachHang.DiemTichLuy)
                .OrderBy(q => q.DiemCanDoi)
                .ToList();

            comboBox1.DataSource = null;
            comboBox1.Items.Clear();

            if (quaDuDieuKien.Count == 0)
            {
                comboBox1.Enabled = false;
                btQuyDoi.Enabled = false;
                lblThongBao.Text = quaTrongThang.Count == 0
                    ? $"Tháng {thangHienTai} hiện chưa có phần quà nào được kích hoạt."
                    : $"Khách hàng chưa đủ điểm để đổi phần quà nào trong tháng {thangHienTai}.";
                txtDiemCanDoi.Text = string.Empty;
                return;
            }

            comboBox1.Enabled = true;
            btQuyDoi.Enabled = true;
            lblThongBao.Text = string.Empty;

            comboBox1.DataSource = quaDuDieuKien;
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
        /// tương ứng với phần quà đã chọn.
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

            // Ghi lại lịch sử đổi quà (để form Lịch Sử Đổi Quà có dữ liệu hiển thị).
            dsGiaoDichDoiQua?.ThemGiaoDich(khachHang.SoDienThoai, quaDangChon.Ten, quaDangChon.DiemCanDoi);

            // Nếu phần quà này có gắn mã voucher -> CẤP RIÊNG voucher đó cho khách
            // hàng. Đây là cách DUY NHẤT để khách có voucher dùng khi mua hàng.
            string dongVoucherCap = string.Empty;
            if (!string.IsNullOrWhiteSpace(quaDangChon.MaVoucher) && dsVoucherKhachHang != null)
            {
                Voucher voucherGoc = dsVoucher?.TimTheoMa(quaDangChon.MaVoucher);
                if (voucherGoc != null)
                {
                    dsVoucherKhachHang.CapVoucher(khachHang.SoDienThoai, quaDangChon.MaVoucher);
                    dongVoucherCap = $"\nKhách hàng được cấp voucher: {voucherGoc.MoTa()} " +
                        "(chỉ dùng được khi mua hàng cho chính khách này).";
                }
            }

            CapNhatTongDiem();

            MessageBox.Show(
                $"Đổi quà \"{quaDangChon.Ten}\" thành công!\n" +
                $"Điểm còn lại: {khachHang.DiemTichLuy} điểm." + dongVoucherCap,
                "Thành công",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Nạp lại danh sách quà vì điểm khách hàng vừa thay đổi
            // (có thể không còn đủ điểm để đổi thêm quà khác nữa).
            NapDanhSachQua();

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Mở form quản lý quà (thêm/xóa quà, gán tháng áp dụng). Sau khi đóng,
        /// nạp lại danh sách quà để cập nhật ngay nếu vừa có thay đổi.
        /// </summary>
        private void btQuanLyQua_Click(object sender, EventArgs e)
        {
            using (QuanLyQua formQuanLyQua = new QuanLyQua(dsQua, dsVoucher))
            {
                formQuanLyQua.ShowDialog(this);
            }

            NapDanhSachQua();
        }

        /// <summary>
        /// Mở form Lịch Sử Đổi Quà cho khách hàng đang đổi quà.
        /// </summary>
        private void btLichSuDoiQua_Click(object sender, EventArgs e)
        {
            using (LichSuDoiQua formLichSu = new LichSuDoiQua(khachHang, dsGiaoDichDoiQua ?? new DsGiaoDichDoiQua()))
            {
                formLichSu.ShowDialog(this);
            }
        }
    }
}
