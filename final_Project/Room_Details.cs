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
    public partial class Room_Details : Form
    {
        string roomId, email, imagePath, imagePath1, imagePath2, imagePath3;
        public Room_Details()
        {
            InitializeComponent();
        }
        public Room_Details(string roomId,string email)
        {
            this.roomId = roomId;
            this.email = email;
            InitializeComponent();
        }

        private void Room_Details_Load(object sender, EventArgs e)
        {
            LoadDetailsRoom();
            load_additional_pic();
            getUser();
        }
        SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }
        //load individual room data
        private void LoadDetailsRoom() 
        { 
                SqlConnection conn=GetConnection();

                SqlCommand cmd = new SqlCommand("SELECT TYPE, PPN, DESCRIPTION, PICTURE, COMPLIMENTARY, FEATURES FROM ROOM_TABLE  WHERE ROOM_ID LIKE '" + roomId + "'", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    txtprice.Text = reader["PPN"].ToString();
                    txtdescription.Text = reader["DESCRIPTION"].ToString();
                    txttype.Text = reader["TYPE"].ToString();
                    txtcomplimentary.Text = reader["COMPLIMENTARY"].ToString();
                    txtfeatures.Text = reader["FEATURES"].ToString();


                    imagePath = reader["PICTURE"].ToString();
                    if (File.Exists(imagePath))
                    {
                        pictureBox1.Image = Image.FromFile(imagePath);
                    }
                    else
                    {
                        pictureBox1.Image = SystemIcons.Warning.ToBitmap();
                    }

                }
            
        }
        //load multiple picture of a room
        void load_additional_pic()
        {
            SqlConnection conn = GetConnection();

            SqlCommand cmd = new SqlCommand("SELECT  IMAGE_1, IMAGE_2, IMAGE_3 FROM ADDITIONAL_PICTURES  WHERE ROOM_ID LIKE '" + roomId + "'", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                imagePath1 = reader["IMAGE_1"].ToString();
                imagePath2 = reader["IMAGE_2"].ToString();
                imagePath3 = reader["IMAGE_3"].ToString();
                if (File.Exists(imagePath1))
                {
                    apic1.Image = Image.FromFile(imagePath1);
                }
                else
                {
                    apic1.Image = SystemIcons.Warning.ToBitmap();
                }

                if (File.Exists(imagePath2))
                {
                    apic2.Image = Image.FromFile(imagePath2);
                }
                else
                {
                    apic2.Image = SystemIcons.Warning.ToBitmap();
                }

                if (File.Exists(imagePath3))
                {
                    apic3.Image = Image.FromFile(imagePath3);
                }
                else
                {
                    apic3.Image = SystemIcons.Warning.ToBitmap();
                }

            }
        }
        //Back button
        private void button2_Click(object sender, EventArgs e)
        {
            Rooms rooms = new Rooms(email);
            rooms.Show();
            this.Visible = false;
        }

        
        //Book Button
        private void button1_Click(object sender, EventArgs e)
        {
            Confirm_Bookings confirm_Bookings = new Confirm_Bookings(roomId,email);
            confirm_Bookings.Show();
            this.Visible = false;
        }

        

        void getUser()
        {
            SqlConnection conn = GetConnection();
            SqlCommand cmd = new SqlCommand($"SELECT NAME FROM USER_TABLE WHERE EMAIL LIKE '" + email + "'", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    txtuser.Text = reader["NAME"].ToString();
                }
            
        }
        //hover event on pictures
        private void apic1_MouseHover_1(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(imagePath1);
        }
        private void apic1_MouseLeave_1(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(imagePath);
        }

        private void apic2_MouseHover_1(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(imagePath2);
        }
       private void apic2_MouseLeave_1(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(imagePath);
        }
        private void apic3_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(imagePath3);
        }
        private void apic3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(imagePath);
        }
        //user profile button
        private void button3_Click(object sender, EventArgs e)
        {
            string type = "room_details";
            User_Profile user_Profile = new User_Profile(email, roomId, type);
            user_Profile.Show();
            this.Visible = false;
        }

 
    }
}
