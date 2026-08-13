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
    public partial class FormChinh : Form
    {
        public FormChinh()
        {
            InitializeComponent();
        }

        private void bài1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBai1 f1 = new FormBai1();

            // Hiển thị form dưới dạng cửa sổ mới
            f1.Show();
        }

        private void bài2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBai2 f2 = new FormBai2();

            // Hiển thị form dưới dạng cửa sổ mới
            f2.Show();
        }

        private void bài3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBai3 f3 = new FormBai3();

            // Hiển thị form dưới dạng cửa sổ mới
            f3.Show();
        }
    }
}
