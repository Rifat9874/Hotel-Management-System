using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace final_Project
{
    public partial class Reciptionist : Form
    {
        string email;
        string receptionistName;

        public Reciptionist()
        {
            InitializeComponent();
        }

        public Reciptionist(string email)
        {
            this.email = email;
            InitializeComponent();
        }

        private void FlowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Optional: Add custom painting if needed
        }

        SqlConnection GetConnection()
        {
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }

        private void LoadReceptionistInfo()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    string query = "SELECT NAME FROM USER_TABLE WHERE EMAIL = @Email";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        receptionistName = result.ToString();
                        lblWelcome.Text = $"Welcome, {receptionistName}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading receptionist info: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Label CreateHeaderLabel(string text, int left, int fontSize)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                AutoSize = true,
                Left = left,
                Top = 15,
                ForeColor = Color.FromArgb(52, 73, 94)
            };
        }

        private Label CreateDataLabel(string text, int left, int fontSize)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", fontSize, FontStyle.Regular),
                AutoSize = true,
                Left = left,
                Top = 15,
                ForeColor = Color.FromArgb(44, 62, 80)
            };
        }

        // Update button
        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string selectedBookingId = clickedButton.Tag.ToString();
            Update_Booking_Status update_Booking_Status = new Update_Booking_Status(selectedBookingId, email);
            update_Booking_Status.Show();
            this.Visible = false;
        }

        // Check-Out button
        private void CheckOutBtn_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string selectedBookingId = clickedButton.Tag.ToString();

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    string query = "UPDATE BOOKINGS_TABLE SET BOOKING_STATUS = 'CHECK-OUT' WHERE BOOKING_ID = @BookingId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@BookingId", selectedBookingId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Guest checked out successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadBookings(); // Refresh the list
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during check-out: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Centralized function to get the correct SQL query
        private string GetBookingsQuery()
        {
            string selectedStatus = cmb.SelectedItem?.ToString();
            string query = @"SELECT RA.BOOKING_ID, RA.EMAIL, RA.ROOM_ID, RA.CHECK_IN, RA.CHECK_OUT, 
                             RA.BOOKING_STATUS, UT.NAME FROM BOOKINGS_TABLE RA 
                             JOIN USER_TABLE UT ON RA.EMAIL = UT.EMAIL WHERE 1=1";

            if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "Default Sorting")
            {
                query += $" AND RA.BOOKING_STATUS = '{selectedStatus}'";
            }

            query += " ORDER BY RA.CHECK_IN DESC";
            return query;
        }

        // Load all booking related information
        void LoadBookings()
        {
            try
            {
                flowLayoutPanel1.Controls.Clear();

                // Add header panel
                Panel headerPanel = new Panel
                {
                    Width = 1400,
                    Height = 50,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(10, 5, 10, 15),
                    BackColor = Color.FromArgb(236, 240, 241)
                };

                int fontSize = 11;
                headerPanel.Controls.Add(CreateHeaderLabel("Booking ID", 20, fontSize));
                headerPanel.Controls.Add(CreateHeaderLabel("Name", 120, fontSize));
                headerPanel.Controls.Add(CreateHeaderLabel("Email", 300, fontSize));
                headerPanel.Controls.Add(CreateHeaderLabel("Room No", 500, fontSize));
                headerPanel.Controls.Add(CreateHeaderLabel("Check-In", 600, fontSize));
                headerPanel.Controls.Add(CreateHeaderLabel("Check-Out", 750, fontSize));
                headerPanel.Controls.Add(CreateHeaderLabel("Status", 900, fontSize));
                headerPanel.Controls.Add(CreateHeaderLabel("Actions", 1100, fontSize));

                flowLayoutPanel1.Controls.Add(headerPanel);

                using (SqlConnection conn = GetConnection())
                {
                    string sqlQuery = GetBookingsQuery();
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (!rdr.HasRows)
                    {
                        Label emptyLabel = new Label
                        {
                            Text = "No bookings found with the selected criteria.",
                            Font = new Font("Segoe UI", 14, FontStyle.Italic),
                            ForeColor = Color.Gray,
                            AutoSize = true,
                            TextAlign = ContentAlignment.MiddleCenter
                        };
                        flowLayoutPanel1.Controls.Add(emptyLabel);
                        rdr.Close();
                        return;
                    }

                    while (rdr.Read())
                    {
                        string bookingId = rdr["BOOKING_ID"].ToString();
                        string guestEmail = rdr["EMAIL"].ToString();
                        string status = rdr["BOOKING_STATUS"].ToString();
                        string name = rdr["NAME"].ToString();
                        string roomId = rdr["ROOM_ID"].ToString();
                        string checkIn = Convert.ToDateTime(rdr["CHECK_IN"]).ToString("yyyy-MM-dd");
                        string checkOut = Convert.ToDateTime(rdr["CHECK_OUT"]).ToString("yyyy-MM-dd");

                        Panel taskPanel = new Panel
                        {
                            Width = 1400,
                            Height = 50,
                            BorderStyle = BorderStyle.None,
                            Margin = new Padding(10, 5, 10, 10),
                            BackColor = Color.White
                        };

                        // Add subtle shadow effect
                        taskPanel.Paint += (s, ev) =>
                        {
                            ev.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 220, 220), 1),
                                new Rectangle(0, 0, taskPanel.Width - 1, taskPanel.Height - 1));
                        };

                        // Update button
                        Button updateBtn = new Button
                        {
                            Text = "✏️ Update",
                            Font = new Font("Segoe UI", 9, FontStyle.Bold),
                            BackColor = Color.FromArgb(52, 152, 219),
                            ForeColor = Color.White,
                            FlatStyle = FlatStyle.Flat,
                            Width = 90,
                            Height = 30,
                            Top = 10,
                            Left = 1050,
                            Tag = bookingId,
                            Cursor = Cursors.Hand
                        };
                        updateBtn.FlatAppearance.BorderSize = 0;
                        updateBtn.Click += UpdateBtn_Click;

                        // Check-Out button (only show for CHECK-IN status)
                        Button checkOutBtn = new Button
                        {
                            Text = "🚪 Check-Out",
                            Font = new Font("Segoe UI", 9, FontStyle.Bold),
                            BackColor = Color.FromArgb(231, 76, 60),
                            ForeColor = Color.White,
                            FlatStyle = FlatStyle.Flat,
                            Width = 120,
                            Height = 30,
                            Top = 10,
                            Left = 1150,
                            Tag = bookingId,
                            Visible = (status == "CHECK-IN"),
                            Cursor = Cursors.Hand
                        };
                        checkOutBtn.FlatAppearance.BorderSize = 0;
                        checkOutBtn.Click += CheckOutBtn_Click;

                        taskPanel.Controls.Add(CreateDataLabel(bookingId, 20, fontSize));
                        taskPanel.Controls.Add(CreateDataLabel(name, 120, fontSize));
                        taskPanel.Controls.Add(CreateDataLabel(guestEmail, 300, fontSize));
                        taskPanel.Controls.Add(CreateDataLabel(roomId, 500, fontSize));
                        taskPanel.Controls.Add(CreateDataLabel(checkIn, 600, fontSize));
                        taskPanel.Controls.Add(CreateDataLabel(checkOut, 750, fontSize));

                        // Color code the status
                        Label statusLabel = new Label
                        {
                            Text = status,
                            Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                            AutoSize = true,
                            Left = 900,
                            Top = 15
                        };

                        // Set color based on status
                        switch (status)
                        {
                            case "CHECK-IN":
                                statusLabel.ForeColor = Color.FromArgb(46, 204, 113);
                                break;
                            case "PAID":
                                statusLabel.ForeColor = Color.FromArgb(52, 152, 219);
                                break;
                            case "PENDING":
                                statusLabel.ForeColor = Color.FromArgb(241, 196, 15);
                                break;
                            case "BOOKED":
                                statusLabel.ForeColor = Color.FromArgb(155, 89, 182);
                                break;
                            case "CHECK-OUT":
                                statusLabel.ForeColor = Color.FromArgb(149, 165, 166);
                                break;
                            default:
                                statusLabel.ForeColor = Color.Black;
                                break;
                        }

                        taskPanel.Controls.Add(statusLabel);
                        taskPanel.Controls.Add(updateBtn);
                        taskPanel.Controls.Add(checkOutBtn);

                        flowLayoutPanel1.Controls.Add(taskPanel);
                    }
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bookings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Reciptionist_Load(object sender, EventArgs e)
        {
            LoadReceptionistInfo();
            LoadBookings();
        }

        // Apply Leave button
        private void BtnApplyLeave_Click(object sender, EventArgs e)
        {
            Reciptionist_Leave reciptionist_Leave = new Reciptionist_Leave(email);
            reciptionist_Leave.Show();
            this.Visible = false;
        }

        // Profile picture click
        // In Reciptionist.cs - Update the PictureBox1_Click method
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            Employee_Profile profileForm = new Employee_Profile(email, "receptionist");
            profileForm.Show();
            this.Hide(); // Use Hide() instead of Visible = false for better navigation
        }

        // Add this method to handle back navigation from profile
        private void HandleProfileFormClosing()
        {
            this.Show(); // Show the dashboard when profile is closed
        }

        // Update the Employee_Profile form to properly handle navigation back

        private void Cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBookings();
        }

        // Refresh button
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadBookings();
        }

        // Logout button
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Login loginForm = new Login();
                loginForm.Show();
                this.Close();
            }
        }

        // Quick Check-Out button
        private void BtnQuickCheckOut_Click(object sender, EventArgs e)
        {
            QuickCheckOutForm checkOutForm = new QuickCheckOutForm(email);
            checkOutForm.ShowDialog();
            LoadBookings(); // Refresh after check-out
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Employee_Profile profile = new Employee_Profile(email, "receptionist");
            profile.Show();
            this.Hide();
        }
    }
}