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
    public partial class showMYTask : Form
    {
        string email;

        public showMYTask()
        {
            InitializeComponent();
        }

        public showMYTask(string email)
        {
            InitializeComponent();
            this.email = email;
        }

        private void showMYTask_Load(object sender, EventArgs e)
        {
            loadmytask(email);
            LoadRoomNumbers();
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

        //load own task
        void loadmytask(string myEmail)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    string q = @"SELECT ROOM_ID, ROOM_STATUS, ASSIGNED_DATE
                         FROM ROOM_ASSIGNMENTS  
                         WHERE EMAIL = @myEmail 
                         ORDER BY ASSIGNED_DATE DESC";

                    using (SqlCommand cmd = new SqlCommand(q, con))
                    {
                        cmd.Parameters.AddWithValue("@myEmail", myEmail);

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            flowLayoutPanel1.Controls.Clear();

                            // Create header panel
                            Panel headerPanel = CreateHeaderPanel();
                            flowLayoutPanel1.Controls.Add(headerPanel);

                            bool hasData = false;

                            while (rdr.Read())
                            {
                                hasData = true;
                                string roomId = rdr["ROOM_ID"].ToString();
                                string status = rdr["ROOM_STATUS"].ToString();
                                DateTime assignedDate = rdr["ASSIGNED_DATE"] != DBNull.Value ? Convert.ToDateTime(rdr["ASSIGNED_DATE"]) : DateTime.Now;

                                Panel taskPanel = CreateTaskPanel(roomId, status, assignedDate);
                                flowLayoutPanel1.Controls.Add(taskPanel);
                            }

                            if (!hasData)
                            {
                                ShowNoDataMessage("No tasks assigned to you.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading tasks: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                Width = 650,
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
                Width = 650,
                Height = 50,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10),
                BackColor = Color.FromArgb(41, 53, 116)
            };

            Label RoomLabel = new Label
            {
                Text = "ROOM NO",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Top = 15,
                Left = 30
            };

            Label StatusLabel = new Label
            {
                Text = "STATUS",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Top = 15,
                Left = 150
            };

            Label DateLabel = new Label
            {
                Text = "ASSIGNED DATE",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Top = 15,
                Left = 280
            };

            Label ActionLabel = new Label
            {
                Text = "ACTION",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Top = 15,
                Left = 480
            };

            headerPanel.Controls.Add(RoomLabel);
            headerPanel.Controls.Add(StatusLabel);
            headerPanel.Controls.Add(DateLabel);
            headerPanel.Controls.Add(ActionLabel);

            return headerPanel;
        }

        // ADD THIS MISSING METHOD TO FIX THE ERROR
        private Panel CreateTaskPanel(string roomId, string status, DateTime assignedDate)
        {
            Panel taskPanel = new Panel
            {
                Width = 650,
                Height = 50,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10, 5, 10, 5),
                BackColor = Color.White
            };

            Label roomLabel = new Label
            {
                Text = roomId,
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Top = 15,
                Left = 30,
                Width = 80
            };

            Label statusLabel = new Label
            {
                Text = status,
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Top = 15,
                Left = 150,
                ForeColor = GetStatusColor(status),
                Width = 100
            };

            Label dateLabel = new Label
            {
                Text = assignedDate.ToString("yyyy-MM-dd"),
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Top = 15,
                Left = 280,
                Width = 120
            };

            // Only show Complete button for tasks that aren't already cleaned
            if (status.ToUpper() != "CLEANED")
            {
                Button completeBtn = new Button
                {
                    Text = "Complete",
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    BackColor = Color.FromArgb(76, 175, 80),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Width = 100,
                    Height = 30,
                    Top = 10,
                    Left = 480,
                    Tag = roomId
                };
                completeBtn.FlatAppearance.BorderSize = 0;
                completeBtn.Click += CompleteBtn_Click;

                taskPanel.Controls.Add(completeBtn);
            }
            else
            {
                Label completedLabel = new Label
                {
                    Text = "Completed",
                    Font = new Font("Segoe UI", 9, FontStyle.Italic),
                    ForeColor = Color.Green,
                    AutoSize = true,
                    Top = 15,
                    Left = 480
                };
                taskPanel.Controls.Add(completedLabel);
            }

            taskPanel.Controls.Add(roomLabel);
            taskPanel.Controls.Add(statusLabel);
            taskPanel.Controls.Add(dateLabel);

            return taskPanel;
        }

        // ADD THIS MISSING METHOD TO FIX THE ERROR
        private Color GetStatusColor(string status)
        {
            switch (status.ToUpper())
            {
                case "CLEANED":
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

        //Complete button for change status of room dirty or clean
        private void CompleteBtn_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            string selectedRoomId = clickedButton.Tag.ToString();

            try
            {
                using (SqlConnection con = GetConnection())
                {
                    // First verify the task belongs to this user
                    string verifyQuery = "SELECT COUNT(*) FROM ROOM_ASSIGNMENTS WHERE ROOM_ID = @roomId AND EMAIL = @email";
                    using (SqlCommand verifyCmd = new SqlCommand(verifyQuery, con))
                    {
                        verifyCmd.Parameters.AddWithValue("@roomId", selectedRoomId);
                        verifyCmd.Parameters.AddWithValue("@email", email);
                        int exists = (int)verifyCmd.ExecuteScalar();

                        if (exists == 0)
                        {
                            MessageBox.Show("This task doesn't belong to you or doesn't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Update the status
                    string updateQuery = "UPDATE ROOM_ASSIGNMENTS SET ROOM_STATUS = 'CLEANED' WHERE ROOM_ID = @roomId AND EMAIL = @email";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                    {
                        updateCmd.Parameters.AddWithValue("@roomId", selectedRoomId);
                        updateCmd.Parameters.AddWithValue("@email", email);
                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Room status updated to CLEANED successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loadmytask(email); // Reload tasks
                        }
                        else
                        {
                            MessageBox.Show("Failed to update room status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating room status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Dashboard Button
        private void button1_Click(object sender, EventArgs e)
        {
            StaffDashboard staffDashboard = new StaffDashboard(email);
            staffDashboard.Show();
            this.Hide();
        }

        //Apply Leave Button
        private void button3_Click(object sender, EventArgs e)
        {
            Staff_LeaveApplication leaveApplication = new Staff_LeaveApplication(email);
            leaveApplication.Show();
            this.Hide();
        }

        // Load room numbers for the dropdown
        private void LoadRoomNumbers()
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    string q = @"SELECT DISTINCT ROOM_ID FROM ROOM_ASSIGNMENTS WHERE EMAIL = @email ORDER BY ROOM_ID";
                    using (SqlCommand cmd = new SqlCommand(q, con))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            cmroomno.Items.Clear();
                            while (rdr.Read())
                            {
                                cmroomno.Items.Add(rdr["ROOM_ID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading room numbers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //send maintenance request 
        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmroomno.Text) || string.IsNullOrEmpty(cmproblem.Text))
            {
                MessageBox.Show("Please select a room and describe the problem.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = GetConnection())
                {
                    DateTime today = DateTime.Now;

                    // Generate maintenance ID
                    string IdQuery = "SELECT ISNULL(MAX(CAST(SUBSTRING(M_ID, 3, LEN(M_ID)) AS INT)), 0) + 1 FROM MaintenanceRequests";
                    SqlCommand cmd1 = new SqlCommand(IdQuery, con);
                    int nextId = (int)cmd1.ExecuteScalar();
                    string Id = "MR" + nextId.ToString("D3");

                    string insertQuery = @"INSERT INTO MaintenanceRequests (M_ID, ROOM_ID, PROBLEM, STATUS, SUBMIT_DATE, EMAIL) 
                               VALUES (@Id, @RoomId, @Problem, 'Pending', @Today, @Email)";

                    using (SqlCommand cmd2 = new SqlCommand(insertQuery, con))
                    {
                        cmd2.Parameters.AddWithValue("@Id", Id);
                        cmd2.Parameters.AddWithValue("@RoomId", cmroomno.Text);
                        cmd2.Parameters.AddWithValue("@Problem", cmproblem.Text);
                        cmd2.Parameters.AddWithValue("@Today", today);
                        cmd2.Parameters.AddWithValue("@Email", email);

                        int rowsAffected = cmd2.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Maintenance request submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Clear the form
                            cmroomno.SelectedIndex = -1;
                            cmproblem.SelectedIndex = -1;
                        }
                        else
                        {
                            MessageBox.Show("Failed to submit maintenance request.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error submitting maintenance request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            StaffDashboard staffDashboard = new StaffDashboard(email);
            staffDashboard.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Employee_Profile profile = new Employee_Profile(email, "staff");
            profile.Show();
            this.Hide();

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Staff_LeaveApplication staff_LeaveApplication = new Staff_LeaveApplication(email);
            staff_LeaveApplication.Show();
            this.Hide();
        }
    }
}