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
    public partial class LichSuDoiQua : Form
    {
        private readonly KhachHang khachHang;
        private readonly DsGiaoDichDoiQua dsGiaoDichDoiQua;

        public LichSuDoiQua(KhachHang khachHang, DsGiaoDichDoiQua dsGiaoDichDoiQua)
        {
            InitializeComponent();
            this.khachHang = khachHang;
            this.dsGiaoDichDoiQua = dsGiaoDichDoiQua;

            Load += LichSuDoiQua_Load;
        }

        private void LichSuDoiQua_Load(object sender, EventArgs e)
        {
            lblKhachHang.Text = khachHang.HoTen + " - " + khachHang.SoDienThoai;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            HienThiLichSu();
        }

        private void HienThiLichSu()
        {
            dataGridView1.Rows.Clear();

            List<GiaoDichDoiQua> lichSu = dsGiaoDichDoiQua.LayDanhSachTheoSoDienThoai(khachHang.SoDienThoai);
            foreach (GiaoDichDoiQua gd in lichSu)
            {
                dataGridView1.Rows.Add(
                    gd.MaDoiQua,
                    gd.TenQua,
                    gd.ThoiGian.ToString("dd/MM/yyyy HH:mm"),
                    gd.ChiPhi + " điểm",
                    gd.TrangThai);
            }
        }
    }
}
