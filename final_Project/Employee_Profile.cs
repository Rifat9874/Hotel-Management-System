using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace final_Project
{
    public partial class Employee_Profile : Form
    {
        string email, type;
        public Employee_Profile()
        {
            InitializeComponent();
        }

        public Employee_Profile(string email, string type)
        {
            this.type = type;
            this.email = email;
            InitializeComponent();
        }

        private void Employee_Profile_Load(object sender, EventArgs e)
        {
            getUser();
            LoadSalaryInfo();
            LoadLeaveInfo();

            if (type == "staff")
            {
                labelType.Text = "STAFF";
                this.Text = "Staff Profile";
            }
            else if (type == "receptionist")
            {
                labelType.Text = "RECEPTIONIST";
                this.Text = "Receptionist Profile";
            }
        }

        SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }

        void getUser()
        {
            try
            {
                SqlConnection conn = GetConnection();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT NAME, PHONE_NUMBER, EMAIL, ADDRESS, DEPARTMENT, POSITION, JOIN_DATE 
                    FROM USER_TABLE 
                    WHERE EMAIL = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtname.Text = reader["NAME"].ToString();
                    txtnumber.Text = reader["PHONE_NUMBER"].ToString();
                    txtemail.Text = reader["EMAIL"].ToString();

                    // Additional fields
                    if (reader["ADDRESS"] != DBNull.Value)
                        txtAddress.Text = reader["ADDRESS"].ToString();
                    else
                        txtAddress.Text = "Not specified";

                    if (reader["DEPARTMENT"] != DBNull.Value)
                        txtDepartment.Text = reader["DEPARTMENT"].ToString();
                    else
                        txtDepartment.Text = "Not specified";

                    if (reader["POSITION"] != DBNull.Value)
                        txtPosition.Text = reader["POSITION"].ToString();
                    else
                        txtPosition.Text = "Not specified";

                    if (reader["JOIN_DATE"] != DBNull.Value)
                        txtJoinDate.Text = Convert.ToDateTime(reader["JOIN_DATE"]).ToString("dd MMMM yyyy");
                    else
                        txtJoinDate.Text = "Not specified";
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LoadSalaryInfo()
        {
            try
            {
                SqlConnection conn = GetConnection();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 1 
                        SALARY_AMOUNT, 
                        PAYMENT_DATE, 
                        PAYMENT_METHOD, 
                        ALLOWANCES, 
                        DEDUCTIONS, 
                        BONUS_AMOUNT, 
                        BONUS_REASON,
                        (SALARY_AMOUNT + ALLOWANCES + ISNULL(BONUS_AMOUNT, 0) - DEDUCTIONS) AS NET_SALARY
                    FROM EMPLOYEE_SALARY 
                    WHERE EMPLOYEE_EMAIL = @Email
                    ORDER BY PAYMENT_DATE DESC", conn);
                cmd.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Basic salary information
                    decimal salaryAmount = Convert.ToDecimal(reader["SALARY_AMOUNT"]);
                    lblSalaryAmount.Text = FormatAsTK(salaryAmount);
                    lblLastSalaryDate.Text = Convert.ToDateTime(reader["PAYMENT_DATE"]).ToString("dd MMMM yyyy");

                    // Allowances and deductions
                    decimal allowances = Convert.ToDecimal(reader["ALLOWANCES"]);
                    decimal deductions = Convert.ToDecimal(reader["DEDUCTIONS"]);
                    lblAllowances.Text = FormatAsTK(allowances);
                    lblDeductions.Text = FormatAsTK(deductions);

                    // Payment method
                    if (reader["PAYMENT_METHOD"] != DBNull.Value)
                        lblPaymentMethod.Text = reader["PAYMENT_METHOD"].ToString();
                    else
                        lblPaymentMethod.Text = "N/A";

                    // Bonus information
                    if (reader["BONUS_AMOUNT"] != DBNull.Value)
                    {
                        decimal bonusAmount = Convert.ToDecimal(reader["BONUS_AMOUNT"]);
                        lblBonusAmount.Text = FormatAsTK(bonusAmount);

                        if (reader["BONUS_REASON"] != DBNull.Value)
                            lblBonusReason.Text = reader["BONUS_REASON"].ToString();
                        else
                            lblBonusReason.Text = "Not specified";
                    }
                    else
                    {
                        lblBonusAmount.Text = FormatAsTK(0);
                        lblBonusReason.Text = "No bonus";
                    }

                    // Net salary calculation
                    decimal netSalary = Convert.ToDecimal(reader["NET_SALARY"]);
                    lblNetSalary.Text = FormatAsTK(netSalary);
                }
                else
                {
                    // Set default values if no salary record found
                    lblSalaryAmount.Text = "N/A";
                    lblLastSalaryDate.Text = "N/A";
                    lblAllowances.Text = "N/A";
                    lblDeductions.Text = "N/A";
                    lblPaymentMethod.Text = "N/A";
                    lblBonusAmount.Text = "N/A";
                    lblBonusReason.Text = "N/A";
                    lblNetSalary.Text = "N/A";
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading salary information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to format numbers as TK (Bangladeshi Taka)
        private string FormatAsTK(decimal amount)
        {
            return amount.ToString("N2") + " TK";
        }

        void LoadLeaveInfo()
        {
            try
            {
                SqlConnection conn = GetConnection();

                // Get leave requests
                SqlCommand cmd = new SqlCommand(@"
                    SELECT LEAVE_ID, FROM_DATE, TO_DATE, REASON, STATUS 
                    FROM LEAVE_REQUEST 
                    WHERE EMAIL = @Email 
                    ORDER BY FROM_DATE DESC", conn);
                cmd.Parameters.AddWithValue("@Email", email);

                SqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);
                dgvLeaveHistory.DataSource = dt;

                if (dgvLeaveHistory.Columns.Count > 0)
                {
                    dgvLeaveHistory.Columns["LEAVE_ID"].HeaderText = "ID";
                    dgvLeaveHistory.Columns["FROM_DATE"].HeaderText = "From Date";
                    dgvLeaveHistory.Columns["TO_DATE"].HeaderText = "To Date";
                    dgvLeaveHistory.Columns["REASON"].HeaderText = "Reason";
                    dgvLeaveHistory.Columns["STATUS"].HeaderText = "Status";

                    dgvLeaveHistory.Columns["FROM_DATE"].DefaultCellStyle.Format = "dd MMM yyyy";
                    dgvLeaveHistory.Columns["TO_DATE"].DefaultCellStyle.Format = "dd MMM yyyy";
                }

                reader.Close();

                // Calculate leave statistics
                cmd = new SqlCommand(@"
                    SELECT 
                        COUNT(*) as TotalRequests,
                        SUM(CASE WHEN STATUS = 'APPROVED' THEN 1 ELSE 0 END) as ApprovedRequests,
                        SUM(CASE WHEN STATUS = 'PENDING' THEN 1 ELSE 0 END) as PendingRequests,
                        SUM(CASE WHEN STATUS = 'REJECTED' THEN 1 ELSE 0 END) as RejectedRequests,
                        SUM(CASE WHEN STATUS = 'APPROVED' THEN DATEDIFF(day, FROM_DATE, TO_DATE) + 1 ELSE 0 END) as TotalLeaveDays
                    FROM LEAVE_REQUEST 
                    WHERE EMAIL = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);

                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int totalLeaveDays = reader["TotalLeaveDays"] != DBNull.Value ? Convert.ToInt32(reader["TotalLeaveDays"]) : 0;
                    int approvedRequests = reader["ApprovedRequests"] != DBNull.Value ? Convert.ToInt32(reader["ApprovedRequests"]) : 0;
                    int pendingRequests = reader["PendingRequests"] != DBNull.Value ? Convert.ToInt32(reader["PendingRequests"]) : 0;
                    int rejectedRequests = reader["RejectedRequests"] != DBNull.Value ? Convert.ToInt32(reader["RejectedRequests"]) : 0;

                    int annualLeave = 20;
                    int remainingLeave = annualLeave - totalLeaveDays;

                    lblTotalLeave.Text = $"{annualLeave} days";
                    lblUsedLeave.Text = $"{totalLeaveDays} days";
                    lblRemainingLeave.Text = $"{remainingLeave} days";
                    lblApprovedRequests.Text = approvedRequests.ToString();
                    lblPendingRequests.Text = pendingRequests.ToString();
                    lblRejectedRequests.Text = rejectedRequests.ToString();
                }
                else
                {
                    lblTotalLeave.Text = "20 days";
                    lblUsedLeave.Text = "0 days";
                    lblRemainingLeave.Text = "20 days";
                    lblApprovedRequests.Text = "0";
                    lblPendingRequests.Text = "0";
                    lblRejectedRequests.Text = "0";
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading leave information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Back to Dashboard
            if (type == "staff")
            {
                StaffDashboard staffDashboard = new StaffDashboard(email);
                staffDashboard.Show();
            }
            else if (type == "receptionist")
            {
                Reciptionist receptionistDashboard = new Reciptionist(email);
                receptionistDashboard.Show();
            }
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Logout
            Login loginForm = new Login();
            loginForm.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Edit Info - Open the Edit_Info form
            Edit_Info editForm = new Edit_Info(email, type);
            editForm.Show();
            this.Hide();

            // When the edit form closes, refresh the data
            editForm.FormClosed += (s, args) => {
                this.Show();
                getUser();
                LoadSalaryInfo();
                LoadLeaveInfo();
            };
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Refresh data
            getUser();
            LoadSalaryInfo();
            LoadLeaveInfo();
        }

        private void txtname_Click(object sender, EventArgs e)
        {

        }

        private void Employee_Profile_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Show appropriate dashboard when profile is closed
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (type == "staff")
                {
                    StaffDashboard staffDashboard = new StaffDashboard(email);
                    staffDashboard.Show();
                }
                else if (type == "receptionist")
                {
                    Reciptionist receptionistDashboard = new Reciptionist(email);
                    receptionistDashboard.Show();
                }
            }
        }
    }
}