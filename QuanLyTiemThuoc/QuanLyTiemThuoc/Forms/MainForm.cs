using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyTiemThuoc.Models;
using QuanLyTiemThuoc.Services;

namespace QuanLyTiemThuoc.Forms
{
    /// <summary>
    /// Form chính của ứng dụng: hiển thị danh sách khách hàng,
    /// cho phép Thêm / Sửa / Xóa, tìm kiếm, và mở màn hình giao dịch.
    /// </summary>
    public class MainForm : Form
    {
        private readonly CustomerService _customerService;
        private readonly TransactionService _transactionService;

        private DataGridView dgvCustomers;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnTransaction;
        private Button btnHistory;
        private Label lblTitle;

        public MainForm()
        {
            string dataFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _customerService = new CustomerService(System.IO.Path.Combine(dataFolder, "customers.txt"));
            _transactionService = new TransactionService(
                System.IO.Path.Combine(dataFolder, "transactions.txt"), _customerService);

            InitializeUI();
            LoadCustomerGrid();
        }

        private void InitializeUI()
        {
            Text = "Quản Lý Tiệm Thuốc";
            Width = 900;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(800, 500);

            lblTitle = new Label
            {
                Text = "QUẢN LÝ KHÁCH HÀNG - TIỆM THUỐC",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };

            txtSearch = new TextBox
            {
                Location = new Point(20, 55),
                Width = 250,
                PlaceholderText = "Tìm theo tên hoặc số điện thoại..."
            };
            txtSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    LoadCustomerGrid();
                }
            };

            btnSearch = new Button { Text = "Tìm kiếm", Location = new Point(280, 53), Width = 90 };
            btnSearch.Click += (s, e) => LoadCustomerGrid();

            btnAdd = new Button { Text = "Thêm khách hàng", Location = new Point(400, 53), Width = 140 };
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button { Text = "Sửa", Location = new Point(550, 53), Width = 80 };
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button { Text = "Xóa", Location = new Point(640, 53), Width = 80 };
            btnDelete.Click += BtnDelete_Click;

            btnTransaction = new Button { Text = "Thêm giao dịch", Location = new Point(730, 53), Width = 130 };
            btnTransaction.Click += BtnTransaction_Click;

            btnHistory = new Button { Text = "Xem lịch sử giao dịch", Location = new Point(20, 90), Width = 180 };
            btnHistory.Click += BtnHistory_Click;

            dgvCustomers = new DataGridView
            {
                Location = new Point(20, 130),
                Size = new Size(840, 420),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            Controls.Add(lblTitle);
            Controls.Add(txtSearch);
            Controls.Add(btnSearch);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnTransaction);
            Controls.Add(btnHistory);
            Controls.Add(dgvCustomers);
        }

        private void LoadCustomerGrid()
        {
            var customers = _customerService.Search(txtSearch.Text);

            dgvCustomers.DataSource = null;
            dgvCustomers.DataSource = customers;

            if (dgvCustomers.Columns.Count > 0)
            {
                dgvCustomers.Columns["Id"].HeaderText = "Mã KH";
                dgvCustomers.Columns["FullName"].HeaderText = "Họ và tên";
                dgvCustomers.Columns["PhoneNumber"].HeaderText = "Số điện thoại";
                dgvCustomers.Columns["Address"].HeaderText = "Địa chỉ";
                dgvCustomers.Columns["RegisterDate"].HeaderText = "Ngày đăng ký";
                dgvCustomers.Columns["LoyaltyPoints"].HeaderText = "Điểm tích lũy";
            }
        }

        private Customer GetSelectedCustomer()
        {
            if (dgvCustomers.CurrentRow == null) return null;
            return dgvCustomers.CurrentRow.DataBoundItem as Customer;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using var form = new CustomerForm(_customerService, null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadCustomerGrid();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedCustomer();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để sửa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var form = new CustomerForm(_customerService, selected.Id);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadCustomerGrid();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedCustomer();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa khách hàng \"{selected.FullName}\"?\n" +
                "Toàn bộ lịch sử giao dịch của khách hàng này cũng sẽ bị xóa.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                // Xóa giao dịch trước để không còn "giao dịch mồ côi" trong file txt
                _transactionService.DeleteAllByCustomer(selected.Id);
                _customerService.DeleteCustomer(selected.Id);
                LoadCustomerGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTransaction_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedCustomer();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để lập giao dịch.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var form = new TransactionForm(_customerService, _transactionService, selected.Id, showHistoryOnly: false);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadCustomerGrid();
            }
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedCustomer();
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xem lịch sử.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var form = new TransactionForm(_customerService, _transactionService, selected.Id, showHistoryOnly: true);
            form.ShowDialog();
        }
    }
}
