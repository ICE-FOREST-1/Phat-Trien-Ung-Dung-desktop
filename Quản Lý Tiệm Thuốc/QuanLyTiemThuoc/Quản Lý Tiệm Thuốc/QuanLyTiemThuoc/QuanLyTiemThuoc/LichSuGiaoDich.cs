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
    public partial class LichSuGiaoDich : Form
    {
        private readonly KhachHang khachHang;
        private readonly DsGiaoDich dsGiaoDich;
        private readonly DsChiTietGiaoDich dsChiTietGiaoDich;

        public LichSuGiaoDich(KhachHang khachHang, DsGiaoDich dsGiaoDich, DsChiTietGiaoDich dsChiTietGiaoDich)
        {
            InitializeComponent();
            this.khachHang = khachHang;
            this.dsGiaoDich = dsGiaoDich;
            this.dsChiTietGiaoDich = dsChiTietGiaoDich;

            Load += LichSuGiaoDich_Load;
        }

        private void LichSuGiaoDich_Load(object sender, EventArgs e)
        {
            lblLsKhachHang.Text = khachHang.HoTen + " - " + khachHang.SoDienThoai;
            HienThiLichSu();
        }

        /// <summary>
        /// Đổ lịch sử giao dịch của khách hàng lên dataGridView1.
        /// Mỗi hóa đơn hiển thị TỪNG DÒNG THUỐC riêng (cùng chung 1 mã giao dịch),
        /// mỗi dòng có điểm tích lũy riêng tính theo thành tiền gốc của dòng đó
        /// (không còn dòng tổng kết gộp). Với giao dịch cũ chưa có dữ liệu chi tiết,
        /// hiển thị lại 1 dòng gộp như trước để không mất dữ liệu.
        /// Thứ tự cột: clMaGiaoDich, clNgayMua, clNoiDung, clSoLuong, clSoTien, clDiem.
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
                    dataGridView1.Rows.Add(
                        gd.MaGiaoDich,
                        gd.NgayMua.ToString("dd/MM/yyyy HH:mm"),
                        gd.NoiDung,
                        gd.SoLuong,
                        gd.SoTien.ToString("N0"),
                        gd.DiemNhan);
                    continue;
                }

                foreach (ChiTietGiaoDich ct in chiTiet)
                {
                    dataGridView1.Rows.Add(
                        gd.MaGiaoDich,
                        gd.NgayMua.ToString("dd/MM/yyyy HH:mm"),
                        ct.TenThuoc,
                        ct.SoLuong,
                        ct.ThanhTien.ToString("N0"),
                        GiaoDich.TinhDiem(ct.ThanhTien));
                }
            }
        }
    }
}
