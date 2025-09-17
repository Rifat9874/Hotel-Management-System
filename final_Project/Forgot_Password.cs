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
    public partial class Forgot_Password : Form
    {
        public Forgot_Password()
        {
            InitializeComponent();
        }

        private void Forgot_Password_Load(object sender, EventArgs e)
        {
            AddBottomBorder(txtemail);
            AddBottomBorder(txtnumber);
            AddBottomBorder(txtpassword);
        }

        private void AddBottomBorder(TextBox textBox)
        {
            Label bottomBorder = new Label();
            bottomBorder.Height = 3;
            bottomBorder.Dock = DockStyle.Bottom;
            bottomBorder.BackColor = Color.Black;
            textBox.Controls.Add(bottomBorder);
        }

        SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }

        //Confirm button
        private void button1_Click(object sender, EventArgs e)
        {
            string number = "";
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT PHONE_NUMBER FROM USER_TABLE WHERE EMAIL = @Email", conn);
                    cmd.Parameters.AddWithValue("@Email", txtemail.Text);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        number = reader["PHONE_NUMBER"].ToString();
                    }
                    reader.Close();
                }

                if (number == txtnumber.Text)
                {
                    updatePassword();
                }
                else
                {
                    MessageBox.Show("Invalid Information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void updatePassword()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    string q = "UPDATE USER_TABLE SET PASSWORD = @Password WHERE EMAIL = @Email";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@Password", txtpassword.Text);
                    cmd.Parameters.AddWithValue("@Email", txtemail.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Your password has been successfully changed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        login();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update password. Email not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating password: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void login()
        {
            Login login = new Login();
            login.Show();
            this.Hide(); // Use Hide() instead of Visible = false for better practice
        }

        //Back button
        private void button2_Click(object sender, EventArgs e)
        {
            login();
        }
    }
}