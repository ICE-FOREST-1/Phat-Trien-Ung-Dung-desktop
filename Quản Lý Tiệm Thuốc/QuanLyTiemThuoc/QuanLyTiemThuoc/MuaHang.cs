using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyTiemThuoc
{
    public partial class MuaHang : Form
    {
        private const string KHONG_AP_DUNG_VOUCHER = "-- Không áp dụng voucher --";

        private readonly KhachHang khachHang;
        private readonly DsKhachHang dsKhachHang;
        private readonly DsGiaoDich dsGiaoDich;
        private readonly DsChiTietGiaoDich dsChiTietGiaoDich;
        private readonly DsVoucher dsVoucher;
        private readonly DsVoucherKhachHang dsVoucherKhachHang;
        private List<ChiTietDonHang> gioHang = new List<ChiTietDonHang>();

        /// <summary>
        /// Gói 1 voucher khách hàng đang sở hữu (VoucherKhachHang) kèm chi tiết
        /// voucher gốc (Voucher) để hiển thị mô tả và tính giảm giá trong cbVoucher.
        /// </summary>
        private class VoucherKhaDung
        {
            public VoucherKhachHang SoHuu { get; }
            public Voucher ChiTiet { get; }

            public VoucherKhaDung(VoucherKhachHang soHuu, Voucher chiTiet)
            {
                SoHuu = soHuu;
                ChiTiet = chiTiet;
            }

            public override string ToString() => ChiTiet.MoTa();
        }

        public MuaHang(KhachHang khachHang, DsKhachHang dsKhachHang, DsGiaoDich dsGiaoDich,
            DsChiTietGiaoDich dsChiTietGiaoDich, DsVoucher dsVoucher, DsVoucherKhachHang dsVoucherKhachHang)
        {
            InitializeComponent();
            this.khachHang = khachHang;
            this.dsKhachHang = dsKhachHang;
            this.dsGiaoDich = dsGiaoDich;
            this.dsChiTietGiaoDich = dsChiTietGiaoDich;
            this.dsVoucher = dsVoucher;
            this.dsVoucherKhachHang = dsVoucherKhachHang;

            Load += MuaHang_Load;
            btnThem.Click += BtnThem_Click;
            btThemGiaoDich.Click += BtThemGiaoDich_Click;
            cbChon.TextChanged += CbChon_TextChanged;
            cbVoucher.SelectedIndexChanged += (s, e) => CapNhatTongKet();
        }

        private void MuaHang_Load(object sender, EventArgs e)
        {
            lblKhachHang.Text = khachHang.HoTen + " - " + khachHang.SoDienThoai;
            KhoiTaoDataGridView1();
            KhoiTaoDataGridView2();
            KhoiTaoComboBoxThuoc();
            KhoiTaoComboBoxVoucher();
            HienThiLichSu();
            CapNhatTongKet();
        }

        private void KhoiTaoDataGridView1()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
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
        /// Đổ danh sách thuốc (kèm liều lượng) vào cbChon và bật gõ-tìm-kiếm
        /// (AutoComplete) để nhân viên gõ thẳng tên thuốc là ra gợi ý.
        /// </summary>
        private void KhoiTaoComboBoxThuoc()
        {
            cbChon.DropDownStyle = ComboBoxStyle.DropDown;
            cbChon.Items.Clear();
            cbChon.Items.AddRange(ThuocInfo.LayDanhSachTen());

            AutoCompleteStringCollection nguon = new AutoCompleteStringCollection();
            nguon.AddRange(ThuocInfo.LayDanhSachTen());
            cbChon.AutoCompleteMode = AutoCompleteMode.Suggest;
            cbChon.AutoCompleteSource = AutoCompleteSource.CustomSource;
            cbChon.AutoCompleteCustomSource = nguon;
        }

        /// <summary>
        /// Đổ danh sách voucher khách hàng ĐANG SỞ HỮU và CHƯA SỬ DỤNG vào cbVoucher.
        /// Khách chỉ có voucher trong danh sách này sau khi đã đổi quà thành công ở
        /// form Đổi Quà (xem DoiQua.btQuyDoi_Click) — không còn cho chọn tự do từ
        /// danh sách voucher chung của cửa hàng nữa.
        /// Mục đầu tiên luôn là "Không áp dụng" (dạng chuỗi, không phải voucher).
        /// </summary>
        private void KhoiTaoComboBoxVoucher()
        {
            cbVoucher.DropDownStyle = ComboBoxStyle.DropDownList;
            cbVoucher.Items.Clear();
            cbVoucher.Items.Add(KHONG_AP_DUNG_VOUCHER);

            List<VoucherKhachHang> voucherSoHuu = dsVoucherKhachHang.LayVoucherKhaDung(khachHang.SoDienThoai);
            foreach (VoucherKhachHang vkh in voucherSoHuu)
            {
                Voucher chiTiet = dsVoucher.TimTheoMa(vkh.MaVoucher);
                if (chiTiet == null) continue; // voucher gốc đã bị xóa khỏi voucher.txt
                cbVoucher.Items.Add(new VoucherKhaDung(vkh, chiTiet));
            }

            cbVoucher.SelectedIndex = 0;
        }

        /// <summary>
        /// Khi gõ/tìm tên thuốc khớp đúng với danh sách có sẵn, tự động điền giá
        /// mẫu vào ô "Đơn Giá". Nhân viên vẫn có thể sửa lại giá này trước khi Thêm.
        /// </summary>
        private void CbChon_TextChanged(object sender, EventArgs e)
        {
            decimal? gia = ThuocInfo.TimGia(cbChon.Text);
            if (gia.HasValue)
            {
                textBox1.Text = gia.Value.ToString("0");
            }
        }

        /// <summary>
        /// Đổ lịch sử giao dịch của khách hàng đang mua hàng lên dataGridView1.
        /// Mỗi hóa đơn (1 mã giao dịch) hiển thị TỪNG DÒNG THUỐC RIÊNG (lấy từ
        /// dsChiTietGiaoDich), MỖI DÒNG CÓ SỐ ĐIỂM TÍCH LŨY TƯƠNG ỨNG CỦA RIÊNG
        /// THUỐC ĐÓ (tính theo thành tiền gốc của dòng đó) — không còn dòng tổng
        /// kết hóa đơn gộp chung nữa.
        /// Với các giao dịch cũ (lưu trước khi có tính năng lưu chi tiết theo dòng),
        /// không có dữ liệu chi tiết thì hiển thị lại 1 dòng gộp như cũ để không mất dữ liệu.
        /// Thứ tự cột: clMaDG, clNgayMua, clNoiDung(Tên Thuốc), clSoLuong, clSoTien, clDiemNhan.
        /// </summary>
        private void HienThiLichSu()
        {
            dataGridView1.Rows.Clear();

            List<GiaoDich> lichSu = dsGiaoDich.LayDanhSachTheoSoDienThoai(khachHang.SoDienThoai);
            foreach (GiaoDich gd in lichSu)
            {
                List<ChiTietGiaoDich> chiTiet = dsChiTietGiaoDich.LayTheoMaGiaoDich(gd.MaGiaoDich);

                if (chiTiet.Count == 0)
                {
                    // Giao dịch cũ, chưa có dữ liệu chi tiết theo dòng -> hiển thị gộp như trước.
                    dataGridView1.Rows.Add(
                        gd.MaGiaoDich,
                        gd.NgayMua.ToString("dd/MM/yyyy HH:mm"),
                        gd.NoiDung,
                        gd.SoLuong,
                        gd.SoTien.ToString("N0"),
                        gd.DiemNhan);
                    continue;
                }

                // Mỗi dòng thuốc hiển thị riêng, CÙNG chung 1 mã giao dịch (gd.MaGiaoDich),
                // và có ĐIỂM TÍCH LŨY RIÊNG tính theo thành tiền gốc của chính dòng đó.
                decimal tongTienGocHoaDon = chiTiet.Sum(x => x.ThanhTien);

                // Tỉ lệ giữa tiền THỰC THU của hóa đơn (gd.SoTien, đã áp voucher nếu có)
                // và tiền GỐC của hóa đơn. Dùng để phân bổ đều số tiền giảm giá xuống
                // từng dòng thuốc khi hiển thị, để tổng các dòng khớp với số tiền
                // khách thực trả thay vì luôn hiện giá gốc.
                decimal tyLeThanhToan = tongTienGocHoaDon > 0 ? gd.SoTien / tongTienGocHoaDon : 1m;

                // Mỗi dòng thuốc hiển thị riêng, CÙNG chung 1 mã giao dịch (gd.MaGiaoDich),
                // và có ĐIỂM TÍCH LŨY RIÊNG tính theo thành tiền GỐC của chính dòng đó
                // (điểm không bị ảnh hưởng bởi voucher, chỉ số tiền hiển thị mới bị).
                foreach (ChiTietGiaoDich ct in chiTiet)
                {
                    decimal thanhTienSauVoucher = ct.ThanhTien * tyLeThanhToan;

                    dataGridView1.Rows.Add(
                        gd.MaGiaoDich,
                        gd.NgayMua.ToString("dd/MM/yyyy HH:mm"),
                        ct.TenThuoc,
                        ct.SoLuong,
                        thanhTienSauVoucher.ToString("N0"),
                        GiaoDich.TinhDiem(ct.ThanhTien));
                }
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
                MessageBox.Show("Vui lòng chọn hoặc nhập tên loại thuốc.",
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
            CapNhatTongKet();

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
                    item.DonGia.ToString("N0"),
                    item.ThanhTien.ToString("N0"));
            }
        }

        /// <summary>
        /// Lấy gói voucher đang được chọn trong cbVoucher (null nếu không áp dụng).
        /// </summary>
        private VoucherKhaDung LayVoucherDangChon()
        {
            return cbVoucher.SelectedItem as VoucherKhaDung;
        }

        /// <summary>
        /// Cập nhật dòng tổng kết (tạm tính / giảm giá / thành tiền) mỗi khi giỏ
        /// hàng hoặc voucher thay đổi, để nhân viên xem trước khi thanh toán.
        /// </summary>
        private void CapNhatTongKet()
        {
            decimal tamTinh = gioHang.Sum(x => x.ThanhTien);
            VoucherKhaDung voucher = LayVoucherDangChon();
            decimal giamGia = voucher?.ChiTiet.TinhSoTienGiam(tamTinh) ?? 0m;
            decimal thanhTienThanhToan = tamTinh - giamGia;

            lblTongKet.Text =
                $"Tạm tính: {tamTinh:N0} đ      Giảm giá: {giamGia:N0} đ      " +
                $"Thành tiền: {thanhTienThanhToan:N0} đ      " +
                $"(Điểm tích lũy tính theo giá gốc: {GiaoDich.TinhDiem(tamTinh)} điểm)";
        }

        /// <summary>
        /// Sự kiện khi bấm nút "Thanh Toán" - lưu lại tất cả các mặt hàng vào hóa đơn
        /// (1 hóa đơn = 1 mã giao dịch duy nhất, dù có 1 hay nhiều loại thuốc).
        /// Nếu có áp dụng voucher: số tiền thực thu được trừ giảm giá, nhưng điểm
        /// tích lũy vẫn tính theo TỔNG TIỀN GỐC trước khi giảm giá.
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

            // Tính tổng giá trị hóa đơn theo giá GỐC (chưa áp voucher)
            decimal tongTienGoc = gioHang.Sum(x => x.ThanhTien);

            // Áp dụng voucher (nếu có chọn) để ra số tiền thực thu
            VoucherKhaDung voucher = LayVoucherDangChon();
            decimal soTienGiam = voucher?.ChiTiet.TinhSoTienGiam(tongTienGoc) ?? 0m;
            decimal tongTienThanhToan = tongTienGoc - soTienGiam;

            // Điểm tích lũy LUÔN tính theo giá gốc, không bị ảnh hưởng bởi voucher
            int diemNhan = GiaoDich.TinhDiem(tongTienGoc);

            // Tạo chuỗi mô tả hóa đơn (vẫn giữ để tương thích/tra cứu nhanh)
            string moTaHoaDon = string.Join("; ", gioHang.Select(x => $"{x.TenThuoc} (SL: {x.SoLuong})"));

            // Tính tổng số lượng
            int tongSoLuong = gioHang.Sum(x => x.SoLuong);

            // Lưu giao dịch (1 mã giao dịch duy nhất) vào dữ liệu.
            // SoTien lưu vào giao dịch là số tiền THỰC THU (sau khi giảm giá).
            GiaoDich giaoDichMoi = dsGiaoDich.ThemGiaoDich(
                khachHang.SoDienThoai, moTaHoaDon, tongTienThanhToan, tongSoLuong, diemNhan);

            if (giaoDichMoi == null)
            {
                MessageBox.Show("Không thể lưu hóa đơn. Vui lòng kiểm tra lại thông tin.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Lưu chi tiết từng dòng thuốc, TẤT CẢ cùng chung 1 mã giao dịch vừa tạo.
            List<ChiTietGiaoDich> cacDongChiTiet = gioHang
                .Select(x => new ChiTietGiaoDich(giaoDichMoi.MaGiaoDich, x.TenThuoc, x.SoLuong, x.DonGia, x.ThanhTien))
                .ToList();
            dsChiTietGiaoDich.ThemNhieu(cacDongChiTiet);

            // Cộng điểm tích lũy vào tài khoản khách hàng
            dsKhachHang.CongDiem(khachHang.SoDienThoai, giaoDichMoi.DiemNhan);

            // Nếu có áp dụng voucher: đánh dấu voucher đó ĐÃ SỬ DỤNG để không dùng lại được lần 2.
            if (voucher != null)
            {
                dsVoucherKhachHang.DanhDauDaSuDung(voucher.SoHuu);
            }

            // Hiển thị thông báo thành công
            string dongVoucher = soTienGiam > 0
                ? $"Voucher: {voucher.ChiTiet.MoTa()} (-{soTienGiam:N0} VNĐ)\n"
                : string.Empty;

            MessageBox.Show(
                $"Thanh toán thành công!\n" +
                $"Tạm tính: {tongTienGoc:N0} VNĐ\n" +
                dongVoucher +
                $"Thành tiền: {tongTienThanhToan:N0} VNĐ\n" +
                $"Khách hàng nhận thêm {giaoDichMoi.DiemNhan} điểm tích lũy (tính theo giá gốc).",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Xóa giỏ hàng, nạp lại danh sách voucher (voucher vừa dùng phải biến mất) và cập nhật hiển thị
            gioHang.Clear();
            HienThiGioHang();
            KhoiTaoComboBoxVoucher();
            CapNhatTongKet();
            HienThiLichSu();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Có thể thêm tính năng xóa dòng hoặc sửa sau này
        }
    }
}
