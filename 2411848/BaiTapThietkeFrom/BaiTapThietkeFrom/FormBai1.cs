using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapThietkeFrom
{
    public partial class FormBai1 : Form
    {
        public FormBai1()
        {
            InitializeComponent();
        }

        

        private void btnTinhTien_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu đơn giá và số lượng từ TextBox
                decimal donGia = decimal.Parse(txtDonGia.Text);
                int soLuong = int.Parse(txtSoLuong.Text);

                // Tính thành tiền
                decimal thanhTien = donGia * soLuong;

                // Hiển thị kết quả
                lblSoTien.Text = $"{thanhTien:N0}";
                // Hoặc dùng MessageBox:
                // MessageBox.Show($"Tổng tiền: {thanhTien:N0} VNĐ", "Kết quả");
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập số hợp lệ cho đơn giá và số lượng!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
