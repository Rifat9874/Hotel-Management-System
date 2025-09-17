using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace final_Project
{
    public partial class Admin_Dashboard : Form
    {
        private string email = "";

        public Admin_Dashboard()
        {
            InitializeComponent();
        }

        public Admin_Dashboard(string email)
        {
            this.email = email;
            InitializeComponent();
        }

        private void Admin_Dashboard_Load(object sender, EventArgs e)
        {
            load_occupied_rooms();
            load_booking_history();
            load_dashboard_counts();
        }

        SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }

        private Label CreateLabel(string text, int fontSize, FontStyle fontStyle, int left, int top)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", fontSize, fontStyle),
                AutoSize = true,
                Left = left,
                Top = top
            };
        }

        // Load Occupied Rooms
        void load_occupied_rooms()
        {
            SqlConnection con = GetConnection();
            string query = @"SELECT RA.ROOM_ID, SA.NAME 
                             FROM BOOKINGS_TABLE RA 
                             JOIN USER_TABLE SA ON RA.EMAIL = SA.EMAIL 
                             WHERE RA.BOOKING_STATUS = 'CHECK-IN'";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader rdr = cmd.ExecuteReader();

            flowLayoutPanel1.Controls.Clear();

            Panel titlePanel = new Panel
            {
                Width = 240,
                Height = 40,
                Margin = new Padding(2)
            };
            titlePanel.Controls.Add(CreateLabel("Occupied Rooms", 18, FontStyle.Bold, 25, 0));
            flowLayoutPanel1.Controls.Add(titlePanel);

            Panel headerPanel = new Panel
            {
                Width = 240,
                Height = 30,
                Margin = new Padding(2)
            };
            headerPanel.Controls.Add(CreateLabel("NAME", 14, FontStyle.Bold, 20, 0));
            headerPanel.Controls.Add(CreateLabel("ROOM NO", 14, FontStyle.Bold, 125, 0));
            headerPanel.Controls.Add(new Panel
            {
                Height = 2,
                Dock = DockStyle.Bottom,
                BackColor = Color.Gray
            });
            flowLayoutPanel1.Controls.Add(headerPanel);

            while (rdr.Read())
            {
                string roomId = rdr["ROOM_ID"].ToString();
                string name = rdr["NAME"].ToString();

                Panel taskPanel = new Panel
                {
                    Width = 240,
                    Height = 30,
                    Margin = new Padding(3)
                };
                taskPanel.Controls.Add(CreateLabel(name, 12, FontStyle.Bold, 0, 0));
                taskPanel.Controls.Add(CreateLabel(roomId, 12, FontStyle.Bold, 145, 0));
                flowLayoutPanel1.Controls.Add(taskPanel);
            }
            con.Close();
        }

        // Load Booking History
        void load_booking_history()
        {
            SqlConnection con = GetConnection();
            string query = @"SELECT RA.ROOM_ID, RA.BOOKING_DATE, SA.NAME 
                             FROM BOOKINGS_TABLE RA 
                             JOIN USER_TABLE SA ON RA.EMAIL = SA.EMAIL";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader rdr = cmd.ExecuteReader();

            flowLayoutPanel2.Controls.Clear();

            Panel titlePanel = new Panel
            {
                Width = 530,
                Height = 40,
                Margin = new Padding(2)
            };
            titlePanel.Controls.Add(CreateLabel("Booking History", 18, FontStyle.Bold, 120, 0));
            flowLayoutPanel2.Controls.Add(titlePanel);

            Panel headerPanel = new Panel
            {
                Width = 530,
                Height = 30,
                Margin = new Padding(5)
            };
            headerPanel.Controls.Add(CreateLabel("NAME", 14, FontStyle.Bold, 10, 0));
            headerPanel.Controls.Add(CreateLabel("Booking Date", 14, FontStyle.Bold, 181, 0));
            headerPanel.Controls.Add(CreateLabel("ROOM NO", 14, FontStyle.Bold, 395, 0));
            headerPanel.Controls.Add(new Panel
            {
                Height = 2,
                Dock = DockStyle.Bottom,
                BackColor = Color.Gray
            });
            flowLayoutPanel2.Controls.Add(headerPanel);

            while (rdr.Read())
            {
                string name = rdr["NAME"].ToString();
                string bookingDate = rdr["BOOKING_DATE"].ToString();
                string roomId = rdr["ROOM_ID"].ToString();

                Panel taskPanel = new Panel { Width = 500, Height = 30, Margin = new Padding(10) };
                taskPanel.Controls.Add(CreateLabel(name, 12, FontStyle.Bold, 0, 0));
                taskPanel.Controls.Add(CreateLabel(bookingDate, 14, FontStyle.Bold, 145, 0));
                taskPanel.Controls.Add(CreateLabel(roomId, 12, FontStyle.Bold, 425, 0));
                flowLayoutPanel2.Controls.Add(taskPanel);
            }
            con.Close();
        }

        // Load Dashboard Counts
        void load_dashboard_counts()
        {
            SqlConnection con = GetConnection();

            // 1. Total Bookings
            SqlCommand totalBookingsCmd = new SqlCommand("SELECT COUNT(*) FROM BOOKINGS_TABLE", con);
            int totalBookings = (int)totalBookingsCmd.ExecuteScalar();
            counttb.Text = totalBookings.ToString();

            // 2. Total Occupied Rooms (CHECK-IN status)
            SqlCommand occupiedRoomsCmd = new SqlCommand("SELECT COUNT(*) FROM BOOKINGS_TABLE WHERE BOOKING_STATUS = 'CHECK-IN'", con);
            int occupiedRooms = (int)occupiedRoomsCmd.ExecuteScalar();
            counttor.Text = occupiedRooms.ToString();

            // 3. Today's Check-Ins (Check current date)
            SqlCommand todaysCheckInCmd = new SqlCommand("SELECT COUNT(*) FROM BOOKINGS_TABLE WHERE CAST(CHECK_IN AS DATE) = CAST(GETDATE() AS DATE)", con);
            int todaysCheckIn = (int)todaysCheckInCmd.ExecuteScalar();
            counttci.Text = todaysCheckIn.ToString();

            // 4. Today's Revenue (calculated from room rates and duration)
            SqlCommand todayRevenueCmd = new SqlCommand(@"
        SELECT ISNULL(SUM(DATEDIFF(DAY, B.CHECK_IN, B.CHECK_OUT) * R.PPN), 0) 
        FROM BOOKINGS_TABLE B
        JOIN ROOM_TABLE R ON B.ROOM_ID = R.ROOM_ID
        WHERE CAST(B.BOOKING_DATE AS DATE) = CAST(GETDATE() AS DATE)", con);

            decimal todayRevenue = Convert.ToDecimal(todayRevenueCmd.ExecuteScalar());
            lblRevenue.Text = "TK " + todayRevenue.ToString("N2");

            con.Close();
        }

        // ===== NEW METHODS FOR REVIEWS AND FACILITIES =====

        // Reviews Button Click
        private void btnReviews_Click(object sender, EventArgs e)
        {
            ReviewsManagementForm reviewsForm = new ReviewsManagementForm();
            reviewsForm.ShowDialog();
        }

        // Facilities Button Click
        private void btnFacilities_Click(object sender, EventArgs e)
        {
            FacilitiesManagementForm facilitiesForm = new FacilitiesManagementForm();
            facilitiesForm.ShowDialog();
        }

        // Revenue Button Click - Show Revenue Report
        private void btnRevenue_Click(object sender, EventArgs e)
        {
            RevenueReportForm revenueForm = new RevenueReportForm();
            revenueForm.ShowDialog();
        }

        // Reports Button Click - Show Reports Form
        private void btnReports_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            reportsForm.ShowDialog();
        }

        // Leave Request button
        private void button1_Click(object sender, EventArgs e)
        {
            Handle_Leave_RequestcsAdmin handle_Leave_Requestcs = new Handle_Leave_RequestcsAdmin();
            handle_Leave_Requestcs.Show();
            this.Visible = false;
        }

        // Manage Rooms button
        private void button2_Click(object sender, EventArgs e)
        {
            Handle_Rooms_Admin handle_Maintainence = new Handle_Rooms_Admin();
            handle_Maintainence.Show();
            this.Visible = false;
        }

        // Log Out button
        private void button3_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Visible = false;
        }

        // Manage User button
        private void button4_Click(object sender, EventArgs e)
        {
            Manage_Users_Admin manage_Users_Admin = new Manage_Users_Admin();
            manage_Users_Admin.Show();
            this.Visible = false;
        }

        // Assign Staff Task button
        private void button5_Click(object sender, EventArgs e)
        {
            Assign_Staff_Task assign_Staff_Task = new Assign_Staff_Task();
            assign_Staff_Task.Show();
            this.Visible = false;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {

        }
    }

    // ===== NEW FORMS FOR REVIEWS AND FACILITIES MANAGEMENT =====

    // Reviews Management Form
    public class ReviewsManagementForm : Form
    {
        private DataGridView dgvReviews;
        private Button btnClose;
        private Button btnDelete;
        private Label label1;

        public ReviewsManagementForm()
        {
            InitializeComponent();
            LoadReviewsData();
        }

        private void InitializeComponent()
        {
            this.Text = "Reviews Management - Hotel Marina";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));

            label1 = new Label();
            label1.Text = "CUSTOMER REVIEWS";
            label1.Font = new Font("Segoe Print", 18, FontStyle.Bold);
            label1.ForeColor = Color.MistyRose;
            label1.AutoSize = true;
            label1.Location = new Point(350, 20);
            this.Controls.Add(label1);

            dgvReviews = new DataGridView();
            dgvReviews.Size = new Size(940, 400);
            dgvReviews.Location = new Point(30, 80);
            dgvReviews.BackgroundColor = Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            dgvReviews.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReviews.ReadOnly = true;
            dgvReviews.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Controls.Add(dgvReviews);

            btnDelete = new Button();
            btnDelete.Text = "Delete Selected";
            btnDelete.Size = new Size(150, 40);
            btnDelete.Location = new Point(30, 500);
            btnDelete.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Size = new Size(100, 40);
            btnClose.Location = new Point(850, 500);
            btnClose.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private void LoadReviewsData()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string query = @"
                SELECT 
                    R.REVIEW_ID,
                    U.NAME AS CustomerName,
                    B.ROOM_ID,
                    R.RATING,
                    R.COMMENTS,
                    R.REVIEW_DATE,
                    B.BOOKING_ID,
                    U.EMAIL
                FROM REVIEWS_TABLE R
                JOIN USER_TABLE U ON R.EMAIL = U.EMAIL
                JOIN BOOKINGS_TABLE B ON R.BOOKING_ID = B.BOOKING_ID
                ORDER BY R.REVIEW_DATE DESC";

                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dgvReviews.DataSource = table;

                    // Format the rating column with stars
                    if (dgvReviews.Columns.Contains("RATING"))
                    {
                        dgvReviews.Columns["RATING"].HeaderText = "Rating";
                    }

                    MessageBox.Show($"Loaded {table.Rows.Count} reviews", "Reviews Loaded",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading reviews: {ex.Message}\n\nPlease make sure the REVIEWS_TABLE exists in your database.",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvReviews.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a review to delete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete the selected review?", "Confirm Delete",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                int reviewId = Convert.ToInt32(dgvReviews.SelectedRows[0].Cells["REVIEW_ID"].Value);

                string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();
                        string query = "DELETE FROM REVIEWS_TABLE WHERE REVIEW_ID = @ReviewId";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@ReviewId", reviewId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Review deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadReviewsData(); // Refresh the data
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting review: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }

    // Facilities Management Form
    public class FacilitiesManagementForm : Form
    {
        private DataGridView dgvFacilities;
        private Button btnClose;
        private Button btnUpdateStatus;
        private ComboBox cbStatus;
        private Label label1;
        private Label label2;

        public FacilitiesManagementForm()
        {
            InitializeComponent();
            LoadFacilitiesData();
        }

        private void InitializeComponent()
        {
            this.Text = "Facilities Bookings - Hotel Marina";
            this.Size = new Size(1100, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));

            label1 = new Label();
            label1.Text = "FACILITIES BOOKINGS";
            label1.Font = new Font("Segoe Print", 18, FontStyle.Bold);
            label1.ForeColor = Color.MistyRose;
            label1.AutoSize = true;
            label1.Location = new Point(380, 20);
            this.Controls.Add(label1);

            dgvFacilities = new DataGridView();
            dgvFacilities.Size = new Size(1040, 400);
            dgvFacilities.Location = new Point(30, 80);
            dgvFacilities.BackgroundColor = Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            dgvFacilities.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFacilities.ReadOnly = true;
            dgvFacilities.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Controls.Add(dgvFacilities);

            label2 = new Label();
            label2.Text = "Update Status:";
            label2.ForeColor = Color.White;
            label2.Location = new Point(30, 500);
            this.Controls.Add(label2);

            cbStatus = new ComboBox();
            cbStatus.Items.AddRange(new object[] { "PENDING", "CONFIRMED", "COMPLETED", "CANCELLED" });
            cbStatus.Location = new Point(140, 500);
            cbStatus.Size = new Size(120, 24);
            this.Controls.Add(cbStatus);

            btnUpdateStatus = new Button();
            btnUpdateStatus.Text = "Update Status";
            btnUpdateStatus.Size = new Size(150, 40);
            btnUpdateStatus.Location = new Point(280, 495);
            btnUpdateStatus.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            btnUpdateStatus.FlatStyle = FlatStyle.Flat;
            btnUpdateStatus.Click += BtnUpdateStatus_Click;
            this.Controls.Add(btnUpdateStatus);

            btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Size = new Size(100, 40);
            btnClose.Location = new Point(950, 500);
            btnClose.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private void LoadFacilitiesData()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string query = @"
                SELECT 
                    FB.FACILITY_BOOKING_ID,
                    U.NAME AS CustomerName,
                    FB.FACILITY_NAME,
                    FB.BOOKING_DATE,
                    FB.GUESTS_COUNT,
                    FB.TOTAL_PRICE,
                    FB.BOOKING_STATUS,
                    FB.BOOKING_TIME,
                    U.EMAIL
                FROM FACILITIES_BOOKING FB
                JOIN USER_TABLE U ON FB.EMAIL = U.EMAIL
                ORDER BY FB.BOOKING_DATE DESC";

                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    // Format the price column
                    table.Columns.Add("FormattedPrice", typeof(string));
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["TOTAL_PRICE"] != DBNull.Value)
                        {
                            decimal price = Convert.ToDecimal(row["TOTAL_PRICE"]);
                            row["FormattedPrice"] = "TK " + price.ToString("N2");
                        }
                    }

                    dgvFacilities.DataSource = table;

                    // Hide the original price column and show formatted one
                    if (dgvFacilities.Columns.Contains("TOTAL_PRICE"))
                        dgvFacilities.Columns["TOTAL_PRICE"].Visible = false;

                    if (dgvFacilities.Columns.Contains("FormattedPrice"))
                        dgvFacilities.Columns["FormattedPrice"].HeaderText = "Total Price";

                    MessageBox.Show($"Loaded {table.Rows.Count} facility bookings", "Facilities Loaded",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading facility bookings: {ex.Message}\n\nPlease make sure the FACILITIES_BOOKING table exists in your database.",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dgvFacilities.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a facility booking to update.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(cbStatus.Text))
            {
                MessageBox.Show("Please select a status.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int bookingId = Convert.ToInt32(dgvFacilities.SelectedRows[0].Cells["FACILITY_BOOKING_ID"].Value);
            string newStatus = cbStatus.Text;

            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "UPDATE FACILITIES_BOOKING SET BOOKING_STATUS = @Status WHERE FACILITY_BOOKING_ID = @BookingId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@BookingId", bookingId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Status updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadFacilitiesData(); // Refresh the data
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    // Revenue Report Form (existing code remains the same)
    public class RevenueReportForm : Form
    {

        private DataGridView dgvRevenue;
        private Button btnClose;
        private Label label1;
        private ComboBox cbReportType;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private Button btnGenerate;
        private Label lblTotalRevenue;

        public RevenueReportForm()
        {
            InitializeComponent();
            LoadRevenueData("Monthly", DateTime.Today.AddMonths(-6), DateTime.Today);
        }

        private void InitializeComponent()
        {
            this.Text = "Revenue Report - Rifat Grand Hotel";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));

            label1 = new Label();
            label1.Text = "REVENUE REPORT";
            label1.Font = new Font("Segoe Print", 18, FontStyle.Bold);
            label1.ForeColor = Color.MistyRose;
            label1.AutoSize = true;
            label1.Location = new Point(320, 20);
            this.Controls.Add(label1);

            // Report Type Selection
            Label lblReportType = new Label();
            lblReportType.Text = "Report Type:";
            lblReportType.ForeColor = Color.White;
            lblReportType.Location = new Point(50, 70);
            this.Controls.Add(lblReportType);

            cbReportType = new ComboBox();
            cbReportType.Items.AddRange(new object[] { "Daily", "Monthly", "Yearly" });
            cbReportType.SelectedItem = "Monthly";
            cbReportType.Location = new Point(140, 70);
            cbReportType.Size = new Size(120, 24);
            cbReportType.SelectedIndexChanged += CbReportType_SelectedIndexChanged;
            this.Controls.Add(cbReportType);

            // Start Date
            Label lblStartDate = new Label();
            lblStartDate.Text = "From:";
            lblStartDate.ForeColor = Color.White;
            lblStartDate.Location = new Point(280, 70);
            this.Controls.Add(lblStartDate);

            dtpStartDate = new DateTimePicker();
            dtpStartDate.Location = new Point(330, 70);
            dtpStartDate.Size = new Size(120, 22);
            dtpStartDate.Value = DateTime.Today.AddMonths(-6);
            this.Controls.Add(dtpStartDate);

            // End Date
            Label lblEndDate = new Label();
            lblEndDate.Text = "To:";
            lblEndDate.ForeColor = Color.White;
            lblEndDate.Location = new Point(460, 70);
            this.Controls.Add(lblEndDate);

            dtpEndDate = new DateTimePicker();
            dtpEndDate.Location = new Point(490, 70);
            dtpEndDate.Size = new Size(120, 22);
            dtpEndDate.Value = DateTime.Today;
            this.Controls.Add(dtpEndDate);

            // Generate Button
            btnGenerate = new Button();
            btnGenerate.Text = "Generate Report";
            btnGenerate.Size = new Size(150, 30);
            btnGenerate.Location = new Point(630, 70);
            btnGenerate.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.Click += BtnGenerate_Click;
            this.Controls.Add(btnGenerate);

            dgvRevenue = new DataGridView();
            dgvRevenue.Size = new Size(840, 350);
            dgvRevenue.Location = new Point(30, 120);
            dgvRevenue.BackgroundColor = Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            dgvRevenue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRevenue.ReadOnly = true;
            this.Controls.Add(dgvRevenue);

            // Total Revenue Label
            lblTotalRevenue = new Label();
            lblTotalRevenue.Text = "Total Revenue: TK 0.00";
            lblTotalRevenue.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTotalRevenue.ForeColor = Color.LightGreen;
            lblTotalRevenue.AutoSize = true;
            lblTotalRevenue.Location = new Point(30, 480);
            this.Controls.Add(lblTotalRevenue);

            btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Size = new Size(100, 40);
            btnClose.Location = new Point(400, 520);
            btnClose.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private void CbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Adjust date range based on report type
            string reportType = cbReportType.SelectedItem.ToString();

            if (reportType == "Yearly")
            {
                dtpStartDate.Value = new DateTime(DateTime.Today.Year - 2, 1, 1);
                dtpEndDate.Value = DateTime.Today;
            }
            else if (reportType == "Monthly")
            {
                dtpStartDate.Value = DateTime.Today.AddMonths(-6);
                dtpEndDate.Value = DateTime.Today;
            }
            else // Daily
            {
                dtpStartDate.Value = DateTime.Today.AddDays(-30);
                dtpEndDate.Value = DateTime.Today;
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            string reportType = cbReportType.SelectedItem.ToString();
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;

            LoadRevenueData(reportType, startDate, endDate);
        }

        private void LoadRevenueData(string reportType, DateTime startDate, DateTime endDate)
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string query = "";
                    string groupBy = "";
                    string dateColumn = "";

                    // Build query based on report type
                    if (reportType == "Daily")
                    {
                        dateColumn = "CONVERT(VARCHAR(10), B.BOOKING_DATE, 120) AS Date";
                        groupBy = "CONVERT(VARCHAR(10), B.BOOKING_DATE, 120)";
                    }
                    else if (reportType == "Monthly")
                    {
                        dateColumn = "CONCAT(DATENAME(MONTH, B.BOOKING_DATE), ' ', YEAR(B.BOOKING_DATE)) AS Month";
                        groupBy = "YEAR(B.BOOKING_DATE), MONTH(B.BOOKING_DATE), DATENAME(MONTH, B.BOOKING_DATE)";
                    }
                    else // Yearly
                    {
                        dateColumn = "YEAR(B.BOOKING_DATE) AS Year";
                        groupBy = "YEAR(B.BOOKING_DATE)";
                    }

                    // Calculate revenue based on room rates and duration
                    query = $@"
        SELECT
            {dateColumn},
            SUM(DATEDIFF(DAY, B.CHECK_IN, B.CHECK_OUT) * R.PPN) AS Revenue,
            COUNT(*) AS Bookings
        FROM BOOKINGS_TABLE B
        JOIN ROOM_TABLE R ON B.ROOM_ID = R.ROOM_ID
        WHERE B.BOOKING_DATE BETWEEN @StartDate AND @EndDate
        GROUP BY {groupBy}
        ORDER BY {groupBy}";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate.AddDays(1).AddSeconds(-1)); // Include entire end date

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    // Calculate total revenue
                    decimal totalRevenue = 0;
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["Revenue"] != DBNull.Value)
                        {
                            totalRevenue += Convert.ToDecimal(row["Revenue"]);
                        }
                    }

                    // Format the revenue column
                    table.Columns.Add("FormattedRevenue", typeof(string));
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["Revenue"] != DBNull.Value)
                        {
                            decimal revenue = Convert.ToDecimal(row["Revenue"]);
                            row["FormattedRevenue"] = "TK " + revenue.ToString("N2");
                        }
                        else
                        {
                            row["FormattedRevenue"] = "TK 0.00";
                        }
                    }

                    // Bind to DataGridView
                    dgvRevenue.DataSource = table;

                    // Hide the raw Revenue column and show the formatted one
                    if (dgvRevenue.Columns.Contains("Revenue"))
                        dgvRevenue.Columns["Revenue"].Visible = false;

                    if (dgvRevenue.Columns.Contains("FormattedRevenue"))
                        dgvRevenue.Columns["FormattedRevenue"].HeaderText = "Revenue";

                    // Update total revenue label
                    lblTotalRevenue.Text = $"Total Revenue: TK {totalRevenue.ToString("N2")}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading revenue data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }


        // ... (keep your existing RevenueReportForm code exactly as it is)
    }

    // Reports Form (existing code remains the same)
    public class ReportsForm : Form
    {
        private DataGridView dgvReports;
        private Button btnClose;
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private Button btnGenerate;
        private Label label1;
        private Label label2;
        private Label label3;

        public ReportsForm()
        {
            InitializeComponent();
            LoadReportData(dtpFromDate.Value, dtpToDate.Value); // Load initial data
        }

        private void InitializeComponent()
        {
            this.Text = "Booking Reports - Hotel Marina";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));

            label1 = new Label();
            label1.Text = "BOOKING REPORTS";
            label1.Font = new Font("Segoe Print", 18, FontStyle.Bold);
            label1.ForeColor = Color.MistyRose;
            label1.AutoSize = true;
            label1.Location = new Point(320, 20);
            this.Controls.Add(label1);

            label2 = new Label();
            label2.Text = "From Date:";
            label2.ForeColor = Color.White;
            label2.Location = new Point(50, 80);
            this.Controls.Add(label2);

            dtpFromDate = new DateTimePicker();
            dtpFromDate.Location = new Point(130, 80);
            dtpFromDate.Size = new Size(200, 22);
            dtpFromDate.Value = DateTime.Today.AddMonths(-1); // Default to last month
            this.Controls.Add(dtpFromDate);

            label3 = new Label();
            label3.Text = "To Date:";
            label3.ForeColor = Color.White;
            label3.Location = new Point(350, 80);
            this.Controls.Add(label3);

            dtpToDate = new DateTimePicker();
            dtpToDate.Location = new Point(420, 80);
            dtpToDate.Size = new Size(200, 22);
            dtpToDate.Value = DateTime.Today; // Default to today
            this.Controls.Add(dtpToDate);

            btnGenerate = new Button();
            btnGenerate.Text = "Generate Report";
            btnGenerate.Size = new Size(150, 30);
            btnGenerate.Location = new Point(650, 80);
            btnGenerate.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.Click += BtnGenerate_Click;
            this.Controls.Add(btnGenerate);

            dgvReports = new DataGridView();
            dgvReports.Size = new Size(820, 400);
            dgvReports.Location = new Point(30, 130);
            dgvReports.BackgroundColor = Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReports.ReadOnly = true;
            this.Controls.Add(dgvReports);

            btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Size = new Size(100, 40);
            btnClose.Location = new Point(400, 550);
            btnClose.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1); // Include entire end date

            if (fromDate > toDate)
            {
                MessageBox.Show("From date cannot be after to date.");
                return;
            }

            LoadReportData(fromDate, toDate);
        }

        private void LoadReportData(DateTime fromDate, DateTime toDate)
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // Calculate total amount based on room rates and duration
                    string query = @"
                SELECT 
                    B.BOOKING_ID,
                    U.NAME AS CustomerName,
                    B.ROOM_ID AS RoomNumber,
                    B.CHECK_IN,
                    B.CHECK_OUT,
                    B.BOOKING_DATE,
                    B.BOOKING_STATUS,
                    DATEDIFF(DAY, B.CHECK_IN, B.CHECK_OUT) * R.PPN AS TotalAmount,
                    B.EMAIL AS CustomerEmail
                FROM BOOKINGS_TABLE B
                JOIN USER_TABLE U ON B.EMAIL = U.EMAIL
                JOIN ROOM_TABLE R ON B.ROOM_ID = R.ROOM_ID
                WHERE B.BOOKING_DATE BETWEEN @FromDate AND @ToDate
                ORDER BY B.BOOKING_DATE DESC";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate.AddDays(1).AddSeconds(-1)); // Include entire end date

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    // Add a new column for formatted amount instead of modifying the original
                    table.Columns.Add("FormattedAmount", typeof(string));

                    // Format currency as Bangladeshi Taka in the new column
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["TotalAmount"] != DBNull.Value && decimal.TryParse(row["TotalAmount"].ToString(), out decimal amount))
                        {
                            row["FormattedAmount"] = "TK " + amount.ToString("N2");
                        }
                        else
                        {
                            row["FormattedAmount"] = "TK 0.00";
                        }
                    }

                    dgvReports.DataSource = table;

                    // Hide the original numeric column and show the formatted one
                    if (dgvReports.Columns.Contains("TotalAmount"))
                        dgvReports.Columns["TotalAmount"].Visible = false;

                    if (dgvReports.Columns.Contains("FormattedAmount"))
                        dgvReports.Columns["FormattedAmount"].HeaderText = "Total Amount";

                    // Show summary
                    decimal totalRevenue = 0;
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["TotalAmount"] != DBNull.Value && decimal.TryParse(row["TotalAmount"].ToString(), out decimal amount))
                        {
                            totalRevenue += amount;
                        }
                    }

                    MessageBox.Show($"Report Generated Successfully!\n\n" +
                                  $"Period: {fromDate.ToShortDateString()} to {toDate.ToShortDateString()}\n" +
                                  $"Total Bookings: {table.Rows.Count}\n" +
                                  $"Total Revenue: TK {totalRevenue.ToString("N2")}",
                                  "Report Summary",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading report data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // ... (keep your existing ReportsForm code exactly as it is)
        }

}