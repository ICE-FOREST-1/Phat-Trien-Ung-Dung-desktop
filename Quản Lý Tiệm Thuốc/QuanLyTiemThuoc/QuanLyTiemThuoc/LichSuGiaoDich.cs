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

        public LichSuGiaoDich(KhachHang khachHang, DsGiaoDich dsGiaoDich)
        {
            InitializeComponent();
            this.khachHang = khachHang;
            this.dsGiaoDich = dsGiaoDich;

            Load += LichSuGiaoDich_Load;
        }

        private void LichSuGiaoDich_Load(object sender, EventArgs e)
        {
            lblLsKhachHang.Text = khachHang.HoTen + " - " + khachHang.SoDienThoai;
            HienThiLichSu();
        }

        /// <summary>
        /// Đổ lịch sử giao dịch của khách hàng lên dataGridView1.
        /// Thứ tự cột: clMaGiaoDich, clNgayMua, clNoiDung, clSoTien, clDiem.
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
    }
}
