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
            label4 = new Label();
            txtSoLuong = new TextBox();
            clMaDG = new DataGridViewTextBoxColumn();
            clNgayMua = new DataGridViewTextBoxColumn();
            clNoiDung = new DataGridViewTextBoxColumn();
            clSoLuong = new DataGridViewTextBoxColumn();
            clSoTien = new DataGridViewTextBoxColumn();
            clDiemNhan = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(33, 88);
            label1.Name = "label1";
            label1.Size = new Size(161, 28);
            label1.TabIndex = 0;
            label1.Text = "Chọn Loại Thuốc:";
            // 
            // cbChon
            // 
            cbChon.FormattingEnabled = true;
            cbChon.Items.AddRange(new object[] { "Paracetamol (Panadol, Hapacol)", "Ibuprofen", "Aspirin", "Cataflam (Diclofenac)", "Alpha Choay", "Amoxicillin", "Augmentin (Amoxicillin + Clavulanate)", "Cefixim", "Azithromycin", "Vitamin C (Efferalgan C / C sủi)", "Hoạt huyết dưỡng não (Ginkgo Biloba)" });
            cbChon.Location = new Point(200, 88);
            cbChon.Name = "cbChon";
            cbChon.Size = new Size(245, 28);
            cbChon.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(33, 148);
            label2.Name = "label2";
            label2.Size = new Size(138, 28);
            label2.TabIndex = 0;
            label2.Text = "Số Tiền (VNĐ):";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(200, 152);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(179, 27);
            textBox1.TabIndex = 2;
            // 
            // btThemGiaoDich
            // 
            btThemGiaoDich.Location = new Point(200, 207);
            btThemGiaoDich.Name = "btThemGiaoDich";
            btThemGiaoDich.Size = new Size(245, 29);
            btThemGiaoDich.TabIndex = 3;
            btThemGiaoDich.Text = "Thêm Giao Dịch(+Điểm Tự Cộng)";
            btThemGiaoDich.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(33, 33);
            label3.Name = "label3";
            label3.Size = new Size(150, 31);
            label3.TabIndex = 4;
            label3.Text = "Khách Hàng:";
            // 
            // lblKhachHang
            // 
            lblKhachHang.AutoSize = true;
            lblKhachHang.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblKhachHang.Location = new Point(189, 33);
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
            dataGridView1.Location = new Point(12, 275);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(849, 268);
            dataGridView1.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(486, 86);
            label4.Name = "label4";
            label4.Size = new Size(175, 28);
            label4.TabIndex = 7;
            label4.Text = "Số Lượng(Vỉ/Hộp):";
            // 
            // txtSoLuong
            // 
            txtSoLuong.Location = new Point(667, 90);
            txtSoLuong.Name = "txtSoLuong";
            txtSoLuong.Size = new Size(147, 27);
            txtSoLuong.TabIndex = 8;
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
            clNoiDung.HeaderText = "Nội Dung";
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
            // MuaHang
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(873, 555);
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
    }
}