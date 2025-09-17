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
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }
        private void Registration_Load(object sender, EventArgs e)
        {
            AddBottomBorder(txtname);
            AddBottomBorder(txtemail);
            AddBottomBorder(txtpassword);
            AddBottomBorder(txtcpassword);
            AddBottomBorder(txtnumber);
        }
        private void AddBottomBorder(TextBox textBox)
        {

            Label bottomBorder = new Label();
            bottomBorder.Height = 3;
            bottomBorder.Dock = DockStyle.Bottom;
            bottomBorder.BackColor = Color.Black;

            textBox.Controls.Add(bottomBorder);
        }

        //the events are use here for the place holder issue
        private void txtname_Enter(object sender, EventArgs e)
        {
            if (txtname.Text == "  Full Name") { txtname.Text = ""; txtname.ForeColor = Color.Black; }
        }

        private void txtname_Leave(object sender, EventArgs e)
        {
            if (txtname.Text == "") { txtname.Text = "  Full Name"; txtname.ForeColor = Color.Gray; }
        }

        private void txtnumber_Enter(object sender, EventArgs e)
        {
            if (txtnumber.Text == "  Phone Number") { txtnumber.Text = ""; txtnumber.ForeColor = Color.Black; }
        }
        private void txtnumber_Leave(object sender, EventArgs e)
        {
            if (txtnumber.Text == "") { txtnumber.Text = "  Phone Number"; txtnumber.ForeColor = Color.Gray; }
        }

        private void txtemail_Enter(object sender, EventArgs e)
        {
            if (txtemail.Text == "  E-Mail") { txtemail.Text = ""; txtemail.ForeColor = Color.Black; }
        }

        private void txtemail_Leave(object sender, EventArgs e)
        {
            if (txtemail.Text == "") { txtemail.Text = "  E-Mail"; txtemail.ForeColor = Color.Gray; }
        }

        private void txtpassword_Enter(object sender, EventArgs e)
        {
            if (txtpassword.Text == "  Password") 
            {
                txtpassword.Text = "";
                txtpassword.ForeColor = Color.Black;
                txtpassword.PasswordChar = '*';
            }
        }

        private void txtpassword_Leave(object sender, EventArgs e)
        {
            if (txtpassword.Text == "") 
            { 
                txtpassword.Text = "  Password";
                txtpassword.ForeColor = Color.Gray;
                txtpassword.PasswordChar = '\0';
            }
        }

        private void txtcpassword_Enter(object sender, EventArgs e)
        {
            if (txtcpassword.Text == "  Confirm Password") { txtcpassword.Text = ""; txtcpassword.ForeColor = Color.Black;
                txtcpassword.PasswordChar = '*';
            }
        }

        private void txtcpassword_Leave(object sender, EventArgs e)
        {
            if (txtcpassword.Text == "") { txtcpassword.Text = "  Confirm Password"; txtcpassword.ForeColor = Color.Gray;
                txtcpassword.PasswordChar = '\0';
            }
        }

        //back button
        private void button1_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Visible = false;
        }

        //enter data to database
        void insert()
        {
            string source = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(source);
            con.Open();
            string q = "INSERT INTO USER_TABLE VALUES('" + txtname.Text + "','" + txtnumber.Text + "','" + txtemail.Text + "','" + txtcpassword.Text + "','CUSTOMER')";
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();

           
        }

        //validity of email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        //sign up button
        private void btnsignup_Click(object sender, EventArgs e)
        {
            bool isValid = true;

            // Name Validation
            if (string.IsNullOrWhiteSpace(txtname.Text) || txtname.Text == "  Full Name")
            {
                txtvalidname.Text = "Invalid Name";
                isValid = false;
            }
            else
            {
                txtvalidname.Text = "";
            }

            // Number Validation
            if (txtnumber.Text.Length != 11 || !txtnumber.Text.All(char.IsDigit))
            {
                txtvalidnumber.Text = "Invalid Number";
                isValid = false;
            }
            else
            {
                txtvalidnumber.Text = "";
            }

            // Email Validation
            if (!IsValidEmail(txtemail.Text))
            {
                txtvalidemail.Text = "Invalid Email";
                isValid = false;
            }
            else
            {
                txtvalidemail.Text = "";
            }

            // Password Validation
            if (txtpassword.Text != txtcpassword.Text)
            {
                txtvalidpass.Text = "Passwords do not match!";
                isValid = false;
            }
            else if (txtpassword.Text.Length < 8)
            {
                txtvalidpass.Text = "Password must be at least 8 characters!";
                isValid = false;
            }
            else
            {
                txtvalidpass.Text = "";
            }

            // If everything is valid, insert data
            if (isValid)
            {
                insert();
                Login login = new Login();
                login.Show();
                this.Visible = false;
            }
        }

    }
}
