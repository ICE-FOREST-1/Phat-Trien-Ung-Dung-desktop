namespace QuanLyTiemThuoc
{
    partial class DoiQua
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
            label2 = new Label();
            comboBox1 = new ComboBox();
            label3 = new Label();
            txtDiemCanDoi = new TextBox();
            label4 = new Label();
            lblTongDiem = new Label();
            btQuyDoi = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(24, 42);
            label1.Name = "label1";
            label1.Size = new Size(150, 31);
            label1.TabIndex = 0;
            label1.Text = "Khách Hàng:";
            // 
            // lblKhachHang
            // 
            lblKhachHang.AutoSize = true;
            lblKhachHang.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblKhachHang.Location = new Point(180, 42);
            lblKhachHang.Name = "lblKhachHang";
            lblKhachHang.Size = new Size(20, 31);
            lblKhachHang.TabIndex = 0;
            lblKhachHang.Text = " ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(24, 180);
            label2.Name = "label2";
            label2.Size = new Size(197, 28);
            label2.TabIndex = 1;
            label2.Text = "Danh Sách Phần Quà:";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(227, 180);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(227, 28);
            comboBox1.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(57, 236);
            label3.Name = "label3";
            label3.Size = new Size(164, 28);
            label3.TabIndex = 3;
            label3.Text = "Số Điểm Cần Đổi:";
            // 
            // txtDiemCanDoi
            // 
            txtDiemCanDoi.Location = new Point(227, 240);
            txtDiemCanDoi.Name = "txtDiemCanDoi";
            txtDiemCanDoi.ReadOnly = true;
            txtDiemCanDoi.Size = new Size(227, 27);
            txtDiemCanDoi.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(24, 102);
            label4.Name = "label4";
            label4.Size = new Size(164, 20);
            label4.TabIndex = 5;
            label4.Text = "Tổng Số Điểm Hiện Có:";
            // 
            // lblTongDiem
            // 
            lblTongDiem.AutoSize = true;
            lblTongDiem.Location = new Point(208, 102);
            lblTongDiem.Name = "lblTongDiem";
            lblTongDiem.Size = new Size(13, 20);
            lblTongDiem.TabIndex = 5;
            lblTongDiem.Text = " ";
            // 
            // btQuyDoi
            // 
            btQuyDoi.Location = new Point(360, 292);
            btQuyDoi.Name = "btQuyDoi";
            btQuyDoi.Size = new Size(94, 29);
            btQuyDoi.TabIndex = 6;
            btQuyDoi.Text = "Quy Đổi";
            btQuyDoi.UseVisualStyleBackColor = true;
            // 
            // DoiQua
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btQuyDoi);
            Controls.Add(lblTongDiem);
            Controls.Add(label4);
            Controls.Add(txtDiemCanDoi);
            Controls.Add(label3);
            Controls.Add(comboBox1);
            Controls.Add(label2);
            Controls.Add(lblKhachHang);
            Controls.Add(label1);
            Name = "DoiQua";
            Text = "Đổi Điểm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label lblKhachHang;
        private Label label2;
        private ComboBox comboBox1;
        private Label label3;
        private TextBox txtDiemCanDoi;
        private Label label4;
        private Label lblTongDiem;
        private Button btQuyDoi;
    }
}