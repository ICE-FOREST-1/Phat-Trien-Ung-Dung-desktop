using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyTiemThuoc
{
    public partial class MainForm : Form
    {
        // Danh sách khách hàng và danh sách giao dịch dùng chung cho toàn bộ MainForm.
        // Khởi tạo 1 lần, tự đọc data.txt / giaodich.txt khi form được mở.
        private DsKhachHang dsKhachHang;
        private DsGiaoDich dsGiaoDich;

        // Menu hiện ra khi bấm chuột phải vào 1 dòng khách hàng trên dataGridView1
        private ContextMenuStrip menuKhachHang;
        private ToolStripMenuItem mnuSuaKhachHang;
        private ToolStripMenuItem mnuXoaKhachHang;

        public MainForm()
        {
            InitializeComponent();

            KhoiTaoMenuChuotPhai();

            // Gắn các sự kiện tại đây (thay vì trong Designer.cs) để không bị
            // Visual Studio Designer ghi đè mỗi khi bạn chỉnh sửa giao diện.
            Load += MainForm_Load;
            btTimKiem.Click += btTimKiem_Click;
            btThem.Click += btThem_Click;
            btMuaHang.Click += btMuaHang_Click;
            btLichSuGiaoDich.Click += btLichSuGiaoDich_Click;
            btDoiThuong.Click += btDoiThuong_Click;
            btResetDiem.Click += btResetDiem_Click;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellMouseDown += dataGridView1_CellMouseDown;
        }

        /// <summary>
        /// Tạo menu chuột phải (Sửa/Xóa khách hàng) và gán cho dataGridView1.
        /// Không cần kéo thả ContextMenuStrip từ Toolbox — tạo bằng code cho gọn.
        /// </summary>
        private void KhoiTaoMenuChuotPhai()
        {
            mnuSuaKhachHang = new ToolStripMenuItem("Sửa khách hàng");
            mnuXoaKhachHang = new ToolStripMenuItem("Xóa khách hàng");

            mnuSuaKhachHang.Click += mnuSuaKhachHang_Click;
            mnuXoaKhachHang.Click += mnuXoaKhachHang_Click;

            menuKhachHang = new ContextMenuStrip();
            menuKhachHang.Items.Add(mnuSuaKhachHang);
            menuKhachHang.Items.Add(mnuXoaKhachHang);

            dataGridView1.ContextMenuStrip = menuKhachHang;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            dsKhachHang = new DsKhachHang();
            dsGiaoDich = new DsGiaoDich();

            // LƯU Ý: việc reset điểm đầu năm KHÔNG còn tự động chạy khi mở form nữa.
            // Chỉ thực hiện khi người dùng bấm nút "Reset Đầu Năm" (xem btResetDiem_Click).

            // Không cho sửa trực tiếp trên ô lưới — mọi thay đổi phải đi qua
            // menu chuột phải (Sửa/Xóa) hoặc form ThemKhach để đảm bảo dữ liệu
            // luôn được kiểm tra hợp lệ và ghi đúng xuống data.txt.
            dataGridView1.ReadOnly = true;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            HienThiDanhSach(dsKhachHang.LayDanhSach());
        }

        /// <summary>
        /// Đổ danh sách khách hàng lên dataGridView1.
        /// Thứ tự cột phải khớp đúng thứ tự đã khai báo trong Designer:
        /// clHoVaTen, clSoDienThoai, clNgayDangKi, clDiemTichLuy.
        /// </summary>
        private void HienThiDanhSach(List<KhachHang> danhSach)
        {
            dataGridView1.Rows.Clear();

            foreach (KhachHang kh in danhSach)
            {
                dataGridView1.Rows.Add(
                    kh.HoTen,
                    kh.SoDienThoai,
                    kh.NgayDangKy.ToString("dd/MM/yyyy"),
                    kh.DiemTichLuy + " điểm");
            }
        }

        private void btTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtSoDienThoai.Text.Trim();
            List<KhachHang> ketQua = dsKhachHang.TimGanDungTheoSoDienThoai(tuKhoa);

            if (ketQua.Count == 0)
            {
                MessageBox.Show(
                    "Không tìm thấy khách hàng nào có số điện thoại chứa: " + tuKhoa,
                    "Không tìm thấy",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            HienThiDanhSach(ketQua);
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            using (ThemKhach formThem = new ThemKhach(dsKhachHang))
            {
                if (formThem.ShowDialog(this) == DialogResult.OK)
                {
                    // Thêm thành công: nạp lại toàn bộ danh sách để hiển thị khách mới
                    HienThiDanhSach(dsKhachHang.LayDanhSach());
                }
            }
        }

        private void btMuaHang_Click(object sender, EventArgs e)
        {
            KhachHang kh = LayKhachHangDangChon();
            if (kh == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn một khách hàng trong danh sách trước khi mua hàng.",
                    "Chưa chọn khách hàng",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using (MuaHang formMuaHang = new MuaHang(kh, dsKhachHang, dsGiaoDich))
            {
                formMuaHang.ShowDialog(this);
            }

            // Nạp lại danh sách để cập nhật điểm tích lũy mới nhất sau khi mua hàng
            HienThiDanhSach(dsKhachHang.LayDanhSach());
        }

        private void btDoiThuong_Click(object sender, EventArgs e)
        {
            KhachHang kh = LayKhachHangDangChon();
            if (kh == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn một khách hàng trong danh sách trước khi đổi quà.",
                    "Chưa chọn khách hàng",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using (DoiQua formDoiQua = new DoiQua(kh, dsKhachHang))
            {
                formDoiQua.ShowDialog(this);
            }

            // Nạp lại danh sách để cập nhật điểm tích lũy mới nhất sau khi đổi quà
            HienThiDanhSach(dsKhachHang.LayDanhSach());
        }

        /// <summary>
        /// Reset thủ công điểm tích lũy của TẤT CẢ khách hàng về 0 (dùng khi bước
        /// sang năm mới). Không tự động chạy — chỉ thực hiện khi người dùng bấm nút
        /// này và xác nhận. Vẫn giữ cơ chế chặn reset trùng trong cùng 1 năm
        /// (xem DsKhachHang.KiemTraVaResetDiemDauNam) để tránh bấm nhầm nhiều lần.
        /// </summary>
        private void btResetDiem_Click(object sender, EventArgs e)
        {
            DialogResult xacNhan = MessageBox.Show(
                "Bạn có chắc muốn RESET điểm tích lũy về 0 cho TẤT CẢ khách hàng không?\n" +
                "Thao tác này chỉ nên thực hiện khi bước sang năm mới và không thể hoàn tác.",
                "Xác nhận reset điểm đầu năm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (xacNhan != DialogResult.Yes) return;

            bool daReset = dsKhachHang.KiemTraVaResetDiemDauNam();

            if (daReset)
            {
                MessageBox.Show(
                    "Đã reset điểm tích lũy của tất cả khách hàng về 0.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "Điểm đã được reset trong năm nay rồi, không cần reset lại.",
                    "Không cần reset",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            HienThiDanhSach(dsKhachHang.LayDanhSach());
        }

        private void btLichSuGiaoDich_Click(object sender, EventArgs e)
        {
            KhachHang kh = LayKhachHangDangChon();
            if (kh == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn một khách hàng trong danh sách trước khi xem lịch sử giao dịch.",
                    "Chưa chọn khách hàng",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using (LichSuGiaoDich formLichSu = new LichSuGiaoDich(kh, dsGiaoDich))
            {
                formLichSu.ShowDialog(this);
            }
        }

        /// <summary>
        /// Bấm vào cột "Điểm Tích Lũy" (dạng nút) trên 1 dòng khách hàng
        /// sẽ mở luôn màn hình Lịch Sử Giao Dịch của khách hàng đó.
        /// </summary>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridView1.Columns[e.ColumnIndex].Name != clDiemTichLuy.Name) return;

            btLichSuGiaoDich_Click(sender, e);
        }

        /// <summary>
        /// Khi bấm chuột phải vào 1 dòng, tự động chọn (bôi đen) dòng đó trước
        /// rồi mới hiện menu Sửa/Xóa — để đảm bảo menu luôn tác động đúng khách
        /// hàng đang trỏ chuột vào, không phải dòng được chọn từ trước đó.
        /// </summary>
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (e.RowIndex < 0) return;

            dataGridView1.ClearSelection();
            dataGridView1.Rows[e.RowIndex].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }

        private void mnuSuaKhachHang_Click(object sender, EventArgs e)
        {
            KhachHang kh = LayKhachHangDangChon();
            if (kh == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để sửa.",
                    "Chưa chọn khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (ThemKhach formSua = new ThemKhach(dsKhachHang, kh))
            {
                if (formSua.ShowDialog(this) == DialogResult.OK)
                {
                    HienThiDanhSach(dsKhachHang.LayDanhSach());
                }
            }
        }

        private void mnuXoaKhachHang_Click(object sender, EventArgs e)
        {
            KhachHang kh = LayKhachHangDangChon();
            if (kh == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa.",
                    "Chưa chọn khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult xacNhan = MessageBox.Show(
                $"Bạn có chắc muốn xóa khách hàng \"{kh.HoTen}\" ({kh.SoDienThoai}) không?\n" +
                "Lưu ý: lịch sử giao dịch của khách hàng này (nếu có) sẽ không bị xóa.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes) return;

            bool thanhCong = dsKhachHang.XoaKhachHang(kh.SoDienThoai);
            if (thanhCong)
            {
                HienThiDanhSach(dsKhachHang.LayDanhSach());
            }
            else
            {
                MessageBox.Show("Không thể xóa khách hàng này. Vui lòng thử lại.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Lấy khách hàng đang được chọn (dòng đang bôi đen) trên dataGridView1.
        /// Trả về null nếu chưa chọn dòng nào.
        /// </summary>
        private KhachHang LayKhachHangDangChon()
        {
            if (dataGridView1.CurrentRow == null)
                return null;

            string soDienThoai = dataGridView1.CurrentRow.Cells[clSoDienThoai.Name].Value?.ToString();
            return dsKhachHang.TimTheoSoDienThoai(soDienThoai);
        }
    }
}
