namespace QuanLyTiemThuoc
{
    partial class ThemKhach
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
            label2 = new Label();
            txtNhapTen = new TextBox();
            txtThemSDT = new TextBox();
            btLuu = new Button();
            btHuy = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(93, 77);
            label1.Name = "label1";
            label1.Size = new Size(151, 31);
            label1.TabIndex = 0;
            label1.Text = "Nhập Họ Tên:";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(23, 139);
            label2.Name = "label2";
            label2.Size = new Size(221, 31);
            label2.TabIndex = 0;
            label2.Text = "Nhập Số Điện Thoại:";
            label2.Click += label1_Click;
            // 
            // txtNhapTen
            // 
            txtNhapTen.Location = new Point(263, 83);
            txtNhapTen.Name = "txtNhapTen";
            txtNhapTen.Size = new Size(256, 27);
            txtNhapTen.TabIndex = 1;
            // 
            // txtThemSDT
            // 
            txtThemSDT.Location = new Point(263, 143);
            txtThemSDT.Name = "txtThemSDT";
            txtThemSDT.Size = new Size(256, 27);
            txtThemSDT.TabIndex = 2;
            // 
            // btLuu
            // 
            btLuu.Location = new Point(263, 186);
            btLuu.Name = "btLuu";
            btLuu.Size = new Size(88, 29);
            btLuu.TabIndex = 3;
            btLuu.Text = "Lưu";
            btLuu.UseVisualStyleBackColor = true;
            // 
            // btHuy
            // 
            btHuy.Location = new Point(425, 186);
            btHuy.Name = "btHuy";
            btHuy.Size = new Size(94, 29);
            btHuy.TabIndex = 4;
            btHuy.Text = "Hủy";
            btHuy.UseVisualStyleBackColor = true;
            // 
            // ThemKhach
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(619, 321);
            Controls.Add(btHuy);
            Controls.Add(btLuu);
            Controls.Add(txtThemSDT);
            Controls.Add(txtNhapTen);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ThemKhach";
            Text = "Thêm Khách Hàng Mới";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtNhapTen;
        private TextBox txtThemSDT;
        private Button btLuu;
        private Button btHuy;
    }
}