using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTiemThuoc.Services;

namespace QuanLyTiemThuoc.Forms
{
    /// <summary>
    /// Form dùng chung:
    ///  - showHistoryOnly = false -> hiển thị khung nhập để THÊM giao dịch mới, kèm bảng lịch sử phía dưới
    ///  - showHistoryOnly = true  -> chỉ hiển thị bảng LỊCH SỬ giao dịch (không cho thêm mới)
    /// </summary>
    public class TransactionForm : Form
    {
        private readonly CustomerService _customerService;
        private readonly TransactionService _transactionService;
        private readonly int _customerId;
        private readonly bool _showHistoryOnly;

        private Label lblCustomerInfo;
        private TextBox txtDescription;
        private TextBox txtAmount;
        private Button btnAddTransaction;
        private DataGridView dgvHistory;
        private Label lblTotalPoints;

        public TransactionForm(CustomerService customerService, TransactionService transactionService,
                                int customerId, bool showHistoryOnly)
        {
            _customerService = customerService;
            _transactionService = transactionService;
            _customerId = customerId;
            _showHistoryOnly = showHistoryOnly;

            InitializeUI();
            LoadCustomerHeader();
            LoadHistoryGrid();
        }

        private void InitializeUI()
        {
            Text = _showHistoryOnly ? "Lịch sử giao dịch" : "Thêm giao dịch mới";
            Width = 650;
            Height = _showHistoryOnly ? 500 : 560;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            lblCustomerInfo = new Label
            {
                Location = new Point(20, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            Controls.Add(lblCustomerInfo);

            int historyTop = 60;

            if (!_showHistoryOnly)
            {
                var lblDesc = new Label { Text = "Nội dung mua:", Location = new Point(20, 60), AutoSize = true };
                txtDescription = new TextBox { Location = new Point(150, 57), Width = 300 };

                var lblAmount = new Label { Text = "Số tiền (VNĐ):", Location = new Point(20, 95), AutoSize = true };
                txtAmount = new TextBox { Location = new Point(150, 92), Width = 150 };

                btnAddTransaction = new Button
                {
                    Text = "Thêm giao dịch (+điểm tự động)",
                    Location = new Point(150, 130),
                    Width = 260
                };
                btnAddTransaction.Click += BtnAddTransaction_Click;

                Controls.Add(lblDesc);
                Controls.Add(txtDescription);
                Controls.Add(lblAmount);
                Controls.Add(txtAmount);
                Controls.Add(btnAddTransaction);

                historyTop = 175;
            }

            var lblHistoryTitle = new Label
            {
                Text = "Lịch sử giao dịch:",
                Location = new Point(20, historyTop),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            Controls.Add(lblHistoryTitle);

            dgvHistory = new DataGridView
            {
                Location = new Point(20, historyTop + 25),
                Size = new Size(590, _showHistoryOnly ? 340 : 260),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            Controls.Add(dgvHistory);

            lblTotalPoints = new Label
            {
                Location = new Point(20, historyTop + 25 + dgvHistory.Height + 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            Controls.Add(lblTotalPoints);
        }

        private void LoadCustomerHeader()
        {
            var customer = _customerService.GetById(_customerId);
            if (customer == null)
            {
                MessageBox.Show("Không tìm thấy khách hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            lblCustomerInfo.Text = $"Khách hàng: {customer.FullName}  -  SĐT: {customer.PhoneNumber}";
        }

        private void LoadHistoryGrid()
        {
            var history = _transactionService.GetByCustomer(_customerId);
            dgvHistory.DataSource = null;
            dgvHistory.DataSource = history;

            if (dgvHistory.Columns.Count > 0)
            {
                dgvHistory.Columns["Id"].HeaderText = "Mã GD";
                dgvHistory.Columns["CustomerId"].Visible = false;
                dgvHistory.Columns["TransactionDate"].HeaderText = "Ngày giao dịch";
                dgvHistory.Columns["Description"].HeaderText = "Nội dung";
                dgvHistory.Columns["Amount"].HeaderText = "Số tiền (VNĐ)";
                dgvHistory.Columns["PointsEarned"].HeaderText = "Điểm nhận";
            }

            var customer = _customerService.GetById(_customerId);
            if (customer != null)
            {
                lblTotalPoints.Text = $"Tổng điểm tích lũy hiện tại: {customer.LoyaltyPoints} điểm";
            }
        }

        private void BtnAddTransaction_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAmount.Text) ||
                !decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Số tiền không hợp lệ. Vui lòng nhập số.", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _transactionService.AddTransaction(_customerId, txtDescription.Text, amount);

                txtDescription.Clear();
                txtAmount.Clear();

                LoadHistoryGrid();

                DialogResult = DialogResult.OK; // báo cho MainForm biết cần load lại điểm khách hàng
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể thêm giao dịch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
