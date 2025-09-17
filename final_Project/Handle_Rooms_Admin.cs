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
    public partial class Handle_Rooms_Admin : Form
    {
        string mId;
        public Handle_Rooms_Admin()
        {
            InitializeComponent();
        }

        private void Handle_Maintainence_Load(object sender, EventArgs e)
        {
            load_maintainence();
            load_rooms();
        }
        SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }
        private Label CreateHeaderLabel(string text, int left, int fontSize)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                AutoSize = true,
                Left = left,
                Top = 1
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
                Top = 5
            };
        }
        //Work In Progess button
        private void inProgessBtn1_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string selectedId = clickedButton.Tag.ToString();
            SqlConnection con = GetConnection();
            string q = "UPDATE MaintenanceRequests SET STATUS='WORK IN PROGESS' WHERE M_ID LIKE  '" + selectedId + "'";
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            load_maintainence();


        }
        //Complete button
        private void completeBtn2Btn1_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string selectedId = clickedButton.Tag.ToString();
            SqlConnection con = GetConnection();
            string q = "UPDATE MaintenanceRequests SET STATUS='COMPLETE' WHERE M_ID LIKE  '" + selectedId + "'";
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            load_maintainence();
        }
        //update button
        void UpdateRoomPrice(string roomId, string newPrice)
        {
            using (SqlConnection con = GetConnection())
            {
                string q = "UPDATE ROOM_TABLE SET PPN = @ppn WHERE ROOM_ID = @roomId";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@ppn", newPrice);
                cmd.Parameters.AddWithValue("@roomId", roomId);
                cmd.ExecuteNonQuery();
            }
        }

        void load_maintainence()
        {
            SqlConnection con = GetConnection();
            string q = "SELECT M_ID,ROOM_ID,PROBLEM,STATUS,SUBMIT_DATE " +
                           "FROM MaintenanceRequests WHERE STATUS != 'COMPLETE'";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader rdr = cmd.ExecuteReader();

            flowLayoutPanel1.Controls.Clear();

            Panel titlePanel = new Panel
            {
                Width = 540,
                Height = 50,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10)
            };
            titlePanel.Controls.Add(CreateHeaderLabel("Maintainence Issue", 110, 18));
            flowLayoutPanel1.Controls.Add(titlePanel);

            Panel headerPanel = new Panel
            {
                Width = 540,
                Height = 40,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10)
            };

            int fontSize = 12;
            headerPanel.Controls.Add(CreateHeaderLabel("Room No", 0, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Problem", 110, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Submit Date", 240, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Status", 385, fontSize));
            flowLayoutPanel1.Controls.Add(headerPanel);

            while (rdr.Read())
            {
                mId = rdr["M_ID"].ToString();
                string room = rdr["ROOM_ID"].ToString();
                string problem = rdr["PROBLEM"].ToString();
                string status = rdr["STATUS"].ToString();
                string date = rdr["SUBMIT_DATE"].ToString();



                Panel taskPanel = new Panel
                {
                    Width = 540,
                    Height = 80,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(5)
                };

                Button inProgressBtn1 = new Button
                {
                    Text = "Work In Progress",
                    Font = new Font("Segoe Print", 11, FontStyle.Bold),
                    BackColor = Color.FromArgb(43, 92, 122),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Top = 40,
                    Left = 100,
                    Tag = mId
                };
                inProgressBtn1.Click += inProgessBtn1_Click;

                Button completeBtn2 = new Button
                {
                    Text = "Complete",
                    Font = new Font("Segoe Print", 11, FontStyle.Bold),
                    BackColor = Color.FromArgb(43, 92, 122),
                    ForeColor = Color.White,
                    Width = 120,
                    Height = 35,
                    Top = 40,
                    Left = 280,
                    Tag = mId
                };
                completeBtn2.Click += completeBtn2Btn1_Click;

                taskPanel.Controls.Add(CreateDataLabel(room, 10, fontSize));
                taskPanel.Controls.Add(CreateDataLabel(problem, 95, fontSize));
                taskPanel.Controls.Add(CreateDataLabel(date, 245, fontSize));
                taskPanel.Controls.Add(CreateDataLabel(status, 360, fontSize));
                taskPanel.Controls.Add(inProgressBtn1);
                taskPanel.Controls.Add(completeBtn2);


                flowLayoutPanel1.Controls.Add(taskPanel);
            }
        }

        void load_rooms()
        {
            SqlConnection con = GetConnection();
            string q = "SELECT ROOM_ID,TYPE,PPN " +
                           "FROM ROOM_TABLE ";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader rdr = cmd.ExecuteReader();

            flowLayoutPanel2.Controls.Clear();

            Panel titlePanel = new Panel
            {
                Width = 540,
                Height = 50,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10)
            };
            titlePanel.Controls.Add(CreateHeaderLabel("ROOMS", 200, 18));
            flowLayoutPanel2.Controls.Add(titlePanel);

            Panel headerPanel = new Panel
            {
                Width = 540,
                Height = 40,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10)
            };
            

            int fontSize = 12;
            headerPanel.Controls.Add(CreateHeaderLabel("Room No", 45, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Type", 162, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Price", 310, fontSize));
            flowLayoutPanel2.Controls.Add(headerPanel);

            while (rdr.Read())
            {
                string room = rdr["ROOM_ID"].ToString();
                string type = rdr["TYPE"].ToString();
                string pricePerNight = rdr["PPN"].ToString();



                Panel taskPanel = new Panel
                {
                    Width = 540,
                    Height = 55,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(5)
                };
                TextBox price = new TextBox
                {
                    Text = pricePerNight,
                    Name = "txtPrice",
                    Font = new Font("Segoe Print", 11, FontStyle.Bold),
                    TextAlign=HorizontalAlignment.Center,
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    AutoSize = true,
                    Top = 5,
                    Left = 300
                };
                Button UpdateBtn1 = new Button
                {
                    Text = "Update",
                    Font = new Font("Segoe Print", 11, FontStyle.Bold),
                    BackColor = Color.FromArgb(43, 92, 122),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Top = 5,
                    Left = 450,
                    Tag = room
                };
                UpdateBtn1.Click += (s, e) =>
                {
                    UpdateRoomPrice(room, price.Text.Trim());
                    load_rooms(); 
                };



                taskPanel.Controls.Add(CreateDataLabel(room, 50, fontSize));
                taskPanel.Controls.Add(CreateDataLabel(type, 145, fontSize));
                taskPanel.Controls.Add(price);
                taskPanel.Controls.Add(UpdateBtn1);


                flowLayoutPanel2.Controls.Add(taskPanel);
            }

        }
        //Back button
        private void button1_Click(object sender, EventArgs e)
        {
            Admin_Dashboard admin_Dashboard = new Admin_Dashboard();
            admin_Dashboard.Show();
            this.Visible = false;
        }
    }
}
