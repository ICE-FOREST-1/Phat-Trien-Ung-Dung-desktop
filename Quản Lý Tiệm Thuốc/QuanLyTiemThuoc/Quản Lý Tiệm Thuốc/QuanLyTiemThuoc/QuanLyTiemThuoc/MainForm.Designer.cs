namespace QuanLyTiemThuoc
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label2 = new Label();
            txtSoDienThoai = new TextBox();
            btTimKiem = new Button();
            dataGridView1 = new DataGridView();
            clHoVaTen = new DataGridViewTextBoxColumn();
            clSoDienThoai = new DataGridViewTextBoxColumn();
            clNgayDangKi = new DataGridViewTextBoxColumn();
            clDiemTichLuy = new DataGridViewButtonColumn();
            btThem = new Button();
            btMuaHang = new Button();
            btLichSuGiaoDich = new Button();
            btDoiThuong = new Button();
            btResetDiem = new Button();
            btLsDoiQua = new Button();
            panel1 = new Panel();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 81);
            label2.Name = "label2";
            label2.Size = new Size(265, 28);
            label2.TabIndex = 1;
            label2.Text = "Nhập Số Điện Thoại Cần Tìm:";
            // 
            // txtSoDienThoai
            // 
            txtSoDienThoai.Location = new Point(283, 81);
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.Size = new Size(259, 27);
            txtSoDienThoai.TabIndex = 2;
            // 
            // btTimKiem
            // 
            btTimKiem.Location = new Point(361, 131);
            btTimKiem.Name = "btTimKiem";
            btTimKiem.Size = new Size(94, 29);
            btTimKiem.TabIndex = 3;
            btTimKiem.Text = "Tìm Kiếm";
            btTimKiem.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { clHoVaTen, clSoDienThoai, clNgayDangKi, clDiemTichLuy });
            dataGridView1.Dock = DockStyle.Bottom;
            dataGridView1.Location = new Point(0, 189);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1056, 473);
            dataGridView1.TabIndex = 4;
            // 
            // clHoVaTen
            // 
            clHoVaTen.HeaderText = "Họ Và Tên";
            clHoVaTen.MinimumWidth = 6;
            clHoVaTen.Name = "clHoVaTen";
            // 
            // clSoDienThoai
            // 
            clSoDienThoai.HeaderText = "Số Điện Thoại";
            clSoDienThoai.MinimumWidth = 6;
            clSoDienThoai.Name = "clSoDienThoai";
            clSoDienThoai.Resizable = DataGridViewTriState.True;
            // 
            // clNgayDangKi
            // 
            clNgayDangKi.HeaderText = "Ngày Đăng Ký";
            clNgayDangKi.MinimumWidth = 6;
            clNgayDangKi.Name = "clNgayDangKi";
            // 
            // clDiemTichLuy
            // 
            clDiemTichLuy.HeaderText = "Điểm Tích Lũy";
            clDiemTichLuy.MinimumWidth = 6;
            clDiemTichLuy.Name = "clDiemTichLuy";
            // 
            // btThem
            // 
            btThem.Location = new Point(570, 81);
            btThem.Name = "btThem";
            btThem.Size = new Size(94, 29);
            btThem.TabIndex = 5;
            btThem.Text = "Thêm";
            btThem.UseVisualStyleBackColor = true;
            // 
            // btMuaHang
            // 
            btMuaHang.Location = new Point(690, 81);
            btMuaHang.Name = "btMuaHang";
            btMuaHang.Size = new Size(182, 29);
            btMuaHang.TabIndex = 6;
            btMuaHang.Text = "Mua Hàng";
            btMuaHang.UseVisualStyleBackColor = true;
            // 
            // btLichSuGiaoDich
            // 
            btLichSuGiaoDich.Location = new Point(690, 131);
            btLichSuGiaoDich.Name = "btLichSuGiaoDich";
            btLichSuGiaoDich.Size = new Size(182, 29);
            btLichSuGiaoDich.TabIndex = 7;
            btLichSuGiaoDich.Text = "Xem Lịch Sử Giao Dịch";
            btLichSuGiaoDich.UseVisualStyleBackColor = true;
            // 
            // btDoiThuong
            // 
            btDoiThuong.Location = new Point(570, 131);
            btDoiThuong.Name = "btDoiThuong";
            btDoiThuong.Size = new Size(94, 29);
            btDoiThuong.TabIndex = 8;
            btDoiThuong.Text = "Đổi Quà";
            btDoiThuong.UseVisualStyleBackColor = true;
            // 
            // btResetDiem
            // 
            btResetDiem.Location = new Point(888, 79);
            btResetDiem.Name = "btResetDiem";
            btResetDiem.Size = new Size(148, 29);
            btResetDiem.TabIndex = 9;
            btResetDiem.Text = "Reset Đầu Năm";
            btResetDiem.UseVisualStyleBackColor = true;
            // 
            // btLsDoiQua
            // 
            btLsDoiQua.Location = new Point(888, 131);
            btLsDoiQua.Name = "btLsDoiQua";
            btLsDoiQua.Size = new Size(148, 29);
            btLsDoiQua.TabIndex = 10;
            btLsDoiQua.Text = "Lịch Sử Đổi Quà";
            btLsDoiQua.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(dataGridView1);
            panel1.Controls.Add(btResetDiem);
            panel1.Controls.Add(btLsDoiQua);
            panel1.Controls.Add(txtSoDienThoai);
            panel1.Controls.Add(btThem);
            panel1.Controls.Add(btLichSuGiaoDich);
            panel1.Controls.Add(btDoiThuong);
            panel1.Controls.Add(btMuaHang);
            panel1.Controls.Add(btTimKiem);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1056, 662);
            panel1.TabIndex = 11;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(1056, 53);
            label1.TabIndex = 11;
            label1.Text = "Quản Lý Tiệm Thuốc";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1062, 673);
            Controls.Add(panel1);
            Name = "MainForm";
            Text = "Quản Lý Tiệm Thuốc";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label label2;
        private TextBox txtSoDienThoai;
        private Button btTimKiem;
        private DataGridView dataGridView1;
        private Button btThem;
        private Button btMuaHang;
        private Button btLichSuGiaoDich;
        private DataGridViewTextBoxColumn clHoVaTen;
        private DataGridViewTextBoxColumn clSoDienThoai;
        private DataGridViewTextBoxColumn clNgayDangKi;
        private DataGridViewButtonColumn clDiemTichLuy;
        private Button btDoiThuong;
        private Button btResetDiem;
        private Button btLsDoiQua;
        private Panel panel1;
        private Label label1;
    }
}
