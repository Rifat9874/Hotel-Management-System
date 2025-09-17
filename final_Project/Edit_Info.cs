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
    public partial class Edit_Info : Form
    {
        string email,roomId,type;
        public Edit_Info()
        {
            InitializeComponent();
        }
        public Edit_Info(string email,string type)
        {
            this.email = email;
            this.type = type;
            InitializeComponent();
        }
        public Edit_Info(string email)
        {
            this.email = email;
            InitializeComponent();
        }
        public Edit_Info(string email, string roomId, string type)
        {
            this.email = email;
            this.roomId = roomId;
            this.type = type;
            InitializeComponent();
        }

        private void Edit_Info_Load(object sender, EventArgs e)
        {
            show();
        }
        //showing information in text-box
        void show()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand($"SELECT NAME,PHONE_NUMBER,EMAIL,PASSWORD FROM USER_TABLE WHERE EMAIL LIKE '" + email + "'", con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                txtname.Text = reader["NAME"].ToString();
                txtnumber.Text= reader["PHONE_NUMBER"].ToString();
                txtemail.Text = reader["EMAIL"].ToString();
                txtpassword.Text = reader["PASSWORD"].ToString();
            }
        }
        //submit button
        private void button1_Click(object sender, EventArgs e)
        {
            update();
            show();
        }
        //back button
        private void button2_Click(object sender, EventArgs e)
        {
            Profile();
        }

        void update()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string q = "UPDATE USER_TABLE SET NAME='"+txtname.Text+"', PHONE_NUMBER='"+txtnumber.Text+"',EMAIL='"+txtemail.Text+"', PASSWORD='" + txtpassword.Text + "' WHERE EMAIL LIKE '" +email+ "'";
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            Profile();

        }
        void Profile()
        {
            if (type == "room")
            {
                User_Profile user_Profile = new User_Profile(email,type);
                user_Profile.Show();
                this.Visible = false;
            }
            else if (type == "room_details")
            {
                User_Profile user_Profile = new User_Profile(email, roomId, type);
                user_Profile.Show();
                this.Visible = false;
            }
            else if (type == "confirm_bookings")
            {
                User_Profile user_Profile = new User_Profile(email, roomId, type);
                user_Profile.Show();
                this.Visible = false;
            }
            else if (type == "home")
            {
                User_Profile user_Profile = new User_Profile(email, type);
                user_Profile.Show();
                this.Visible = false;
            }
            else if (type == "staff")
            {
                Employee_Profile user_Profile = new Employee_Profile(email, "staff");
                user_Profile.Show();
                this.Visible = false;
            }
            else if (type == "receptionist")
            {
                Employee_Profile user_Profile = new Employee_Profile(email, "receptionist");
                user_Profile.Show();
                this.Visible = false;
            }
        }
    }
}
