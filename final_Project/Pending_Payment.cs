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
    public partial class Pending_Payment : Form
    {
        string email, checkIn, checkOut,roomId,Rid;
        public Pending_Payment()
        {
            InitializeComponent();
        }
        public Pending_Payment(string email,string rID)
        {
            roomId = rID;
            this.email = email;
            InitializeComponent();
        }

        private void Pending_Payment_Load(object sender, EventArgs e)
        {
            load_pending_bokiings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rooms rooms = new Rooms(email);
            rooms.Show();
            this.Visible = false;
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
                Font = new Font("Segoe Print", 11, FontStyle.Bold),
                AutoSize = true,
                Left = left,
                Top = 5
            };
        }
        private void CencelBtn_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string BId = clickedButton.Tag.ToString();
            Payment payment = new Payment(Rid, email,BId, checkIn, checkOut);
            payment.Show();
            this.Visible = false;

        }
        //load all unpaid booking from database
        void load_pending_bokiings()
        {
            flowLayoutPanel1.Controls.Clear();
            SqlConnection conn = GetConnection();

            SqlCommand cmd = new SqlCommand("SELECT BOOKING_ID,ROOM_ID,CHECK_IN,CHECK_OUT,BOOKING_DATE,BOOKING_STATUS FROM BOOKINGS_TABLE WHERE EMAIL LIKE '" + email + "' AND BOOKING_STATUS LIKE 'PENDING'", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string id = reader["BOOKING_ID"].ToString();
                Rid= reader["ROOM_ID"].ToString();
                checkIn = Convert.ToDateTime(reader["CHECK_IN"]).ToString("yyyy-MM-dd");
                checkOut = Convert.ToDateTime(reader["CHECK_OUT"]).ToString("yyyy-MM-dd");
                string booked = Convert.ToDateTime(reader["BOOKING_DATE"]).ToString("yyyy-MM-dd");
                string status = reader["BOOKING_STATUS"].ToString();

                // Create panel UI (same as before, no need to change here)
                Panel roomPanel = new Panel
                {
                    Width = 1100,
                    Height = 50,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10)
                };
                Button cencelBtn = new Button
                {
                    Text = "Pay Bill",
                    Font = new Font("Segoe Print", 10, FontStyle.Bold),
                    BackColor = Color.FromArgb(43, 92, 122),
                    ForeColor = Color.White,
                    Width = 80,
                    Height = 37,
                    Top = 5,
                    Left = 1015,
                    Tag = id
                };
                cencelBtn.Click += CencelBtn_Click;

                roomPanel.Controls.Add(CreateDataLabel($"Booked in - {booked}", 20));
                roomPanel.Controls.Add(CreateDataLabel($"From - {checkIn}", 242));
                roomPanel.Controls.Add(CreateDataLabel($"To - {checkOut}", 455));
                roomPanel.Controls.Add(CreateDataLabel($"Status - {status}", 655));
                roomPanel.Controls.Add(CreateDataLabel($"Room No - {Rid}", 850));
                roomPanel.Controls.Add(cencelBtn);

                flowLayoutPanel1.Controls.Add(roomPanel);
            }
        }
    }
}
