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
    public partial class Home : Form
    {
        string email="";
        public Home()
        {
            InitializeComponent();
        }
        public Home(string email)
        {
            InitializeComponent();
            this.email = email;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
           // getUser();
        }
        //log in button
        private void button3_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
            this.Visible = false;
        }
        void Login()
        {
            Login login = new Login();
            login.Show();
            this.Visible = false;
        }

        //see more button
        private void button1_Click_1(object sender, EventArgs e)
        {
         
            if(email != "")
            {
                Rooms rooms = new Rooms(email);
                rooms.Show();
                this.Visible = false;
            }
            else
            {
                Login();
            }
            
        }
        void getUser()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";

                SqlConnection conn = new SqlConnection(connectionString);        
                conn.Open();
                string q = "SELECT NAME FROM USER_TABLE WHERE EMAIL LIKE '" + email + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtuser.Text = reader["NAME"].ToString();
                }
            
        }

        //user label 
        private void txtuser_Click(object sender, EventArgs e)
        {

            if (email != "")
            {
                string type = "home";
                User_Profile user_Profile = new User_Profile(email,type);
                user_Profile.Show();
                this.Visible = false;
            }
            else
            {
                Login();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
