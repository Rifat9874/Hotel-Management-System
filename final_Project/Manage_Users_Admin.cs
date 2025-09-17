using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace final_Project
{
    public partial class Manage_Users_Admin : Form
    {
        private string currentView = "users"; // Track current view: "users" or "staff"
        public Manage_Users_Admin()
        {
            InitializeComponent();
        }

        private void Manage_Employee_Admin_Load(object sender, EventArgs e)
        {
            load_Users();
            currentView = "users";
        }

        private Label CreateHeaderLabel(string text, int left, int fontSize)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Left = left,
                Top = 10
            };
        }

        private TextBox CreateStyledTextBox(string text, string name, int left, int top, int width)
        {
            return new TextBox
            {
                Text = text,
                Name = name,
                Font = new Font("Segoe Print", 15, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Height = 40,
                Width = width,
                Left = left,
                Top = top
            };
        }

        SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }

        //Update button
        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Panel panel = btn.Tag as Panel;

            string name = panel.Controls["txtName"].Text.Trim();
            string phone = panel.Controls["txtPhone"].Text.Trim();
            string email = panel.Controls["txtEmail"].Text.Trim(); // Key
            string password = panel.Controls["txtPassword"].Text.Trim();

            using (SqlConnection con = GetConnection())
            {
                string q = "UPDATE USER_TABLE SET NAME = @name, PHONE_NUMBER = @phone, PASSWORD = @password WHERE EMAIL = @email";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@email", email);
                int rowsAffected = cmd.ExecuteNonQuery();
                MessageBox.Show(rowsAffected > 0 ? "User updated successfully." : "Update failed.");
            }

            load_Users();
        }

        void load_Users(string userType = "")
        {
            currentView = "users";
            SqlConnection con = GetConnection();

            string q = "SELECT NAME, PHONE_NUMBER, EMAIL, PASSWORD FROM USER_TABLE";
            if (!string.IsNullOrEmpty(userType) && userType != "Default Sorting")
            {
                q += " WHERE USER_TYPE = @type";
            }

            SqlCommand cmd = new SqlCommand(q, con);
            if (!string.IsNullOrEmpty(userType) && userType != "Default Sorting")
            {
                cmd.Parameters.AddWithValue("@type", userType);
            }

            SqlDataReader rdr = cmd.ExecuteReader();
            flowLayoutPanel1.Controls.Clear();

            Panel headerPanel = new Panel
            {
                Width = 940,
                Height = 40,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10)
            };

            int fontSize = 18;
            headerPanel.Controls.Add(CreateHeaderLabel("Name", 26, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Phone Number", 185, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Email", 450, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Password", 660, fontSize));
            flowLayoutPanel1.Controls.Add(headerPanel);

            while (rdr.Read())
            {
                string name = rdr["NAME"].ToString();
                string phone = rdr["PHONE_NUMBER"].ToString();
                string email = rdr["EMAIL"].ToString();
                string password = rdr["PASSWORD"].ToString();

                Panel taskPanel = new Panel
                {
                    Width = 940,
                    Height = 60,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(5)
                };

                // Create named TextBoxes
                TextBox txtName = CreateStyledTextBox(name, "txtName", 10, 15, 150);
                TextBox txtPhone = CreateStyledTextBox(phone, "txtPhone", 200, 15, 170);
                TextBox txtEmail = CreateStyledTextBox(email, "txtEmail", 400, 15, 240);
                TextBox txtPassword = CreateStyledTextBox(password, "txtPassword", 670, 15, 130);

                // Buttons with parent Panel as Tag
                Button updateBtn = new Button
                {
                    Text = "Update",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    BackColor = Color.FromArgb(43, 92, 122),
                    ForeColor = Color.White,
                    Width = 100,
                    Height = 40,
                    Top = 15,
                    Left = 825,
                    Tag = taskPanel
                };
                updateBtn.Click += UpdateBtn_Click;

                // Add controls to panel
                taskPanel.Controls.Add(txtName);
                taskPanel.Controls.Add(txtPhone);
                taskPanel.Controls.Add(txtEmail);
                taskPanel.Controls.Add(txtPassword);
                taskPanel.Controls.Add(updateBtn);

                flowLayoutPanel1.Controls.Add(taskPanel);
            }

            rdr.Close();
            con.Close();
        }

        // Load staff details with salary information
        void load_StaffDetails()
        {
            currentView = "staff";
            SqlConnection con = GetConnection();

            string q = @"SELECT u.NAME, u.PHONE_NUMBER, u.EMAIL, u.USER_TYPE, u.JOIN_DATE, 
                        es.SALARY_AMOUNT, es.ALLOWANCES, es.DEDUCTIONS, es.BONUS_AMOUNT, es.BONUS_REASON,
                        (es.SALARY_AMOUNT + es.ALLOWANCES + es.BONUS_AMOUNT - es.DEDUCTIONS) as NET_SALARY
                        FROM USER_TABLE u
                        LEFT JOIN EMPLOYEE_SALARY es ON u.EMAIL = es.EMPLOYEE_EMAIL
                        WHERE u.USER_TYPE IN ('STAFF', 'RECEPTIONIST')";

            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader rdr = cmd.ExecuteReader();
            flowLayoutPanel1.Controls.Clear();

            Panel headerPanel = new Panel
            {
                Width = 1450,
                Height = 40,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10)
            };

            int fontSize = 11;
            headerPanel.Controls.Add(CreateHeaderLabel("Name", 10, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Phone", 100, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Email", 180, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Type", 380, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Join Date", 450, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Salary", 550, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Allowances", 650, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Deductions", 750, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Bonus", 850, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Bonus Reason", 950, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Net Salary", 1150, fontSize));
            flowLayoutPanel1.Controls.Add(headerPanel);

            while (rdr.Read())
            {
                string name = rdr["NAME"].ToString();
                string phone = rdr["PHONE_NUMBER"].ToString();
                string email = rdr["EMAIL"].ToString();
                string userType = rdr["USER_TYPE"].ToString();
                string joinDate = Convert.ToDateTime(rdr["JOIN_DATE"]).ToString("yyyy-MM-dd");
                string salary = rdr["SALARY_AMOUNT"] != DBNull.Value ? Convert.ToDecimal(rdr["SALARY_AMOUNT"]).ToString("N0") : "0";
                string allowances = rdr["ALLOWANCES"] != DBNull.Value ? Convert.ToDecimal(rdr["ALLOWANCES"]).ToString("N0") : "0";
                string deductions = rdr["DEDUCTIONS"] != DBNull.Value ? Convert.ToDecimal(rdr["DEDUCTIONS"]).ToString("N0") : "0";
                string bonus = rdr["BONUS_AMOUNT"] != DBNull.Value ? Convert.ToDecimal(rdr["BONUS_AMOUNT"]).ToString("N0") : "0";
                string bonusReason = rdr["BONUS_REASON"] != DBNull.Value ? rdr["BONUS_REASON"].ToString() : "";
                string netSalary = rdr["NET_SALARY"] != DBNull.Value ? Convert.ToDecimal(rdr["NET_SALARY"]).ToString("N0") : "0";

                Panel staffPanel = new Panel
                {
                    Width = 1450,
                    Height = 50,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(5),
                    BackColor = Color.FromArgb(50, 50, 60)
                };

                // Create labels to display information
                staffPanel.Controls.Add(CreateInfoLabel(name, 10, 15, 80));
                staffPanel.Controls.Add(CreateInfoLabel(phone, 100, 15, 70));
                staffPanel.Controls.Add(CreateInfoLabel(email, 180, 15, 190));
                staffPanel.Controls.Add(CreateInfoLabel(userType, 380, 15, 60));
                staffPanel.Controls.Add(CreateInfoLabel(joinDate, 450, 15, 90));
                staffPanel.Controls.Add(CreateInfoLabel(salary, 550, 15, 90));
                staffPanel.Controls.Add(CreateInfoLabel(allowances, 650, 15, 90));
                staffPanel.Controls.Add(CreateInfoLabel(deductions, 750, 15, 90));
                staffPanel.Controls.Add(CreateInfoLabel(bonus, 850, 15, 90));
                staffPanel.Controls.Add(CreateInfoLabel(bonusReason, 950, 15, 190));
                staffPanel.Controls.Add(CreateInfoLabel(netSalary, 1150, 15, 90));

                flowLayoutPanel1.Controls.Add(staffPanel);
            }

            rdr.Close();
            con.Close();
        }

        private Label CreateInfoLabel(string text, int left, int top, int width)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = false,
                Width = width,
                Height = 20,
                Left = left,
                Top = top,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        //Sorting combo-box
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = comboBox1.SelectedItem.ToString();

            if (currentView == "users")
            {
                load_Users(selectedType);
            }
        }

        //Back button
        private void button2_Click(object sender, EventArgs e)
        {
            if (currentView == "staff")
            {
                // If in staff view, go back to user view
                load_Users();
                currentView = "users";
            }
            else
            {
                // If in user view, go back to admin dashboard
                Admin_Dashboard admin_Dashboard = new Admin_Dashboard();
                admin_Dashboard.Show();
                this.Visible = false;
            }
        }

        // Button3 - View Staff Details
        private void button3_Click(object sender, EventArgs e)
        {
            load_StaffDetails();
        }

        // Button4 - Manage Salary
        private void button4_Click(object sender, EventArgs e)
        {
            // Open salary management form
            SalaryManagementForm salaryForm = new SalaryManagementForm();
            salaryForm.ShowDialog();

            // Refresh staff details after any changes
            load_StaffDetails();
        }

        // Button5 - Assign New Staff
        private void button5_Click(object sender, EventArgs e)
        {
            // Open form to assign new staff or receptionist
            AssignStaffForm assignForm = new AssignStaffForm();
            if (assignForm.ShowDialog() == DialogResult.OK)
            {
                // Refresh the view after adding new staff
                MessageBox.Show("Staff assigned successfully! Refreshing view...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_StaffDetails();
            }
        }
    }

    // New form for assigning staff
    public class AssignStaffForm : Form
    {
        private TextBox txtName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private ComboBox cmbUserType;
        private TextBox txtSalary;
        private Button btnAssign;
        private Button btnCancel;

        public AssignStaffForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Assign New Staff";
            this.Size = new Size(500, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Name
            Label lblName = new Label { Text = "Name:", Location = new Point(20, 20), Size = new Size(100, 20) };
            txtName = new TextBox { Location = new Point(150, 20), Size = new Size(300, 20) };

            // Phone
            Label lblPhone = new Label { Text = "Phone:", Location = new Point(20, 60), Size = new Size(100, 20) };
            txtPhone = new TextBox { Location = new Point(150, 60), Size = new Size(300, 20) };

            // Email
            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, 100), Size = new Size(100, 20) };
            txtEmail = new TextBox { Location = new Point(150, 100), Size = new Size(300, 20) };

            // Password
            Label lblPassword = new Label { Text = "Password:", Location = new Point(20, 140), Size = new Size(100, 20) };
            txtPassword = new TextBox { Location = new Point(150, 140), Size = new Size(300, 20), UseSystemPasswordChar = true };

            // User Type
            Label lblUserType = new Label { Text = "User Type:", Location = new Point(20, 180), Size = new Size(100, 20) };
            cmbUserType = new ComboBox { Location = new Point(150, 180), Size = new Size(300, 20) };
            cmbUserType.Items.AddRange(new object[] { "STAFF", "RECEPTIONIST" });
            cmbUserType.SelectedIndex = 0;

            // Salary
            Label lblSalary = new Label { Text = "Salary:", Location = new Point(20, 220), Size = new Size(100, 20) };
            txtSalary = new TextBox { Location = new Point(150, 220), Size = new Size(300, 20), Text = "0" };

            // Buttons
            btnAssign = new Button { Text = "Assign", Location = new Point(150, 270), Size = new Size(100, 30) };
            btnAssign.Click += BtnAssign_Click;

            btnCancel = new Button { Text = "Cancel", Location = new Point(270, 270), Size = new Size(100, 30) };
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.AddRange(new Control[] {
                lblName, txtName,
                lblPhone, txtPhone,
                lblEmail, txtEmail,
                lblPassword, txtPassword,
                lblUserType, cmbUserType,
                lblSalary, txtSalary,
                btnAssign, btnCancel
            });
        }

        private void BtnAssign_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                if (AssignNewStaff())
                {
                    MessageBox.Show("Staff assigned successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to assign staff. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter a phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter a password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtSalary.Text, out decimal salary) || salary < 0)
            {
                MessageBox.Show("Please enter a valid salary amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool AssignNewStaff()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // First check if email already exists
                    string checkQuery = "SELECT COUNT(*) FROM USER_TABLE WHERE EMAIL = @email";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("This email is already registered. Please use a different email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Insert into USER_TABLE
                    string userQuery = @"INSERT INTO USER_TABLE 
                                       (NAME, PHONE_NUMBER, EMAIL, PASSWORD, USER_TYPE, ADDRESS, DEPARTMENT, POSITION, JOIN_DATE)
                                       VALUES (@name, @phone, @email, @password, @userType, @address, @department, @position, @joinDate)";

                    SqlCommand userCmd = new SqlCommand(userQuery, con);
                    userCmd.Parameters.AddWithValue("@name", txtName.Text);
                    userCmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    userCmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    userCmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    userCmd.Parameters.AddWithValue("@userType", cmbUserType.SelectedItem.ToString());
                    userCmd.Parameters.AddWithValue("@address", "Dhaka, Bangladesh");
                    userCmd.Parameters.AddWithValue("@department", cmbUserType.SelectedItem.ToString() == "STAFF" ? "Operations" : "Front Desk");
                    userCmd.Parameters.AddWithValue("@position", cmbUserType.SelectedItem.ToString() == "STAFF" ? "Operations Staff" : "Front Desk Executive");
                    userCmd.Parameters.AddWithValue("@joinDate", DateTime.Now);

                    int userRows = userCmd.ExecuteNonQuery();

                    if (userRows > 0)
                    {
                        // Insert into EMPLOYEE_SALARY
                        string salaryQuery = @"INSERT INTO EMPLOYEE_SALARY 
                                             (EMPLOYEE_EMAIL, SALARY_AMOUNT, PAYMENT_DATE, PAYMENT_METHOD, ALLOWANCES, DEDUCTIONS, BONUS_AMOUNT, BONUS_REASON)
                                             VALUES (@email, @salary, @paymentDate, @paymentMethod, @allowances, @deductions, @bonus, @bonusReason)";

                        SqlCommand salaryCmd = new SqlCommand(salaryQuery, con);
                        salaryCmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        salaryCmd.Parameters.AddWithValue("@salary", decimal.Parse(txtSalary.Text));
                        salaryCmd.Parameters.AddWithValue("@paymentDate", DateTime.Now);
                        salaryCmd.Parameters.AddWithValue("@paymentMethod", "Bank Transfer");
                        salaryCmd.Parameters.AddWithValue("@allowances", 0);
                        salaryCmd.Parameters.AddWithValue("@deductions", 0);
                        salaryCmd.Parameters.AddWithValue("@bonus", 0);
                        salaryCmd.Parameters.AddWithValue("@bonusReason", DBNull.Value);

                        int salaryRows = salaryCmd.ExecuteNonQuery();
                        return salaryRows > 0;
                    }

                    return userRows > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error assigning staff: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    // Salary Management Form
    public class SalaryManagementForm : Form
    {
        private DataGridView dataGridView1;
        private Button btnUpdateSalary;
        private Button btnAddBonus;
        private Button btnViewBonusHistory;
        private Button btnClose;

        public SalaryManagementForm()
        {
            InitializeComponent();
            LoadSalaryData();
        }

        private void InitializeComponent()
        {
            this.dataGridView1 = new DataGridView();
            this.btnUpdateSalary = new Button();
            this.btnAddBonus = new Button();
            this.btnViewBonusHistory = new Button();
            this.btnClose = new Button();

            // dataGridView1
            this.dataGridView1.Location = new Point(20, 20);
            this.dataGridView1.Size = new Size(1100, 300);
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // btnUpdateSalary
            this.btnUpdateSalary.Text = "Update Salary";
            this.btnUpdateSalary.Location = new Point(20, 340);
            this.btnUpdateSalary.Size = new Size(120, 40);
            this.btnUpdateSalary.Click += BtnUpdateSalary_Click;

            // btnAddBonus
            this.btnAddBonus.Text = "Add Bonus";
            this.btnAddBonus.Location = new Point(160, 340);
            this.btnAddBonus.Size = new Size(120, 40);
            this.btnAddBonus.Click += BtnAddBonus_Click;

            // btnViewBonusHistory
            this.btnViewBonusHistory.Text = "Bonus History";
            this.btnViewBonusHistory.Location = new Point(300, 340);
            this.btnViewBonusHistory.Size = new Size(120, 40);
            this.btnViewBonusHistory.Click += BtnViewBonusHistory_Click;

            // btnClose
            this.btnClose.Text = "Close";
            this.btnClose.Location = new Point(440, 340);
            this.btnClose.Size = new Size(120, 40);
            this.btnClose.Click += BtnClose_Click;

            // Form settings
            this.Text = "Salary Management";
            this.Size = new Size(1150, 450);
            this.Controls.Add(dataGridView1);
            this.Controls.Add(btnUpdateSalary);
            this.Controls.Add(btnAddBonus);
            this.Controls.Add(btnViewBonusHistory);
            this.Controls.Add(btnClose);
        }

        private void LoadSalaryData()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"SELECT u.NAME, u.EMAIL, u.USER_TYPE, u.JOIN_DATE, 
                                es.SALARY_AMOUNT, es.ALLOWANCES, es.DEDUCTIONS, es.BONUS_AMOUNT, es.BONUS_REASON, es.PAYMENT_DATE,
                                (es.SALARY_AMOUNT + es.ALLOWANCES + es.BONUS_AMOUNT - es.DEDUCTIONS) as NET_SALARY
                                FROM USER_TABLE u
                                LEFT JOIN EMPLOYEE_SALARY es ON u.EMAIL = es.EMPLOYEE_EMAIL
                                WHERE u.USER_TYPE IN ('STAFF', 'RECEPTIONIST')";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;

                // Format currency columns
                string[] currencyColumns = { "SALARY_AMOUNT", "ALLOWANCES", "BONUS_AMOUNT", "DEDUCTIONS", "NET_SALARY" };
                foreach (string col in currencyColumns)
                {
                    if (dataGridView1.Columns[col] != null)
                    {
                        dataGridView1.Columns[col].DefaultCellStyle.Format = "N0";
                    }
                }

                // Highlight bonus column
                if (dataGridView1.Columns["BONUS_AMOUNT"] != null)
                {
                    dataGridView1.Columns["BONUS_AMOUNT"].DefaultCellStyle.ForeColor = Color.Green;
                    dataGridView1.Columns["BONUS_AMOUNT"].DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                }
            }
        }

        private void BtnUpdateSalary_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string email = dataGridView1.CurrentRow.Cells["EMAIL"].Value.ToString();
                decimal currentSalary = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["SALARY_AMOUNT"].Value);

                using (var updateForm = new UpdateSalaryForm(email, currentSalary))
                {
                    if (updateForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadSalaryData(); // Refresh data
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a staff member to update.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddBonus_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string email = dataGridView1.CurrentRow.Cells["EMAIL"].Value.ToString();
                string name = dataGridView1.CurrentRow.Cells["NAME"].Value.ToString();

                using (var bonusForm = new AddBonusForm(email, name))
                {
                    if (bonusForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadSalaryData(); // Refresh data
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a staff member to add bonus.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnViewBonusHistory_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string email = dataGridView1.CurrentRow.Cells["EMAIL"].Value.ToString();
                string name = dataGridView1.CurrentRow.Cells["NAME"].Value.ToString();

                using (var historyForm = new BonusHistoryForm(email, name))
                {
                    historyForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Please select a staff member to view bonus history.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    // Form for updating base salary
    public class UpdateSalaryForm : Form
    {
        private string employeeEmail;
        private TextBox txtSalary;
        private Button btnUpdate;
        private Button btnCancel;
        private decimal currentSalary;

        public UpdateSalaryForm(string email, decimal currentSalary)
        {
            employeeEmail = email;
            this.currentSalary = currentSalary;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Update Base Salary";
            this.Size = new Size(350, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblCurrent = new Label { Text = $"Current Salary: {currentSalary:N0}", Location = new Point(20, 20), Size = new Size(300, 20) };
            Label lblSalary = new Label { Text = "New Base Salary:", Location = new Point(20, 50), Size = new Size(120, 20) };
            txtSalary = new TextBox { Location = new Point(150, 50), Size = new Size(150, 20), Text = currentSalary.ToString() };

            btnUpdate = new Button { Text = "Update", Location = new Point(20, 90), Size = new Size(100, 30) };
            btnUpdate.Click += BtnUpdate_Click;

            btnCancel = new Button { Text = "Cancel", Location = new Point(140, 90), Size = new Size(100, 30) };
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.AddRange(new Control[] { lblCurrent, lblSalary, txtSalary, btnUpdate, btnCancel });
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtSalary.Text, out decimal newSalary) && newSalary >= 0)
            {
                UpdateSalaryInDatabase(newSalary);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid salary amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSalaryInDatabase(decimal newSalary)
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = @"UPDATE EMPLOYEE_SALARY 
                               SET SALARY_AMOUNT = @salary, PAYMENT_DATE = GETDATE()
                               WHERE EMPLOYEE_EMAIL = @email";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@salary", newSalary);
                cmd.Parameters.AddWithValue("@email", employeeEmail);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Base salary updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update salary.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    // Form for adding bonus (separate from base salary)
    public class AddBonusForm : Form
    {
        private string employeeEmail;
        private string employeeName;
        private TextBox txtBonus;
        private ComboBox cmbBonusReason;
        private Button btnAdd;
        private Button btnCancel;

        public AddBonusForm(string email, string name)
        {
            employeeEmail = email;
            employeeName = name;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = $"Add Bonus - {employeeName}";
            this.Size = new Size(400, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblInfo = new Label { Text = $"Adding bonus for: {employeeName}", Location = new Point(20, 20), Size = new Size(350, 20) };
            Label lblBonus = new Label { Text = "Bonus Amount:", Location = new Point(20, 50), Size = new Size(100, 20) };
            txtBonus = new TextBox { Location = new Point(120, 50), Size = new Size(150, 20), Text = "0" };

            Label lblReason = new Label { Text = "Bonus Reason:", Location = new Point(20, 80), Size = new Size(100, 20) };
            cmbBonusReason = new ComboBox { Location = new Point(120, 80), Size = new Size(200, 20) };
            cmbBonusReason.Items.AddRange(new object[] {
                "Eid Bonus",
                "Puja Bonus",
                "Individual Performance Bonus",
                "Attendance Bonus",
                "Retention Bonus",
                "Company Performance Bonus",
                "Profit-Sharing",
                "Annual Bonus",
                "Holiday Bonus",
                "Other"
            });
            cmbBonusReason.SelectedIndex = 0;

            btnAdd = new Button { Text = "Add Bonus", Location = new Point(20, 120), Size = new Size(100, 30) };
            btnAdd.Click += BtnAdd_Click;

            btnCancel = new Button { Text = "Cancel", Location = new Point(140, 120), Size = new Size(100, 30) };
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.AddRange(new Control[] { lblInfo, lblBonus, txtBonus, lblReason, cmbBonusReason, btnAdd, btnCancel });
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtBonus.Text, out decimal bonus) && bonus >= 0)
            {
                AddBonusToDatabase(bonus, cmbBonusReason.SelectedItem.ToString());
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid bonus amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddBonusToDatabase(decimal bonus, string reason)
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // First get current bonus amount
                string getQuery = "SELECT BONUS_AMOUNT FROM EMPLOYEE_SALARY WHERE EMPLOYEE_EMAIL = @email";
                SqlCommand getCmd = new SqlCommand(getQuery, con);
                getCmd.Parameters.AddWithValue("@email", employeeEmail);

                decimal currentBonus = 0;
                object result = getCmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    currentBonus = Convert.ToDecimal(result);
                }

                // Update with new bonus amount (ADD to existing bonus, not replace)
                string updateQuery = @"UPDATE EMPLOYEE_SALARY 
                                     SET BONUS_AMOUNT = @bonus, BONUS_REASON = @reason, PAYMENT_DATE = GETDATE()
                                     WHERE EMPLOYEE_EMAIL = @email";

                SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@bonus", currentBonus + bonus);
                updateCmd.Parameters.AddWithValue("@reason", reason);
                updateCmd.Parameters.AddWithValue("@email", employeeEmail);

                int rows = updateCmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show($"Bonus of {bonus:N0} added successfully!\nReason: {reason}\nTotal Bonus: {currentBonus + bonus:N0}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to add bonus.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    // Form for viewing bonus history
    public class BonusHistoryForm : Form
    {
        private DataGridView dataGridView1;
        private string employeeEmail;
        private string employeeName;

        public BonusHistoryForm(string email, string name)
        {
            employeeEmail = email;
            employeeName = name;
            InitializeComponent();
            LoadBonusHistory();
        }

        private void InitializeComponent()
        {
            this.dataGridView1 = new DataGridView();

            // dataGridView1
            this.dataGridView1.Location = new Point(20, 20);
            this.dataGridView1.Size = new Size(600, 300);
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Form settings
            this.Text = $"Bonus History - {employeeName}";
            this.Size = new Size(650, 400);
            this.Controls.Add(dataGridView1);
        }

        private void LoadBonusHistory()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = @"SELECT PAYMENT_DATE as [Payment Date], 
                                BONUS_AMOUNT as [Bonus Amount], 
                                BONUS_REASON as [Bonus Reason]
                                FROM EMPLOYEE_SALARY 
                                WHERE EMPLOYEE_EMAIL = @email AND BONUS_AMOUNT > 0
                                ORDER BY PAYMENT_DATE DESC";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                adapter.SelectCommand.Parameters.AddWithValue("@email", employeeEmail);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;

                // Format currency column
                if (dataGridView1.Columns["Bonus Amount"] != null)
                {
                    dataGridView1.Columns["Bonus Amount"].DefaultCellStyle.Format = "N0";
                    dataGridView1.Columns["Bonus Amount"].DefaultCellStyle.ForeColor = Color.Green;
                    dataGridView1.Columns["Bonus Amount"].DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                }
            }
        }
    }
}