namespace QuanLyTiemThuoc
{
    partial class LichSuDoiQua
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
            lblKhachHang = new Label();
            dataGridView1 = new DataGridView();
            label2 = new Label();
            clMaQua = new DataGridViewTextBoxColumn();
            clTenQua = new DataGridViewTextBoxColumn();
            clThoiGian = new DataGridViewTextBoxColumn();
            clChiPhi = new DataGridViewTextBoxColumn();
            clTrangThai = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(150, 31);
            label1.TabIndex = 0;
            label1.Text = "Khách Hàng,";
            // 
            // lblKhachHang
            // 
            lblKhachHang.AutoSize = true;
            lblKhachHang.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblKhachHang.Location = new Point(168, 9);
            lblKhachHang.Name = "lblKhachHang";
            lblKhachHang.Size = new Size(20, 31);
            lblKhachHang.TabIndex = 0;
            lblKhachHang.Text = " ";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { clMaQua, clTenQua, clThoiGian, clChiPhi, clTrangThai });
            dataGridView1.Location = new Point(3, 104);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(785, 334);
            dataGridView1.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(9, 71);
            label2.Name = "label2";
            label2.Size = new Size(181, 31);
            label2.TabIndex = 2;
            label2.Text = "Lịch Sử Đổi Quà:";
            // 
            // clMaQua
            // 
            clMaQua.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            clMaQua.HeaderText = "Mã Đổi Quà";
            clMaQua.MinimumWidth = 6;
            clMaQua.Name = "clMaQua";
            // 
            // clTenQua
            // 
            clTenQua.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            clTenQua.HeaderText = "Tên Quà";
            clTenQua.MinimumWidth = 6;
            clTenQua.Name = "clTenQua";
            // 
            // clThoiGian
            // 
            clThoiGian.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            clThoiGian.HeaderText = "Thời Gian ";
            clThoiGian.MinimumWidth = 6;
            clThoiGian.Name = "clThoiGian";
            // 
            // clChiPhi
            // 
            clChiPhi.HeaderText = "Chi Phí";
            clChiPhi.MinimumWidth = 6;
            clChiPhi.Name = "clChiPhi";
            clChiPhi.Width = 125;
            // 
            // clTrangThai
            // 
            clTrangThai.HeaderText = "Trạng Thái";
            clTrangThai.MinimumWidth = 6;
            clTrangThai.Name = "clTrangThai";
            clTrangThai.Width = 125;
            // 
            // LichSuDoiQua
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(dataGridView1);
            Controls.Add(lblKhachHang);
            Controls.Add(label1);
            Name = "LichSuDoiQua";
            Text = "LichSuDoiQua";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label lblKhachHang;
        private DataGridView dataGridView1;
        private Label label2;
        private DataGridViewTextBoxColumn clMaQua;
        private DataGridViewTextBoxColumn clTenQua;
        private DataGridViewTextBoxColumn clThoiGian;
        private DataGridViewTextBoxColumn clChiPhi;
        private DataGridViewTextBoxColumn clTrangThai;
    }
}