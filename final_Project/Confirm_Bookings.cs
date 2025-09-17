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
    public partial class Confirm_Bookings : Form
    {
        string roomId, email;
        public Confirm_Bookings()
        {
            InitializeComponent();
        }
        public Confirm_Bookings(string email)
        {
            this.email = email;
            InitializeComponent();
        }
        public Confirm_Bookings(string roomId, string email)
        {
            this.roomId = roomId;
            this.email = email;
            InitializeComponent();
        }
        SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }
        private Label CreateDataLabel(string text, int left)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe Print", 10, FontStyle.Bold),
                AutoSize = true,
                Left = left,
                Top = 5
            };
        }
        //load all booking for a particular room
        void load_current_booking()
        {
            flowLayoutPanel1.Controls.Clear();
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT CHECK_IN,CHECK_OUT FROM BOOKINGS_TABLE WHERE ROOM_ID LIKE @roomId AND BOOKING_STATUS != 'CHECK-OUT'", conn))
                {
                    cmd.Parameters.AddWithValue("@roomId", roomId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string checkIn = Convert.ToDateTime(reader["CHECK_IN"]).ToString("yyyy-MM-dd");
                            string checkOut = Convert.ToDateTime(reader["CHECK_OUT"]).ToString("yyyy-MM-dd");

                            // Create panel UI (same as before, no need to change here)
                            Panel roomPanel = new Panel
                            {
                                Width = 600,
                                Height = 40,
                                BorderStyle = BorderStyle.FixedSingle,
                                Margin = new Padding(10)
                            };

                            roomPanel.Controls.Add(CreateDataLabel($"From -- {checkIn}", 100));
                            roomPanel.Controls.Add(CreateDataLabel($"To -- {checkOut}", 370));

                            flowLayoutPanel1.Controls.Add(roomPanel);
                        }
                    }
                }
            }
        }
        //Back button
        private void button1_Click(object sender, EventArgs e)
        {
            Room_Details room_Details = new Room_Details(roomId, email);
            room_Details.Show();
            this.Visible = false;
        }
        //interference on booking if the selected date is not free 
        private void button2_Click(object sender, EventArgs e)
        {
            DateTime newCheckIn = checkInDate.Value.Date;
            DateTime newCheckOut = checkOutDate.Value.Date;

            if (newCheckIn >= newCheckOut)
            {
                MessageBox.Show("Check-out date must be after check-in date.");
                return;
            }

            using (SqlConnection con = GetConnection())
            {
                string checkQuery = @"
                    SELECT COUNT(*) FROM BOOKINGS_TABLE
                    WHERE 
                        ROOM_ID = @roomId AND 
                        BOOKING_STATUS != 'CHECK-OUT' AND
                        @newCheckIn < CHECK_OUT AND 
                        @newCheckOut > CHECK_IN";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@roomId", roomId);
                    checkCmd.Parameters.AddWithValue("@newCheckIn", newCheckIn);
                    checkCmd.Parameters.AddWithValue("@newCheckOut", newCheckOut);

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("This room is already booked for the selected dates.");
                        return;
                    }

                    // Proceed with booking
                    string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string IdQuery = "SELECT ISNULL(MAX(BOOKING_ID), 0) + 1 FROM BOOKINGS_TABLE";

                    string Id;
                    using (SqlCommand cmd1 = new SqlCommand(IdQuery, con))
                    {
                        Id = cmd1.ExecuteScalar().ToString();
                    }

                    string insertQuery = @"
                        INSERT INTO BOOKINGS_TABLE (BOOKING_ID, EMAIL, ROOM_ID, CHECK_IN, CHECK_OUT, BOOKING_DATE, BOOKING_STATUS)
                        VALUES (@Id, @Email, @RoomId, @CheckIn, @CheckOut, @Today, 'PENDING')";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                    {
                        insertCmd.Parameters.AddWithValue("@Id", Id);
                        insertCmd.Parameters.AddWithValue("@Email", email);
                        insertCmd.Parameters.AddWithValue("@RoomId", roomId);
                        insertCmd.Parameters.AddWithValue("@CheckIn", newCheckIn.ToString("yyyy-MM-dd"));
                        insertCmd.Parameters.AddWithValue("@CheckOut", newCheckOut.ToString("yyyy-MM-dd"));
                        insertCmd.Parameters.AddWithValue("@Today", today);

                        insertCmd.ExecuteNonQuery();
                    }

                    Payment payment = new Payment(roomId, email, Id, newCheckIn.ToString("yyyy-MM-dd"), newCheckOut.ToString("yyyy-MM-dd"));
                    payment.Show();
                    this.Visible = false;
                }
            }
        }

        private void Confirm_Bookings_Load(object sender, EventArgs e)
        {
            load_current_booking();
            getUser();
        }

        private void txtuser_Click(object sender, EventArgs e)
        {
            string type = "confirm_bookings";
            User_Profile user_Profile = new User_Profile(email, roomId, type);
            user_Profile.Show();
            this.Visible = false;
        }

        void getUser()
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT NAME FROM USER_TABLE WHERE EMAIL LIKE @email", conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            txtuser.Text = reader["NAME"].ToString();
                        }
                    }
                }
            }
        }
    }
}