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
            label1 = new Label();
            label2 = new Label();
            txtSoDienThoai = new TextBox();
            btTimKiem = new Button();
            dataGridView1 = new DataGridView();
            btThem = new Button();
            btMuaHang = new Button();
            btLichSuGiaoDich = new Button();
            clHoVaTen = new DataGridViewTextBoxColumn();
            clSoDienThoai = new DataGridViewTextBoxColumn();
            clNgayDangKi = new DataGridViewTextBoxColumn();
            clDiemTichLuy = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(1062, 68);
            label1.TabIndex = 0;
            label1.Text = "Quản Lý Tiệm Thuốc";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 100);
            label2.Name = "label2";
            label2.Size = new Size(265, 28);
            label2.TabIndex = 1;
            label2.Text = "Nhập Số Điện Thoại Cần Tìm:";
            // 
            // txtSoDienThoai
            // 
            txtSoDienThoai.Location = new Point(283, 104);
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.Size = new Size(259, 27);
            txtSoDienThoai.TabIndex = 2;
            // 
            // btTimKiem
            // 
            btTimKiem.Location = new Point(355, 153);
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
            dataGridView1.Location = new Point(12, 188);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1038, 473);
            dataGridView1.TabIndex = 4;
            // 
            // btThem
            // 
            btThem.Location = new Point(597, 103);
            btThem.Name = "btThem";
            btThem.Size = new Size(94, 29);
            btThem.TabIndex = 5;
            btThem.Text = "Thêm";
            btThem.UseVisualStyleBackColor = true;
            // 
            // btMuaHang
            // 
            btMuaHang.Location = new Point(720, 104);
            btMuaHang.Name = "btMuaHang";
            btMuaHang.Size = new Size(154, 29);
            btMuaHang.TabIndex = 6;
            btMuaHang.Text = "Mua Hàng";
            btMuaHang.UseVisualStyleBackColor = true;
            // 
            // btLichSuGiaoDich
            // 
            btLichSuGiaoDich.Location = new Point(720, 139);
            btLichSuGiaoDich.Name = "btLichSuGiaoDich";
            btLichSuGiaoDich.Size = new Size(182, 29);
            btLichSuGiaoDich.TabIndex = 7;
            btLichSuGiaoDich.Text = "Xem Lịch Sử Giao Dịch";
            btLichSuGiaoDich.UseVisualStyleBackColor = true;
            
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
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1062, 673);
            Controls.Add(btLichSuGiaoDich);
            Controls.Add(btMuaHang);
            Controls.Add(btThem);
            Controls.Add(dataGridView1);
            Controls.Add(btTimKiem);
            Controls.Add(txtSoDienThoai);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "MainForm";
            Text = "Quản Lý Tiệm Thuốc";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
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
    }
}
