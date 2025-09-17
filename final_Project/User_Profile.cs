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
    public partial class User_Profile : Form
    {
        string email, type = "", roomId, rID;

        public User_Profile()
        {
            InitializeComponent();
        }

        public User_Profile(string email, string type)
        {
            this.email = email;
            this.type = type;
            InitializeComponent();
        }

        public User_Profile(string email, string roomId, string type)
        {
            this.roomId = roomId;
            this.email = email;
            this.type = type;
            InitializeComponent();
        }

        // For methods that want to manage connection themselves (like CancelBtn_Click)
        SqlConnection GetClosedConnection()
        {
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            return new SqlConnection(connectionString);
        }

        // For methods that expect an already open connection (keep this for backward compatibility)
        SqlConnection GetConnection()
        {
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);

            try
            {
                con.Open();
                return con;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection failed: " + ex.Message);
                throw; // Re-throw the exception to be handled by the calling method
            }
        }

        private Label CreateDataLabel(string text, int left)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe Print", 11, FontStyle.Bold),
                AutoSize = true,
                Left = left,
                Top = 5
            };
        }

        // Cancel your booking [can't cancel bookings that's already been check-out]
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null || clickedButton.Tag == null)
            {
                MessageBox.Show("Invalid booking selection.");
                return;
            }

            string selectedRoomId = clickedButton.Tag.ToString();

            try
            {
                using (SqlConnection conn = GetClosedConnection()) // CHANGED THIS LINE
                {
                    conn.Open(); // Now this is correct - opening a closed connection

                    // Use UPDATE instead of DELETE
                    string query = "UPDATE BOOKINGS_TABLE SET BOOKING_STATUS = 'CANCELLED' WHERE BOOKING_ID = @BookingId AND BOOKING_STATUS != 'CHECK-OUT'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@BookingId", selectedRoomId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Booking cancelled successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Cannot cancel booking. It may have already been checked out or doesn't exist.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cancelling booking: " + ex.Message);
            }

            load_current_booking();
        }

        // Load the booking information from database and show
        void load_current_booking()
        {
            try
            {
                flowLayoutPanel1.Controls.Clear();

                using (SqlConnection conn = GetConnection())
                {
                    string query = "SELECT BOOKING_ID, ROOM_ID, CHECK_IN, CHECK_OUT, BOOKING_DATE, BOOKING_STATUS " +
                                  "FROM BOOKINGS_TABLE WHERE EMAIL = @Email";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Label noBookingsLabel = new Label
                                {
                                    Text = "No bookings found.",
                                    Font = new Font("Segoe Print", 11, FontStyle.Bold),
                                    AutoSize = true,
                                    ForeColor = Color.Red
                                };
                                flowLayoutPanel1.Controls.Add(noBookingsLabel);
                                return;
                            }

                            while (reader.Read())
                            {
                                string id = reader["BOOKING_ID"].ToString();
                                rID = reader["ROOM_ID"].ToString();
                                string checkIn = Convert.ToDateTime(reader["CHECK_IN"]).ToString("yyyy-MM-dd");
                                string checkOut = Convert.ToDateTime(reader["CHECK_OUT"]).ToString("yyyy-MM-dd");
                                string booked = Convert.ToDateTime(reader["BOOKING_DATE"]).ToString("yyyy-MM-dd");
                                string status = reader["BOOKING_STATUS"].ToString();

                                // Create panel UI
                                Panel roomPanel = new Panel
                                {
                                    Width = 1130,
                                    Height = 40,
                                    ForeColor = Color.Black,
                                    BorderStyle = BorderStyle.None,
                                    Margin = new Padding(2)
                                };

                                Button cencelBtn = new Button
                                {
                                    Text = "Cancel",
                                    Font = new Font("Segoe Print", 10, FontStyle.Bold),
                                    BackColor = Color.FromArgb(85, 88, 121),
                                    ForeColor = Color.White,
                                    FlatStyle = FlatStyle.Flat,
                                    Width = 80,
                                    Height = 30,
                                    Top = 5,
                                    Left = 1040,
                                    Tag = id
                                };
                                cencelBtn.Click += CancelBtn_Click;
                                cencelBtn.FlatAppearance.BorderSize = 0;

                                roomPanel.Controls.Add(CreateDataLabel($"Booked in - {booked}", 20));
                                roomPanel.Controls.Add(CreateDataLabel($"From - {checkIn}", 255));
                                roomPanel.Controls.Add(CreateDataLabel($"To - {checkOut}", 465));
                                roomPanel.Controls.Add(CreateDataLabel($"Status - {status}", 665));
                                roomPanel.Controls.Add(CreateDataLabel($"Room No - {rID}", 860));

                                // Only show cancel button for active bookings
                                if (status != "CHECK-OUT" && status != "CANCELLED")
                                {
                                    roomPanel.Controls.Add(cencelBtn);
                                }

                                flowLayoutPanel1.BackColor = Color.FromArgb(234, 239, 239);
                                flowLayoutPanel1.Controls.Add(roomPanel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bookings: " + ex.Message);

                // Add error message to UI
                Label errorLabel = new Label
                {
                    Text = "Error loading bookings. Please try again.",
                    Font = new Font("Segoe Print", 11, FontStyle.Bold),
                    AutoSize = true,
                    ForeColor = Color.Red
                };
                flowLayoutPanel1.Controls.Add(errorLabel);
            }
        }

        void getUser()
        {
            SqlConnection conn = GetConnection();
            SqlCommand cmd = new SqlCommand($"SELECT NAME,PHONE_NUMBER,EMAIL FROM USER_TABLE WHERE EMAIL LIKE '" + email + "'", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                txtname.Text = reader["NAME"].ToString();
                txtnumber.Text = reader["PHONE_NUMBER"].ToString();
                txtemail.Text = reader["EMAIL"].ToString();
            }
        }

        // Load completed bookings for review
        void LoadCompletedBookingsForReview()
        {
            try
            {
                SqlConnection conn = GetConnection();
                SqlCommand cmd = new SqlCommand("SELECT BOOKING_ID, ROOM_ID, CHECK_IN, CHECK_OUT FROM BOOKINGS_TABLE WHERE EMAIL LIKE '" + email + "' AND BOOKING_STATUS = 'CHECK-OUT'", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                cmbBookings.Items.Clear();

                while (reader.Read())
                {
                    string bookingId = reader["BOOKING_ID"].ToString();
                    string roomId = reader["ROOM_ID"].ToString();
                    string checkIn = Convert.ToDateTime(reader["CHECK_IN"]).ToString("yyyy-MM-dd");
                    string checkOut = Convert.ToDateTime(reader["CHECK_OUT"]).ToString("yyyy-MM-dd");

                    string displayText = $"Booking #{bookingId} - Room {roomId} ({checkIn} to {checkOut})";
                    cmbBookings.Items.Add(new KeyValuePair<string, string>(bookingId, displayText));
                }

                cmbBookings.DisplayMember = "Value";
                cmbBookings.ValueMember = "Key";

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading completed bookings: " + ex.Message);
            }
        }

        // Load hotel facilities
        void LoadHotelFacilities()
        {
            try
            {
                panelFacilities.Controls.Clear();

                // Define hotel facilities with prices in Bangladeshi Taka (TK)
                var facilities = new List<Facility>
                {
                    new Facility { Name = "Swimming Pool", Description = "Enjoy our heated swimming pool", Price = 1000.00m },
                    new Facility { Name = "Breakfast Buffet", Description = "Start your day with our delicious breakfast buffet", Price = 1200.00m },
                    new Facility { Name = "Spa Services", Description = "Relax with our premium spa treatments", Price = 2500.00m },
                    new Facility { Name = "Fitness Center", Description = "Stay fit with our modern gym equipment", Price = 1000.00m },
                    new Facility { Name = "Business Center", Description = "Access computers, printers, and meeting rooms", Price = 800.00m }
                };

                int top = 20;
                foreach (var facility in facilities)
                {
                    Panel panel = new Panel();
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.BackColor = Color.White;
                    panel.Size = new Size(1100, 80);
                    panel.Location = new Point(20, top);

                    Label lblName = new Label();
                    lblName.Text = facility.Name;
                    lblName.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                    lblName.Location = new Point(10, 10);
                    lblName.AutoSize = true;
                    panel.Controls.Add(lblName);

                    Label lblDesc = new Label();
                    lblDesc.Text = facility.Description;
                    lblDesc.Font = new Font("Segoe UI", 10);
                    lblDesc.Location = new Point(10, 40);
                    lblDesc.AutoSize = true;
                    panel.Controls.Add(lblDesc);

                    Label lblPrice = new Label();
                    lblPrice.Text = $"TK{facility.Price:F2} per person";
                    lblPrice.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    lblPrice.Location = new Point(800, 10);
                    lblPrice.AutoSize = true;
                    panel.Controls.Add(lblPrice);

                    Button btnBook = new Button();
                    btnBook.Text = "Book Now";
                    btnBook.Location = new Point(950, 25);
                    btnBook.Size = new Size(120, 30);
                    btnBook.BackColor = Color.FromArgb(152, 161, 188);
                    btnBook.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    btnBook.Tag = facility.Name;
                    btnBook.Click += BtnBookFacility_Click;
                    panel.Controls.Add(btnBook);

                    panelFacilities.Controls.Add(panel);

                    top += 90;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading facilities: " + ex.Message);
            }
        }

        // Submit review button handler
        private void BtnSubmitReview_Click(object sender, EventArgs e)
        {
            if (cmbBookings.SelectedItem == null)
            {
                MessageBox.Show("Please select a booking to review.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtReviewComments.Text))
            {
                MessageBox.Show("Please enter your comments.");
                return;
            }

            try
            {
                string bookingId = ((KeyValuePair<string, string>)cmbBookings.SelectedItem).Key;
                int rating = (int)numRating.Value;
                string comments = txtReviewComments.Text;

                SqlConnection conn = GetConnection();
                string query = @"INSERT INTO REVIEWS_TABLE (BOOKING_ID, EMAIL, RATING, COMMENTS, REVIEW_DATE) 
                                VALUES (@bookingId, @email, @rating, @comments, GETDATE())";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@bookingId", bookingId);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@comments", comments);

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Thank you for your review!");
                    txtReviewComments.Clear();
                    numRating.Value = 5;
                    cmbBookings.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error submitting review: " + ex.Message);
            }
        }

        // Book facility button handler
        private void BtnBookFacility_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            string facilityName = btn.Tag.ToString();

            try
            {
                // Show facility booking form
                Facility_Booking facilityBooking = new Facility_Booking(email, facilityName);
                facilityBooking.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Facility booking feature is not available yet. Please contact support.\nError: " + ex.Message);
            }
        }

        // Back button [as you can go profile from any form. So you have go back those form that you came from.]
        private void button1_Click(object sender, EventArgs e)
        {
            if (type == "room")
            {
                Rooms rooms = new Rooms(email);
                rooms.Show();
                this.Visible = false;
            }
            else if (type == "room_details")
            {
                Room_Details room_Details = new Room_Details(roomId, email);
                room_Details.Show();
                this.Visible = false;
            }
            else if (type == "confirm_bookings")
            {
                Confirm_Bookings confirm_Bookings = new Confirm_Bookings(roomId, email);
                confirm_Bookings.Show();
                this.Visible = false;
            }
            else if (type == "home")
            {
                Home form1 = new Home(email);
                form1.Show();
                this.Visible = false;
            }
        }

        // Edit Info Button [passing the current state also for coming back on the same form]
        private void button2_Click(object sender, EventArgs e)
        {
            if (type == "room")
            {
                Edit_Info edit_Info = new Edit_Info(email, type);
                edit_Info.Show();
                this.Visible = false;
            }
            else if (type == "room_details")
            {
                Edit_Info edit_Info = new Edit_Info(email, roomId, type);
                edit_Info.Show();
                this.Visible = false;
            }
            else if (type == "confirm_bookings")
            {
                Edit_Info edit_Info = new Edit_Info(email, roomId, type);
                edit_Info.Show();
                this.Visible = false;
            }
            else if (type == "home")
            {
                Edit_Info edit_Info = new Edit_Info(email, type);
                edit_Info.Show();
                this.Visible = false;
            }
        }

        // Unpaid Rooms button
        private void button3_Click(object sender, EventArgs e)
        {
            Pending_Payment pending_Payment = new Pending_Payment(email, roomId);
            pending_Payment.Show();
            this.Visible = false;
        }

        // Log out button
        private void btnlogout_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Visible = false;
        }

        private void User_Profile_Load(object sender, EventArgs e)
        {
            load_current_booking();
            getUser();
            LoadCompletedBookingsForReview();
            LoadHotelFacilities();
        }
    }

    // Helper class for facilities
    public class Facility
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}