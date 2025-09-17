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
    public partial class Login : Form
    {
        string type;
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            AddBottomBorder(txtemail);
            AddBottomBorder(txtpassword);
        }

        //design purpose -- to show only the bottom line of text-box
        private void AddBottomBorder(TextBox textBox)
        {
            Label bottomBorder = new Label();
            bottomBorder.Height = 3; 
            bottomBorder.Dock = DockStyle.Bottom;
            bottomBorder.BackColor = Color.Black; 

            textBox.Controls.Add(bottomBorder);
        }

        //back button
        private void button2_Click(object sender, EventArgs e)
        {
            Home form1 = new Home();
            form1.Show();
            this.Visible = false;
        }

        //sign up link label
        private void linksignup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Visible=false;
        }

        //sign up button
        private void btnsignin_Click(object sender, EventArgs e)
        {
            if (txtemail.Text == "" || txtpassword.Text == "")
            {
                MessageBox.Show("Please enter both Email and Password.");
                return;
            }
            else
            {
                bool flag = true;
                string source = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
                SqlConnection con = new SqlConnection(source);
                con.Open();
                string q = "SELECT PASSWORD,USER_TYPE FROM USER_TABLE WHERE EMAIL='" + txtemail.Text + "'";
                SqlCommand cmd = new SqlCommand(q, con);
                SqlDataReader reader = cmd.ExecuteReader();
                string password = "";
                if (reader.Read())
                {
                    password = reader["PASSWORD"].ToString();
                    type=reader["USER_TYPE"].ToString();
                }
                else
                {
                    MessageBox.Show("Invalid Email");
                    flag = false;
                }

                if (password == txtpassword.Text)
                {
                    if(type == "CUSTOMER")
                    {
                        Rooms rooms = new Rooms(txtemail.Text);
                        rooms.Show();
                        this.Visible = false;
                    }
                    else if (type == "RECEPTIONIST")
                    { 
                        Reciptionist reciptionist = new Reciptionist(txtemail.Text);
                        reciptionist.Show();
                        this.Visible=false;
                    }
                    else if (type == "STAFF")
                    {
                        StaffDashboard staffDashboard=new StaffDashboard(txtemail.Text);
                        staffDashboard.Show();
                        this.Visible = false;
                    }
                    else if (type == "ADMIN")
                    {
                        Admin_Dashboard admin_=new Admin_Dashboard(txtemail.Text);
                        admin_.Show();
                        this.Visible = false;
                    }

                }
                else
                {
                    if (flag)
                    {
                        MessageBox.Show("Invalid Password");
                    }

                }
            }

        }

        //forgot password Link
        private void linkforgotpasswod_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Forgot_Password forgot_Password = new Forgot_Password();
            forgot_Password.Show();
            this.Visible=false;
        }
    }
}
