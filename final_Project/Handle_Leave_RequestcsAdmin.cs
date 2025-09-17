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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace final_Project
{
    
    public partial class Handle_Leave_RequestcsAdmin : Form
    {
        string lId;
        public Handle_Leave_RequestcsAdmin()
        {
            InitializeComponent();
        }

        private void Handle_Leave_Requestcs_Load(object sender, EventArgs e)
        {
            load_leave();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Admin_Dashboard admin_Dashboard = new Admin_Dashboard();
            admin_Dashboard.Show();
            this.Visible = false;
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
                Top = 10
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
                Top = 10
            };
        }
        //Approve button
        private void approveBtn1_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string selectedId = clickedButton.Tag.ToString();
            SqlConnection con = GetConnection();
            string q = "UPDATE LEAVE_REQUEST SET STATUS='APPROVED' WHERE LEAVE_ID LIKE  '" + selectedId + "'";
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            load_leave();


        }
        //Dismiss button
        private void dismissBtn2Btn1_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string selectedId = clickedButton.Tag.ToString();
            SqlConnection con = GetConnection();
            string q = "UPDATE LEAVE_REQUEST SET STATUS='DISMISSED' WHERE LEAVE_ID LIKE  '" + selectedId + "'";
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.ExecuteNonQuery();
            load_leave();
        }
        void load_leave()
        {
            SqlConnection con = GetConnection();
            string q = "SELECT RA.LEAVE_ID,RA.FROM_DATE,RA.TO_DATE,RA.REASON,RA.STATUS, SA.NAME " +
                           "FROM LEAVE_REQUEST RA " +
                           "JOIN USER_TABLE SA ON RA.EMAIL = SA.EMAIL WHERE RA.STATUS = 'Pending'";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader rdr = cmd.ExecuteReader();

            flowLayoutPanel1.Controls.Clear();

            Panel headerPanel = new Panel
            {
                Width = 900,
                Height = 40,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10)
            };

            int fontSize = 12;
            headerPanel.Controls.Add(CreateHeaderLabel("Name", 15, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("From", 155, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("To", 295, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Reason", 415, fontSize));
            headerPanel.Controls.Add(CreateHeaderLabel("Status", 586, fontSize));
            flowLayoutPanel1.Controls.Add(headerPanel);

            while (rdr.Read())
            {
                lId = rdr["LEAVE_ID"].ToString();
                string from = rdr["FROM_DATE"].ToString();
                string to = rdr["TO_DATE"].ToString();
                string reason = rdr["REASON"].ToString();
                string status = rdr["STATUS"].ToString();
                string name = rdr["NAME"].ToString();



                Panel taskPanel = new Panel
                {
                    Width = 1000,
                    Height = 50,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(5)
                };
                Button approveBtn1 = new Button
                {
                    Text = "Approave",
                    Font = new Font("Segoe Print", 11, FontStyle.Bold),
                    BackColor = Color.FromArgb(43, 92, 122),
                    ForeColor = Color.White,
                    Width = 120,
                    Height = 35,
                    Top = 10,
                    Left = 680,
                    Tag = lId
                };
                approveBtn1.Click += approveBtn1_Click;

                Button dismissBtn2 = new Button
                {
                    Text = "Dismiss",
                    Font = new Font("Segoe Print", 11, FontStyle.Bold),
                    BackColor = Color.FromArgb(43, 92, 122),
                    ForeColor = Color.White,
                    Width = 120,
                    Height = 35,
                    Top = 10,
                    Left = 820,
                    Tag=lId
                };
                dismissBtn2.Click += dismissBtn2Btn1_Click;

                taskPanel.Controls.Add(CreateDataLabel(name, 12, fontSize));
                taskPanel.Controls.Add(CreateDataLabel(from, 145, fontSize));
                taskPanel.Controls.Add(CreateDataLabel(to, 270, fontSize));
                taskPanel.Controls.Add(CreateDataLabel(reason, 395, fontSize));
                taskPanel.Controls.Add(CreateDataLabel(status, 585, fontSize));
                taskPanel.Controls.Add(approveBtn1);
                taskPanel.Controls.Add(dismissBtn2);


                flowLayoutPanel1.Controls.Add(taskPanel);
            }
        }
    }
}
