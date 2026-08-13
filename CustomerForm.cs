using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTiemThuoc.Models;
using QuanLyTiemThuoc.Services;

namespace QuanLyTiemThuoc.Forms
{
    /// <summary>
    /// Form dùng chung cho cả THÊM MỚI và SỬA khách hàng.
    /// customerId == null  -> chế độ Thêm mới
    /// customerId != null  -> chế độ Sửa
    /// </summary>
    public class CustomerForm : Form
    {
        private readonly CustomerService _customerService;
        private readonly int? _customerId;

        private TextBox txtName;
        private TextBox txtPhone;
        private TextBox txtAddress;
        private Label lblRegisterDate;
        private Label lblPoints;
        private Button btnSave;
        private Button btnCancel;

        public CustomerForm(CustomerService customerService, int? customerId)
        {
            _customerService = customerService;
            _customerId = customerId;

            InitializeUI();

            if (_customerId.HasValue)
            {
                LoadExistingCustomer(_customerId.Value);
            }
            else
            {
                // Chế độ thêm mới: hiển thị rõ ràng điểm khởi tạo = 0 và ngày đăng ký = hôm nay
                lblRegisterDate.Text = "Ngày đăng ký: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                lblPoints.Text = "Điểm tích lũy khởi tạo: 0";
            }
        }

        private void InitializeUI()
        {
            Text = _customerId.HasValue ? "Sửa thông tin khách hàng" : "Thêm khách hàng mới";
            Width = 420;
            Height = 320;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var lblName = new Label { Text = "Họ và tên (*):", Location = new Point(20, 25), AutoSize = true };
            txtName = new TextBox { Location = new Point(160, 22), Width = 220 };

            var lblPhone = new Label { Text = "Số điện thoại (*):", Location = new Point(20, 60), AutoSize = true };
            txtPhone = new TextBox { Location = new Point(160, 57), Width = 220 };

            var lblAddress = new Label { Text = "Địa chỉ:", Location = new Point(20, 95), AutoSize = true };
            txtAddress = new TextBox { Location = new Point(160, 92), Width = 220 };

            lblRegisterDate = new Label
            {
                Location = new Point(20, 140),
                AutoSize = true,
                ForeColor = Color.DimGray
            };

            lblPoints = new Label
            {
                Location = new Point(20, 165),
                AutoSize = true,
                ForeColor = Color.DimGray
            };

            btnSave = new Button { Text = "Lưu", Location = new Point(200, 220), Width = 80 };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Hủy", Location = new Point(300, 220), Width = 80 };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblPhone);
            Controls.Add(txtPhone);
            Controls.Add(lblAddress);
            Controls.Add(txtAddress);
            Controls.Add(lblRegisterDate);
            Controls.Add(lblPoints);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);

            AcceptButton = btnSave;
            CancelButton = btnCancel;
        }

        private void LoadExistingCustomer(int id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null)
            {
                MessageBox.Show("Không tìm thấy khách hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            txtName.Text = customer.FullName;
            txtPhone.Text = customer.PhoneNumber;
            txtAddress.Text = customer.Address;
            lblRegisterDate.Text = "Ngày đăng ký: " + customer.RegisterDate.ToString("dd/MM/yyyy HH:mm");
            lblPoints.Text = "Điểm tích lũy hiện tại: " + customer.LoyaltyPoints;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_customerId.HasValue)
                {
                    _customerService.UpdateCustomer(_customerId.Value, txtName.Text, txtPhone.Text, txtAddress.Text);
                }
                else
                {
                    _customerService.AddCustomer(txtName.Text, txtPhone.Text, txtAddress.Text);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể lưu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
