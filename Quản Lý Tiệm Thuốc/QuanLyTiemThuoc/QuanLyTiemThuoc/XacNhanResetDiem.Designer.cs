namespace QuanLyTiemThuoc
{
    partial class XacNhanResetDiem
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
            lblGhiChu = new Label();
            dgvKhachHang = new DataGridView();
            colHoTen = new DataGridViewTextBoxColumn();
            colSoDienThoai = new DataGridViewTextBoxColumn();
            colDiemHienTai = new DataGridViewTextBoxColumn();
            btTiepTuc = new Button();
            btHuy = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvKhachHang).BeginInit();
            SuspendLayout();
            // 
            // lblGhiChu
            // 
            lblGhiChu.AutoSize = false;
            lblGhiChu.ForeColor = Color.Firebrick;
            lblGhiChu.Location = new Point(20, 15);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(600, 60);
            lblGhiChu.TabIndex = 0;
            lblGhiChu.Text = " ";
            // 
            // dgvKhachHang
            // 
            dgvKhachHang.AllowUserToAddRows = false;
            dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKhachHang.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKhachHang.Columns.AddRange(new DataGridViewColumn[] { colHoTen, colSoDienThoai, colDiemHienTai });
            dgvKhachHang.Location = new Point(20, 85);
            dgvKhachHang.Name = "dgvKhachHang";
            dgvKhachHang.ReadOnly = true;
            dgvKhachHang.RowHeadersVisible = false;
            dgvKhachHang.RowHeadersWidth = 51;
            dgvKhachHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKhachHang.Size = new Size(600, 300);
            dgvKhachHang.TabIndex = 1;
            // 
            // colHoTen
            // 
            colHoTen.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colHoTen.HeaderText = "Họ Tên";
            colHoTen.MinimumWidth = 6;
            colHoTen.Name = "colHoTen";
            // 
            // colSoDienThoai
            // 
            colSoDienThoai.HeaderText = "Số Điện Thoại";
            colSoDienThoai.MinimumWidth = 6;
            colSoDienThoai.Name = "colSoDienThoai";
            colSoDienThoai.Width = 150;
            // 
            // colDiemHienTai
            // 
            colDiemHienTai.HeaderText = "Điểm Hiện Tại";
            colDiemHienTai.MinimumWidth = 6;
            colDiemHienTai.Name = "colDiemHienTai";
            colDiemHienTai.Width = 120;
            // 
            // btTiepTuc
            // 
            btTiepTuc.BackColor = Color.IndianRed;
            btTiepTuc.ForeColor = Color.White;
            btTiepTuc.Location = new Point(370, 400);
            btTiepTuc.Name = "btTiepTuc";
            btTiepTuc.Size = new Size(250, 32);
            btTiepTuc.TabIndex = 2;
            btTiepTuc.Text = "Tiếp Tục Reset Điểm";
            btTiepTuc.UseVisualStyleBackColor = false;
            // 
            // btHuy
            // 
            btHuy.Location = new Point(20, 400);
            btHuy.Name = "btHuy";
            btHuy.Size = new Size(150, 32);
            btHuy.TabIndex = 3;
            btHuy.Text = "Hủy";
            btHuy.UseVisualStyleBackColor = true;
            // 
            // XacNhanResetDiem
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(640, 450);
            Controls.Add(btHuy);
            Controls.Add(btTiepTuc);
            Controls.Add(dgvKhachHang);
            Controls.Add(lblGhiChu);
            Name = "XacNhanResetDiem";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Xác Nhận Reset Điểm";
            ((System.ComponentModel.ISupportInitialize)dgvKhachHang).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblGhiChu;
        private DataGridView dgvKhachHang;
        private DataGridViewTextBoxColumn colHoTen;
        private DataGridViewTextBoxColumn colSoDienThoai;
        private DataGridViewTextBoxColumn colDiemHienTai;
        private Button btTiepTuc;
        private Button btHuy;
    }
}
