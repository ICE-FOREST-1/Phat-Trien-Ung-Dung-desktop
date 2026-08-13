using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTap_Windowsform
{
    public partial class Mainfrom : Form
    {
        public Mainfrom()
        {
            InitializeComponent();
        }


        private void btnOK_Click_1(object sender, EventArgs e)
        {
            var tenDaNhap = txtTen.Text;
            MessageBox.Show($"Xin chào bạn {tenDaNhap}, Rất Vui Được Gặp bạn", "Thông điệp chào mừng");
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            txtCopy.Text = txtTen.Text;
        }

        private void txtCopy_TextChanged(object sender, EventArgs e)
        {
            txtCopy.Text = txtTen.Text;
        }

        private void txtTen_TextChanged(object sender, EventArgs e)
        {
            txtCopy.Text = txtTen.Text;
        }
    }
}
