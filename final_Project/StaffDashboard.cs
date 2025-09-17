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
    public partial class StaffDashboard : Form
    {
        string Uemail, roomId, name;

        public StaffDashboard()
        {
            InitializeComponent();
        }

        public StaffDashboard(string email)
        {
            this.Uemail = email;
            InitializeComponent();
        }

        //Apply Leave button
        private void button3_Click(object sender, EventArgs e)
        {
            Staff_LeaveApplication leaveApplication = new Staff_LeaveApplication(Uemail);
            leaveApplication.Show();
            this.Hide();
        }

        //Show Profile button
        private void btnUserProfile_Click(object sender, EventArgs e)
        {
            Employee_Profile profile = new Employee_Profile(Uemail, "staff");
            profile.Show();
            this.Hide();
        }

        //My Task button
        private void button2_Click(object sender, EventArgs e)
        {
            showMYTask showMYTaskForm = new showMYTask(Uemail);
            showMYTaskForm.Show();
            this.Hide();
        }

        //search button
        private void btnsearch_Click(object sender, EventArgs e)
        {
            // Only search if it's not placeholder text
            if (txtsearch.Text != "Search tasks...")
            {
                string searchedName = txtsearch.Text;
                load_room_assignments(searchedName);
            }
            else
            {
                load_room_assignments();
            }
        }

        //Dashboard button
        private void button1_Click_1(object sender, EventArgs e)
        {
            load_room_assignments();
        }

        private void StaffDashboard_Load(object sender, EventArgs e)
        {
            load_room_assignments();
            getUser();
            lblWelcome.Text = $"Welcome, {GetUserName()}!";

            // Set placeholder text
            txtsearch_Leave(null, null);
        }

        // Logout functionality
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Close();
        }

        // Placeholder text functionality for search
        private void txtsearch_Enter(object sender, EventArgs e)
        {
            if (txtsearch.Text == "Search tasks...")
            {
                txtsearch.Text = "";
                txtsearch.ForeColor = SystemColors.ControlText;
            }
        }

        private void txtsearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtsearch.Text))
            {
                txtsearch.Text = "Search tasks...";
                txtsearch.ForeColor = SystemColors.GrayText;
            }
        }

        private void txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow search on Enter key press
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnsearch_Click(sender, e);
                e.Handled = true; // Prevent the beep sound
            }
        }

        SqlConnection GetConnection()
        {
            // Added error handling for connection
            try
            {
                string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                return con;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        void getUser()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    if (conn == null) return;

                    SqlCommand cmd = new SqlCommand("SELECT NAME FROM USER_TABLE WHERE EMAIL = @email", conn);
                    cmd.Parameters.AddWithValue("@email", Uemail);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtuser.Text = reader["NAME"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        string GetUserName()
        {
            string userName = "";
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    if (conn == null) return userName;

                    SqlCommand cmd = new SqlCommand("SELECT NAME FROM USER_TABLE WHERE EMAIL = @email", conn);
                    cmd.Parameters.AddWithValue("@email", Uemail);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userName = reader["NAME"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting user name: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return userName;
        }

        //load all task for all from DB
        void load_room_assignments(string nameFilter = "")
        {
            flowLayoutPanel1.Controls.Clear();

            // Create header panel
            Panel headerPanel = CreateHeaderPanel();
            flowLayoutPanel1.Controls.Add(headerPanel);

            try
            {
                using (SqlConnection con = GetConnection())
                {
                    if (con == null) return;

                    string q = @"SELECT RA.ASSIGNMENT_ID, RA.ROOM_ID, RA.ASSIGNED_DATE, RA.EMAIL, RA.ROOM_STATUS, UT.NAME 
                         FROM ROOM_ASSIGNMENTS RA
                         JOIN USER_TABLE UT ON RA.EMAIL = UT.EMAIL
                         WHERE (@nameFilter = '' OR UT.NAME LIKE @nameFilter)";

                    using (SqlCommand cmd = new SqlCommand(q, con))
                    {
                        cmd.Parameters.AddWithValue("@nameFilter", string.IsNullOrEmpty(nameFilter) ? "" : "%" + nameFilter + "%");

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            bool hasData = false;
                            while (rdr.Read())
                            {
                                hasData = true;
                                roomId = rdr["ROOM_ID"].ToString();
                                string email = rdr["EMAIL"].ToString();
                                string status = rdr["ROOM_STATUS"].ToString();
                                name = rdr["NAME"].ToString();
                                DateTime assignedDate = rdr["ASSIGNED_DATE"] != DBNull.Value ? Convert.ToDateTime(rdr["ASSIGNED_DATE"]) : DateTime.Now;

                                Panel taskPanel = CreateTaskPanel(name, email, roomId, status, assignedDate);
                                flowLayoutPanel1.Controls.Add(taskPanel);
                            }

                            if (!hasData)
                            {
                                ShowNoDataMessage("No room assignments found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading room assignments: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Refresh the layout
            flowLayoutPanel1.PerformLayout();
        }

        private void ShowNoDataMessage(string message)
        {
            Label noDataLabel = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.Gray,
                AutoSize = true,
                Top = 20,
                Left = 20
            };

            Panel messagePanel = new Panel
            {
                Width = flowLayoutPanel1.Width - 40,
                Height = 60,
                BorderStyle = BorderStyle.None
            };
            messagePanel.Controls.Add(noDataLabel);
            flowLayoutPanel1.Controls.Add(messagePanel);
        }

        private Panel CreateHeaderPanel()
        {
            Panel headerPanel = new Panel
            {
                Width = flowLayoutPanel1.Width - 25, // Adjust for scrollbar
                Height = 50,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10),
                BackColor = Color.FromArgb(41, 53, 116)
            };

            // Calculate widths based on panel width
            int col1 = 30;
            int col2 = (int)(headerPanel.Width * 0.25);
            int col3 = (int)(headerPanel.Width * 0.5);
            int col4 = (int)(headerPanel.Width * 0.65);
            int col5 = (int)(headerPanel.Width * 0.8);

            Label NameLabel = new Label
            {
                Text = "NAME",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Width = 150,
                Top = 15,
                Left = col1
            };

            Label EmailLabel = new Label
            {
                Text = "EMAIL",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Width = 180,
                Top = 15,
                Left = col2
            };

            Label RoomLabel = new Label
            {
                Text = "ROOM NO",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Width = 80,
                Top = 15,
                Left = col3
            };

            Label StatusLabel = new Label
            {
                Text = "STATUS",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Width = 100,
                Top = 15,
                Left = col4
            };

            Label DateLabel = new Label
            {
                Text = "ASSIGNED DATE",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Width = 120,
                Top = 15,
                Left = col5
            };

            headerPanel.Controls.Add(NameLabel);
            headerPanel.Controls.Add(EmailLabel);
            headerPanel.Controls.Add(RoomLabel);
            headerPanel.Controls.Add(StatusLabel);
            headerPanel.Controls.Add(DateLabel);

            return headerPanel;
        }

        private Panel CreateTaskPanel(string name, string email, string roomId, string status, DateTime assignedDate)
        {
            Panel taskPanel = new Panel
            {
                Width = flowLayoutPanel1.Width - 25, // Adjust for scrollbar
                Height = 50,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10, 5, 10, 5),
                BackColor = Color.White
            };

            // Calculate widths based on panel width (same as header)
            int col1 = 30;
            int col2 = (int)(taskPanel.Width * 0.25);
            int col3 = (int)(taskPanel.Width * 0.5);
            int col4 = (int)(taskPanel.Width * 0.65);
            int col5 = (int)(taskPanel.Width * 0.8);

            Label nameLabel = new Label
            {
                Text = name,
                Font = new Font("Segoe UI", 10),
                AutoSize = false,
                Width = 150,
                Top = 15,
                Left = col1
            };

            Label emailLabel = new Label
            {
                Text = email,
                Font = new Font("Segoe UI", 10),
                AutoSize = false,
                Width = 180,
                Top = 15,
                Left = col2
            };

            Label roomLabel = new Label
            {
                Text = roomId,
                Font = new Font("Segoe UI", 10),
                AutoSize = false,
                Width = 80,
                Top = 15,
                Left = col3
            };

            Label statusLabel = new Label
            {
                Text = status,
                Font = new Font("Segoe UI", 10),
                AutoSize = false,
                Width = 100,
                Top = 15,
                Left = col4,
                ForeColor = GetStatusColor(status)
            };

            Label dateLabel = new Label
            {
                Text = assignedDate.ToString("yyyy-MM-dd"),
                Font = new Font("Segoe UI", 10),
                AutoSize = false,
                Width = 120,
                Top = 15,
                Left = col5
            };

            taskPanel.Controls.Add(nameLabel);
            taskPanel.Controls.Add(emailLabel);
            taskPanel.Controls.Add(roomLabel);
            taskPanel.Controls.Add(statusLabel);
            taskPanel.Controls.Add(dateLabel);

            return taskPanel;
        }

        private Color GetStatusColor(string status)
        {
            switch (status.ToUpper())
            {
                case "CLEANED":
                case "COMPLETED":
                    return Color.Green;
                case "IN PROGRESS":
                    return Color.Orange;
                case "PENDING":
                case "DIRTY":
                    return Color.Red;
                default:
                    return Color.Black;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void txtuser_Click(object sender, EventArgs e) { }

        private void button4_Click(object sender, EventArgs e)
        {
            Employee_Profile profile = new Employee_Profile(Uemail, "staff");
            profile.Show();
            this.Hide();
        }

        // Handle form closing to prevent application from running in background
        private void StaffDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Application.OpenForms.Count == 1 && Application.OpenForms[0] is Login)
            {
                // This is the last form, allow closing
                return;
            }

            // If not logging out, ask for confirmation
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Application.Exit();
                }
            }
        }
    }
}