namespace QuanLyTiemThuoc
{
    partial class MuaHang
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
            cbChon = new ComboBox();
            label2 = new Label();
            textBox1 = new TextBox();
            btThemGiaoDich = new Button();
            label3 = new Label();
            lblKhachHang = new Label();
            dataGridView1 = new DataGridView();
            clMaDG = new DataGridViewTextBoxColumn();
            clNgayMua = new DataGridViewTextBoxColumn();
            clNoiDung = new DataGridViewTextBoxColumn();
            clSoLuong = new DataGridViewTextBoxColumn();
            clSoTien = new DataGridViewTextBoxColumn();
            clDiemNhan = new DataGridViewTextBoxColumn();
            label4 = new Label();
            txtSoLuong = new TextBox();
            dataGridView2 = new DataGridView();
            clTenThuoc = new DataGridViewTextBoxColumn();
            clSoLuong2 = new DataGridViewTextBoxColumn();
            clDonGia2 = new DataGridViewTextBoxColumn();
            clSoTien2 = new DataGridViewTextBoxColumn();
            btnThem = new Button();
            labelVoucher = new Label();
            cbVoucher = new ComboBox();
            lblTongKet = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(33, 60);
            label1.Name = "label1";
            label1.Size = new Size(161, 28);
            label1.TabIndex = 0;
            label1.Text = "Chọn Loại Thuốc:";
            // 
            // cbChon
            // 
            cbChon.FormattingEnabled = true;
            cbChon.Location = new Point(214, 58);
            cbChon.Name = "cbChon";
            cbChon.Size = new Size(430, 28);
            cbChon.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(330, 98);
            label2.Name = "label2";
            label2.Size = new Size(145, 28);
            label2.TabIndex = 0;
            label2.Text = "Đơn Giá (VNĐ):";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(481, 102);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(163, 27);
            textBox1.TabIndex = 3;
            // 
            // btThemGiaoDich
            // 
            btThemGiaoDich.Location = new Point(399, 372);
            btThemGiaoDich.Name = "btThemGiaoDich";
            btThemGiaoDich.Size = new Size(245, 29);
            btThemGiaoDich.TabIndex = 8;
            btThemGiaoDich.Text = "Thanh Toán (+Điểm Tự Cộng)";
            btThemGiaoDich.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(33, 20);
            label3.Name = "label3";
            label3.Size = new Size(150, 31);
            label3.TabIndex = 4;
            label3.Text = "Khách Hàng:";
            // 
            // lblKhachHang
            // 
            lblKhachHang.AutoSize = true;
            lblKhachHang.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblKhachHang.Location = new Point(189, 20);
            lblKhachHang.Name = "lblKhachHang";
            lblKhachHang.Size = new Size(20, 31);
            lblKhachHang.TabIndex = 5;
            lblKhachHang.Text = " ";
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { clMaDG, clNgayMua, clNoiDung, clSoLuong, clSoTien, clDiemNhan });
            dataGridView1.Location = new Point(12, 412);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(849, 270);
            dataGridView1.TabIndex = 9;
            // 
            // clMaDG
            // 
            clMaDG.HeaderText = "Mã Giao Dịch";
            clMaDG.MinimumWidth = 6;
            clMaDG.Name = "clMaDG";
            // 
            // clNgayMua
            // 
            clNgayMua.HeaderText = "Ngày Mua";
            clNgayMua.MinimumWidth = 6;
            clNgayMua.Name = "clNgayMua";
            // 
            // clNoiDung
            // 
            clNoiDung.HeaderText = "Tên Thuốc";
            clNoiDung.MinimumWidth = 6;
            clNoiDung.Name = "clNoiDung";
            // 
            // clSoLuong
            // 
            clSoLuong.HeaderText = "Số Lượng";
            clSoLuong.MinimumWidth = 6;
            clSoLuong.Name = "clSoLuong";
            clSoLuong.ReadOnly = true;
            // 
            // clSoTien
            // 
            clSoTien.HeaderText = "Số Tiền (VNĐ)";
            clSoTien.MinimumWidth = 6;
            clSoTien.Name = "clSoTien";
            // 
            // clDiemNhan
            // 
            clDiemNhan.HeaderText = "Điểm Nhận";
            clDiemNhan.MinimumWidth = 6;
            clDiemNhan.Name = "clDiemNhan";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(33, 98);
            label4.Name = "label4";
            label4.Size = new Size(175, 28);
            label4.TabIndex = 7;
            label4.Text = "Số Lượng(Vỉ/Hộp):";
            // 
            // txtSoLuong
            // 
            txtSoLuong.Location = new Point(214, 95);
            txtSoLuong.Name = "txtSoLuong";
            txtSoLuong.Size = new Size(100, 27);
            txtSoLuong.TabIndex = 2;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { clTenThuoc, clSoLuong2, clDonGia2, clSoTien2 });
            dataGridView2.Location = new Point(12, 175);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(849, 140);
            dataGridView2.TabIndex = 6;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
            // 
            // clTenThuoc
            // 
            clTenThuoc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            clTenThuoc.HeaderText = "Tên Thuốc";
            clTenThuoc.MinimumWidth = 6;
            clTenThuoc.Name = "clTenThuoc";
            // 
            // clSoLuong2
            // 
            clSoLuong2.HeaderText = "Số Lượng";
            clSoLuong2.MinimumWidth = 6;
            clSoLuong2.Name = "clSoLuong2";
            // 
            // clDonGia2
            // 
            clDonGia2.HeaderText = "Đơn Giá";
            clDonGia2.MinimumWidth = 6;
            clDonGia2.Name = "clDonGia2";
            clDonGia2.Width = 120;
            // 
            // clSoTien2
            // 
            clSoTien2.HeaderText = "Thành Tiền";
            clSoTien2.MinimumWidth = 6;
            clSoTien2.Name = "clSoTien2";
            clSoTien2.Width = 130;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(650, 132);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(90, 29);
            btnThem.TabIndex = 4;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            // 
            // labelVoucher
            // 
            labelVoucher.AutoSize = true;
            labelVoucher.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelVoucher.Location = new Point(33, 136);
            labelVoucher.Name = "labelVoucher";
            labelVoucher.Size = new Size(170, 28);
            labelVoucher.TabIndex = 0;
            labelVoucher.Text = "Áp Dụng Voucher:";
            // 
            // cbVoucher
            // 
            cbVoucher.FormattingEnabled = true;
            cbVoucher.Location = new Point(214, 133);
            cbVoucher.Name = "cbVoucher";
            cbVoucher.Size = new Size(430, 28);
            cbVoucher.TabIndex = 5;
            // 
            // lblTongKet
            // 
            lblTongKet.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTongKet.Location = new Point(12, 322);
            lblTongKet.Name = "lblTongKet";
            lblTongKet.Size = new Size(849, 40);
            lblTongKet.TabIndex = 7;
            lblTongKet.Text = " ";
            // 
            // MuaHang
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(873, 715);
            Controls.Add(lblTongKet);
            Controls.Add(cbVoucher);
            Controls.Add(labelVoucher);
            Controls.Add(btnThem);
            Controls.Add(dataGridView2);
            Controls.Add(txtSoLuong);
            Controls.Add(label4);
            Controls.Add(dataGridView1);
            Controls.Add(lblKhachHang);
            Controls.Add(label3);
            Controls.Add(btThemGiaoDich);
            Controls.Add(textBox1);
            Controls.Add(cbChon);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "MuaHang";
            Text = "Mua Hàng";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cbChon;
        private Label label2;
        private TextBox textBox1;
        private Button btThemGiaoDich;
        private Label label3;
        private Label lblKhachHang;
        private DataGridView dataGridView1;
        private Label label4;
        private TextBox txtSoLuong;
        private DataGridViewTextBoxColumn clMaDG;
        private DataGridViewTextBoxColumn clNgayMua;
        private DataGridViewTextBoxColumn clNoiDung;
        private DataGridViewTextBoxColumn clSoLuong;
        private DataGridViewTextBoxColumn clSoTien;
        private DataGridViewTextBoxColumn clDiemNhan;
        private DataGridView dataGridView2;
        private DataGridViewTextBoxColumn clTenThuoc;
        private DataGridViewTextBoxColumn clSoLuong2;
        private DataGridViewTextBoxColumn clDonGia2;
        private DataGridViewTextBoxColumn clSoTien2;
        private Button btnThem;
        private Label labelVoucher;
        private ComboBox cbVoucher;
        private Label lblTongKet;
    }
}
