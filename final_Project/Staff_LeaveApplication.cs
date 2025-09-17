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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace final_Project
{
    public partial class Staff_LeaveApplication : Form
    {
        string email = "";
        public Staff_LeaveApplication()
        {
            InitializeComponent();
        }
        public Staff_LeaveApplication(string email)
        {
            this.email = email;
            InitializeComponent();
        }

        private void LeaveApplication_Load(object sender, EventArgs e)
        {
            load_history();
            txtfromdate.MinDate = DateTime.Today;
            txttodate.MinDate = DateTime.Today;
        }

        // Logout functionality
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Close();
        }

        SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }

        //submit button (detail about leave)
        private void btnsubmit_Click(object sender, EventArgs e)
        {
            // Validation
            if (txtfromdate.Value > txttodate.Value)
            {
                MessageBox.Show("To date cannot be earlier than From date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(cmreason.Text))
            {
                MessageBox.Show("Please select a reason for leave.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = GetConnection())
                {
                    // First, verify the email exists in USER_TABLE
                    if (!EmailExistsInUserTable(email, con))
                    {
                        MessageBox.Show("Your email is not registered in the system. Please contact administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Generate leave ID
                    string IdQuery = "SELECT ISNULL(MAX(CAST(SUBSTRING(LEAVE_ID, 4, LEN(LEAVE_ID)) AS INT)), 0) + 1 FROM LEAVE_REQUEST";
                    SqlCommand cmd1 = new SqlCommand(IdQuery, con);
                    int nextId = (int)cmd1.ExecuteScalar();
                    string Id = "LV" + nextId.ToString("D3"); // Format as LV001, LV002, etc.

                    string q = @"INSERT INTO LEAVE_REQUEST (LEAVE_ID, EMAIL, FROM_DATE, TO_DATE, REASON, STATUS) 
                     VALUES (@Id, @Email, @FromDate, @ToDate, @Reason, 'Pending')";

                    using (SqlCommand cmd2 = new SqlCommand(q, con))
                    {
                        cmd2.Parameters.AddWithValue("@Id", Id);
                        cmd2.Parameters.AddWithValue("@Email", email);
                        cmd2.Parameters.AddWithValue("@FromDate", txtfromdate.Value.ToString("yyyy-MM-dd"));
                        cmd2.Parameters.AddWithValue("@ToDate", txttodate.Value.ToString("yyyy-MM-dd"));
                        cmd2.Parameters.AddWithValue("@Reason", cmreason.Text);

                        int rowsAffected = cmd2.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Leave application submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            load_history();
                            // Clear selection
                            cmreason.SelectedIndex = -1;
                        }
                        else
                        {
                            MessageBox.Show("Failed to submit leave application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error submitting leave application: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to check if email exists in USER_TABLE
        private bool EmailExistsInUserTable(string email, SqlConnection con)
        {
            try
            {
                string checkQuery = "SELECT COUNT(1) FROM USER_TABLE WHERE EMAIL = @Email";
                using (SqlCommand cmd = new SqlCommand(checkQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking email existence: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //load previous booking history
        void load_history()
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    string q = @"SELECT 
                                RA.FROM_DATE,
                                RA.TO_DATE,
                                RA.REASON,
                                RA.STATUS,
                                UT.NAME 
                            FROM LEAVE_REQUEST RA
                            JOIN USER_TABLE UT ON RA.EMAIL = UT.EMAIL
                            WHERE RA.EMAIL = @Email
                            ORDER BY RA.FROM_DATE DESC";

                    using (SqlCommand cmd = new SqlCommand(q, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            flowLayoutPanel2.Controls.Clear();

                            // Create header panel
                            Panel headerPanel = CreateHeaderPanel();
                            flowLayoutPanel2.Controls.Add(headerPanel);

                            while (rdr.Read())
                            {
                                string name = rdr["NAME"].ToString();
                                DateTime fromDate = Convert.ToDateTime(rdr["FROM_DATE"]);
                                DateTime toDate = Convert.ToDateTime(rdr["TO_DATE"]);
                                string reason = rdr["REASON"].ToString();
                                string status = rdr["STATUS"].ToString();

                                Panel leavePanel = CreateLeavePanel(name, fromDate, toDate, reason, status);
                                flowLayoutPanel2.Controls.Add(leavePanel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading leave history: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CreateHeaderPanel()
        {
            Panel headerPanel = new Panel
            {
                Width = 800,
                Height = 50,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(5),
                BackColor = Color.FromArgb(41, 53, 116)
            };

            Label nameLabel = CreateLabel("NAME", 20, 12, true, Color.White);
            Label fromLabel = CreateLabel("FROM", 150, 12, true, Color.White);
            Label toLabel = CreateLabel("TO", 250, 12, true, Color.White);
            Label daysLabel = CreateLabel("DAYS", 350, 12, true, Color.White);
            Label reasonLabel = CreateLabel("REASON", 420, 12, true, Color.White);
            Label statusLabel = CreateLabel("STATUS", 650, 12, true, Color.White);

            headerPanel.Controls.Add(nameLabel);
            headerPanel.Controls.Add(fromLabel);
            headerPanel.Controls.Add(toLabel);
            headerPanel.Controls.Add(daysLabel);
            headerPanel.Controls.Add(reasonLabel);
            headerPanel.Controls.Add(statusLabel);

            return headerPanel;
        }

        // ADD THIS MISSING METHOD TO FIX THE ERROR
        private Panel CreateLeavePanel(string name, DateTime fromDate, DateTime toDate, string reason, string status)
        {
            Panel leavePanel = new Panel
            {
                Width = 800,
                Height = 50,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5, 2, 5, 2),
                BackColor = Color.White
            };

            int days = (toDate - fromDate).Days + 1; // Inclusive of both dates

            Label nameLabel = CreateLabel(name, 20, 10);
            Label fromLabel = CreateLabel(fromDate.ToString("yyyy-MM-dd"), 150, 10);
            Label toLabel = CreateLabel(toDate.ToString("yyyy-MM-dd"), 250, 10);
            Label daysLabel = CreateLabel(days.ToString() + " day(s)", 350, 10);
            Label reasonLabel = CreateLabel(reason, 420, 10);

            Label statusLabel = CreateLabel(status, 650, 10);
            statusLabel.ForeColor = GetStatusColor(status);

            leavePanel.Controls.Add(nameLabel);
            leavePanel.Controls.Add(fromLabel);
            leavePanel.Controls.Add(toLabel);
            leavePanel.Controls.Add(daysLabel);
            leavePanel.Controls.Add(reasonLabel);
            leavePanel.Controls.Add(statusLabel);

            return leavePanel;
        }

        // ADD THIS MISSING METHOD TO FIX THE ERROR
        private Color GetStatusColor(string status)
        {
            switch (status.ToUpper())
            {
                case "APPROVED":
                    return Color.Green;
                case "PENDING":
                    return Color.Orange;
                case "REJECTED":
                    return Color.Red;
                default:
                    return Color.Black;
            }
        }

        private Label CreateLabel(string text, int left, int fontSize = 10, bool bold = false, Color? foreColor = null)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", fontSize, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = foreColor ?? Color.Black,
                AutoSize = true,
                Top = 15,
                Left = left
            };
        }

        // Update To date minimum date when From date changes
        private void txtfromdate_ValueChanged(object sender, EventArgs e)
        {
            txttodate.MinDate = txtfromdate.Value;
        }

        //Dashboard Button
        private void button1_Click(object sender, EventArgs e)
        {
            StaffDashboard staffDashboard = new StaffDashboard(email);
            staffDashboard.Show();
            this.Visible = false;
        }

        //My Task Button 
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            StaffDashboard std = new StaffDashboard(email);
            std.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            showMYTask showMYTask = new showMYTask(email);
            showMYTask.Show();
            this.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Staff_LeaveApplication staff_LeaveApplication = new Staff_LeaveApplication(email);
            staff_LeaveApplication.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Employee_Profile profile = new Employee_Profile(email, "staff");
            profile.Show();
            this.Hide();
        }
    }
}