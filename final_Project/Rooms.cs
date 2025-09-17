using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace final_Project
{
    public partial class Rooms : Form
    {
        public string mail = "rifat9461@gmail.com";
        private string currentSort = "ORDER BY ROOM_ID";
        private string currentFilter = "";
        private string currentSearch = "";

        public Rooms()
        {
            InitializeComponent();
            LoadRooms();
        }

        public Rooms(string email)
        {
            InitializeComponent();
            mail = email;
            LoadRooms();
        }

        SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }

        private bool IsRoomAvailable(string roomId, DateTime checkIn, DateTime checkOut)
        {
            using (SqlConnection conn = GetConnection())
            {
                string query = @"
                SELECT COUNT(*) 
                FROM BOOKINGS_TABLE 
                WHERE ROOM_ID = @RoomId 
                AND BOOKING_STATUS IN ('PAID', 'BOOKED', 'CHECK-IN', 'PENDING', 'Confirmed', 'Checked-In')
                AND (
                    (CHECK_IN <= @CheckOut AND CHECK_OUT >= @CheckIn)
                )";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@RoomId", roomId);
                cmd.Parameters.AddWithValue("@CheckIn", checkIn.Date);
                cmd.Parameters.AddWithValue("@CheckOut", checkOut.Date);

                int overlappingBookings = (int)cmd.ExecuteScalar();
                return overlappingBookings == 0;
            }
        }

        private void BookBtn_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string selectedRoomId = clickedButton.Tag.ToString();
            Room_Details bookingForm = new Room_Details(selectedRoomId, mail);
            bookingForm.Show();
            this.Visible = false;
        }

        private void Rooms_Load(object sender, EventArgs e)
        {
            GetUser();
        }

        private void LoadRooms()
        {
            flowLayoutPanel1.Controls.Clear();
            using (SqlConnection conn = GetConnection())
            {
                string baseQuery = "SELECT ROOM_ID, TYPE, PPN, DESCRIPTION, PICTURE FROM ROOM_TABLE";
                string whereClause = "";
                List<SqlParameter> parameters = new List<SqlParameter>();

                // Handle search filter
                if (!string.IsNullOrEmpty(currentSearch))
                {
                    whereClause = "WHERE TYPE LIKE @SearchText";
                    parameters.Add(new SqlParameter("@SearchText", $"%{currentSearch}%"));
                }

                string fullQuery = $"{baseQuery} {whereClause} {currentSort}";
                SqlCommand cmd = new SqlCommand(fullQuery, conn);

                // Add parameters if any
                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                SqlDataReader reader = cmd.ExecuteReader();

                DateTime checkIn = dateTimePickerCheckIn.Value.Date;
                DateTime checkOut = dateTimePickerCheckOut.Value.Date;

                // Create a list to store all rooms first
                List<RoomData> rooms = new List<RoomData>();

                while (reader.Read())
                {
                    string roomId = reader["ROOM_ID"].ToString();
                    bool isAvailable = IsRoomAvailable(roomId, checkIn, checkOut);

                    rooms.Add(new RoomData
                    {
                        RoomId = roomId,
                        Type = reader["TYPE"].ToString(),
                        Price = reader["PPN"].ToString(),
                        Description = reader["DESCRIPTION"].ToString(),
                        ImagePath = reader["PICTURE"].ToString(),
                        IsAvailable = isAvailable
                    });
                }
                reader.Close();

                // Now filter based on currentFilter
                IEnumerable<RoomData> filteredRooms = rooms;

                if (currentFilter == "Available")
                {
                    filteredRooms = rooms.Where(r => r.IsAvailable);
                }
                else if (currentFilter == "Booked")
                {
                    filteredRooms = rooms.Where(r => !r.IsAvailable);
                }

                // Create panels for filtered rooms
                foreach (var room in filteredRooms)
                {
                    CreateRoomPanel(room);
                }
            }
        }

        private void CreateRoomPanel(RoomData room)
        {
            string status = room.IsAvailable ? "Available" : "Booked";
            Color statusColor = room.IsAvailable ? Color.Green : Color.Red;
            Color panelColor = room.IsAvailable ? Color.FromArgb(230, 255, 230) : Color.FromArgb(255, 230, 230);

            int panelWidth = flowLayoutPanel1.ClientSize.Width - 30;
            Panel roomPanel = new Panel
            {
                Height = 220,
                Width = panelWidth,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = panelColor
            };

            PictureBox pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 230,
                Height = 170,
                Top = 10,
                Left = 10
            };

            if (File.Exists(room.ImagePath))
            {
                pictureBox.Image = Image.FromFile(room.ImagePath);
            }
            else
            {
                pictureBox.Image = SystemIcons.Warning.ToBitmap();
            }

            Panel rightPanel = new Panel
            {
                Left = 247,
                Top = 10,
                Width = 800,
                Height = 180
            };

            Label typeLabel = new Label
            {
                Text = room.Type,
                Font = new Font("Segoe Print", 12, FontStyle.Bold),
                AutoSize = false,
                Top = 10,
                Left = 0,
                Width = 230,
                Height = 35
            };

            Label descLabel = new Label
            {
                Text = room.Description,
                Font = new Font("Segoe Print", 10, FontStyle.Regular),
                AutoSize = false,
                Top = 50,
                Left = 0,
                Width = 800,
                Height = 50
            };

            Label priceLabel = new Label
            {
                Text = $"Price: {room.Price} BDT/night",
                Font = new Font("Segoe Print", 11, FontStyle.Bold),
                AutoSize = true,
                Top = 100,
                Left = 0
            };

            Label statusLabel = new Label
            {
                Text = $"Status: {status}",
                Font = new Font("Segoe Print", 11, FontStyle.Bold),
                ForeColor = statusColor,
                AutoSize = true,
                Top = 125,
                Left = 0
            };

            Button bookBtn = new Button
            {
                Text = "Book Now",
                Font = new Font("Segoe Print", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(43, 92, 122),
                ForeColor = Color.White,
                Width = 120,
                Height = 35,
                Top = 150,
                Left = 0,
                Tag = room.RoomId,
                Enabled = room.IsAvailable
            };
            bookBtn.Click += BookBtn_Click;

            rightPanel.Controls.Add(typeLabel);
            rightPanel.Controls.Add(descLabel);
            rightPanel.Controls.Add(priceLabel);
            rightPanel.Controls.Add(statusLabel);
            rightPanel.Controls.Add(bookBtn);

            roomPanel.Controls.Add(pictureBox);
            roomPanel.Controls.Add(rightPanel);

            flowLayoutPanel1.Controls.Add(roomPanel);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSort = comboBox1.SelectedItem?.ToString();
            currentSearch = "";

            if (selectedSort == "Alphabetically")
            {
                currentSort = "ORDER BY TYPE";
                currentFilter = "";
            }
            else if (selectedSort == "Low to High")
            {
                currentSort = "ORDER BY CAST(PPN AS INT) ASC";
                currentFilter = "";
            }
            else if (selectedSort == "High to Low")
            {
                currentSort = "ORDER BY CAST(PPN AS INT) DESC";
                currentFilter = "";
            }
            else if (selectedSort == "Available")
            {
                currentSort = "ORDER BY ROOM_ID";
                currentFilter = "Available";
            }
            else if (selectedSort == "Booked")
            {
                currentSort = "ORDER BY ROOM_ID";
                currentFilter = "Booked";
            }
            else
            {
                currentSort = "ORDER BY ROOM_ID";
                currentFilter = "";
            }

            LoadRooms();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            currentSearch = txtsearch.Text.Trim();
            currentFilter = "";
            currentSort = "ORDER BY TYPE";
            LoadRooms();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Home form1 = new Home(mail);
            form1.Show();
            this.Visible = false;
        }

        private void GetUser()
        {
            using (SqlConnection conn = GetConnection())
            {
                SqlCommand cmd = new SqlCommand($"SELECT NAME FROM USER_TABLE WHERE EMAIL = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", mail);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtuser.Text = reader["NAME"].ToString();
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string type = "room";
            User_Profile user_Profile = new User_Profile(mail, type);
            user_Profile.Show();
            this.Visible = false;
        }

        private void BtnCheckAvailability_Click(object sender, EventArgs e)
        {
            DateTime checkIn = dateTimePickerCheckIn.Value.Date;
            DateTime checkOut = dateTimePickerCheckOut.Value.Date;

            if (checkIn >= checkOut)
            {
                MessageBox.Show("Check-out date must be after check-in date.");
                return;
            }

            MessageBox.Show($"Checking availability from {checkIn.ToShortDateString()} to {checkOut.ToShortDateString()}");
            LoadRooms();
        }

        // Helper class to store room data
        private class RoomData
        {
            public string RoomId { get; set; }
            public string Type { get; set; }
            public string Price { get; set; }
            public string Description { get; set; }
            public string ImagePath { get; set; }
            public bool IsAvailable { get; set; }
        }

        // Debug method to check which rooms are booked
        private void DebugBookedRooms()
        {
            DateTime checkIn = dateTimePickerCheckIn.Value.Date;
            DateTime checkOut = dateTimePickerCheckOut.Value.Date;

            using (SqlConnection conn = GetConnection())
            {
                string query = "SELECT ROOM_ID FROM ROOM_TABLE";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                List<string> bookedRooms = new List<string>();

                while (reader.Read())
                {
                    string roomId = reader["ROOM_ID"].ToString();
                    if (!IsRoomAvailable(roomId, checkIn, checkOut))
                    {
                        bookedRooms.Add(roomId);
                    }
                }
                reader.Close();

                MessageBox.Show($"Booked rooms for {checkIn.ToShortDateString()} to {checkOut.ToShortDateString()}: {string.Join(", ", bookedRooms)}");
            }
        }
    }
}