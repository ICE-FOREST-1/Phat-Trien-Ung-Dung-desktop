namespace QuanLyTiemThuoc
{
    partial class LichSuGiaoDich
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            lblLsKhachHang = new Label();
            label2 = new Label();
            dataGridView1 = new DataGridView();
            clMaGiaoDich = new DataGridViewTextBoxColumn();
            clNgayMua = new DataGridViewTextBoxColumn();
            clNoiDung = new DataGridViewTextBoxColumn();
            clSoLuong = new DataGridViewTextBoxColumn();
            clSoTien = new DataGridViewTextBoxColumn();
            clDiem = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 26);
            label1.Name = "label1";
            label1.Size = new Size(150, 31);
            label1.TabIndex = 0;
            label1.Text = "Khách Hàng:";
            // 
            // lblLsKhachHang
            // 
            lblLsKhachHang.AutoSize = true;
            lblLsKhachHang.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLsKhachHang.Location = new Point(168, 26);
            lblLsKhachHang.Name = "lblLsKhachHang";
            lblLsKhachHang.Size = new Size(20, 31);
            lblLsKhachHang.TabIndex = 0;
            lblLsKhachHang.Text = " ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 117);
            label2.Name = "label2";
            label2.Size = new Size(155, 23);
            label2.TabIndex = 1;
            label2.Text = "Lịch Sử Giao Dịch:";
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { clMaGiaoDich, clNgayMua, clNoiDung, clSoLuong, clSoTien, clDiem });
            dataGridView1.Location = new Point(12, 143);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(776, 295);
            dataGridView1.TabIndex = 2;
            // 
            // clMaGiaoDich
            // 
            clMaGiaoDich.HeaderText = "Mã GD";
            clMaGiaoDich.MinimumWidth = 6;
            clMaGiaoDich.Name = "clMaGiaoDich";
            // 
            // clNgayMua
            // 
            clNgayMua.HeaderText = "Ngày Mua";
            clNgayMua.MinimumWidth = 6;
            clNgayMua.Name = "clNgayMua";
            clNgayMua.ReadOnly = true;
            // 
            // clNoiDung
            // 
            clNoiDung.HeaderText = "Tên Thuốc";
            clNoiDung.MinimumWidth = 6;
            clNoiDung.Name = "clNoiDung";
            clNoiDung.ReadOnly = true;
            // 
            // clSoLuong
            // 
            clSoLuong.HeaderText = "Số Lượng";
            clSoLuong.MinimumWidth = 6;
            clSoLuong.Name = "clSoLuong";
            // 
            // clSoTien
            // 
            clSoTien.HeaderText = "Số Tiền (VNĐ)";
            clSoTien.MinimumWidth = 6;
            clSoTien.Name = "clSoTien";
            clSoTien.ReadOnly = true;
            // 
            // clDiem
            // 
            clDiem.HeaderText = "Điểm Nhận";
            clDiem.MinimumWidth = 6;
            clDiem.Name = "clDiem";
            clDiem.ReadOnly = true;
            // 
            // LichSuGiaoDich
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridView1);
            Controls.Add(label2);
            Controls.Add(lblLsKhachHang);
            Controls.Add(label1);
            Name = "LichSuGiaoDich";
            Text = "Lịch Sử Giao Dịch";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label lblLsKhachHang;
        private Label label2;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn clMaGiaoDich;
        private DataGridViewTextBoxColumn clNgayMua;
        private DataGridViewTextBoxColumn clNoiDung;
        private DataGridViewTextBoxColumn clSoLuong;
        private DataGridViewTextBoxColumn clSoTien;
        private DataGridViewTextBoxColumn clDiem;
    }
}