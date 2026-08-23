namespace QuanLyTiemThuoc
{
    partial class QuanLyQua
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            label1 = new Label();
            txtTenQua = new TextBox();
            label2 = new Label();
            txtDiemQua = new TextBox();
            label3 = new Label();
            txtThangApDung = new TextBox();
            label4 = new Label();
            txtMaVoucher = new TextBox();
            btThemQua = new Button();
            dgvQua = new DataGridView();
            colTenQua = new DataGridViewTextBoxColumn();
            colDiemQua = new DataGridViewTextBoxColumn();
            colThangApDung = new DataGridViewTextBoxColumn();
            colMaVoucher = new DataGridViewTextBoxColumn();
            btXoaQua = new Button();
            btDong = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvQua).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 20);
            label1.Name = "label1";
            label1.Size = new Size(80, 20);
            label1.TabIndex = 0;
            label1.Text = "Tên Quà:";
            // 
            // txtTenQua
            // 
            txtTenQua.Location = new Point(150, 17);
            txtTenQua.Name = "txtTenQua";
            txtTenQua.Size = new Size(320, 27);
            txtTenQua.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 60);
            label2.Name = "label2";
            label2.Size = new Size(115, 20);
            label2.TabIndex = 2;
            label2.Text = "Điểm Cần Đổi:";
            // 
            // txtDiemQua
            // 
            txtDiemQua.Location = new Point(150, 57);
            txtDiemQua.Name = "txtDiemQua";
            txtDiemQua.Size = new Size(120, 27);
            txtDiemQua.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 100);
            label3.Name = "label3";
            label3.Size = new Size(300, 20);
            label3.TabIndex = 4;
            label3.Text = "Tháng Áp Dụng (1-12, cách nhau dấu phẩy):";
            // 
            // txtThangApDung
            // 
            txtThangApDung.Location = new Point(340, 97);
            txtThangApDung.Name = "txtThangApDung";
            txtThangApDung.PlaceholderText = "VD: 9,10";
            txtThangApDung.Size = new Size(130, 27);
            txtThangApDung.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(24, 140);
            label4.Name = "label4";
            label4.Size = new Size(300, 20);
            label4.TabIndex = 10;
            label4.Text = "Mã Voucher Cấp Kèm (để trống nếu là quà vật lý):";
            // 
            // txtMaVoucher
            // 
            txtMaVoucher.Location = new Point(340, 137);
            txtMaVoucher.Name = "txtMaVoucher";
            txtMaVoucher.PlaceholderText = "VD: GIAM20K";
            txtMaVoucher.Size = new Size(130, 27);
            txtMaVoucher.TabIndex = 11;
            // 
            // btThemQua
            // 
            btThemQua.Location = new Point(24, 180);
            btThemQua.Name = "btThemQua";
            btThemQua.Size = new Size(150, 29);
            btThemQua.TabIndex = 6;
            btThemQua.Text = "Thêm Quà";
            btThemQua.UseVisualStyleBackColor = true;
            // 
            // dgvQua
            // 
            dgvQua.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvQua.Columns.AddRange(new DataGridViewColumn[] { colTenQua, colDiemQua, colThangApDung, colMaVoucher });
            dgvQua.Location = new Point(24, 230);
            dgvQua.Name = "dgvQua";
            dgvQua.RowHeadersVisible = false;
            dgvQua.RowHeadersWidth = 51;
            dgvQua.Size = new Size(580, 260);
            dgvQua.TabIndex = 7;
            // 
            // colTenQua
            // 
            colTenQua.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colTenQua.HeaderText = "Tên Quà";
            colTenQua.MinimumWidth = 6;
            colTenQua.Name = "colTenQua";
            // 
            // colDiemQua
            // 
            colDiemQua.HeaderText = "Điểm Cần Đổi";
            colDiemQua.MinimumWidth = 6;
            colDiemQua.Name = "colDiemQua";
            colDiemQua.Width = 120;
            // 
            // colThangApDung
            // 
            colThangApDung.HeaderText = "Tháng Áp Dụng";
            colThangApDung.MinimumWidth = 6;
            colThangApDung.Name = "colThangApDung";
            colThangApDung.Width = 150;
            // 
            // colMaVoucher
            // 
            colMaVoucher.HeaderText = "Mã Voucher";
            colMaVoucher.MinimumWidth = 6;
            colMaVoucher.Name = "colMaVoucher";
            colMaVoucher.Width = 110;
            // 
            // btXoaQua
            // 
            btXoaQua.Location = new Point(24, 500);
            btXoaQua.Name = "btXoaQua";
            btXoaQua.Size = new Size(150, 29);
            btXoaQua.TabIndex = 8;
            btXoaQua.Text = "Xóa Quà Đã Chọn";
            btXoaQua.UseVisualStyleBackColor = true;
            // 
            // btDong
            // 
            btDong.Location = new Point(454, 500);
            btDong.Name = "btDong";
            btDong.Size = new Size(150, 29);
            btDong.TabIndex = 9;
            btDong.Text = "Đóng";
            btDong.UseVisualStyleBackColor = true;
            // 
            // QuanLyQua
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(630, 550);
            Controls.Add(btDong);
            Controls.Add(btXoaQua);
            Controls.Add(dgvQua);
            Controls.Add(txtMaVoucher);
            Controls.Add(label4);
            Controls.Add(btThemQua);
            Controls.Add(txtThangApDung);
            Controls.Add(label3);
            Controls.Add(txtDiemQua);
            Controls.Add(label2);
            Controls.Add(txtTenQua);
            Controls.Add(label1);
            Name = "QuanLyQua";
            Text = "Quản Lý Quà";
            ((System.ComponentModel.ISupportInitialize)dgvQua).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtTenQua;
        private Label label2;
        private TextBox txtDiemQua;
        private Label label3;
        private TextBox txtThangApDung;
        private Label label4;
        private TextBox txtMaVoucher;
        private Button btThemQua;
        private DataGridView dgvQua;
        private DataGridViewTextBoxColumn colTenQua;
        private DataGridViewTextBoxColumn colDiemQua;
        private DataGridViewTextBoxColumn colThangApDung;
        private DataGridViewTextBoxColumn colMaVoucher;
        private Button btXoaQua;
        private Button btDong;
    }
}
