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
    public partial class Reciptionist_Leave : Form
    {
        string email="";
        public Reciptionist_Leave()
        {
            InitializeComponent();
        }
        public Reciptionist_Leave(string email)
        {
            this.email = email;
            InitializeComponent();
        }

        private void Reciptionist_Leave_Load(object sender, EventArgs e)
        {
            load_history();
        }
        SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }

        private void btnsubmit_Click(object sender, EventArgs e)
        {
            
        }
        private Label CreateHeaderLabel(string text, int left)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor= Color.Black,
                AutoSize = true,
                Top = 0,
                Left = left
            };
        }

        private Label CreateDataLabel(string text, int left)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.Black,
                AutoSize = true,
                Top = 0,
                Left = left
            };
        }
        void load_history()
        {
            SqlConnection con = GetConnection();
            string q = @"SELECT 
                        RA.FROM_DATE,
                        RA.TO_DATE,
                        RA.REASON,
                        RA.STATUS,
                        UT.NAME 
                    FROM LEAVE_REQUEST RA
                    JOIN USER_TABLE UT ON RA.EMAIL = UT.EMAIL
                    WHERE RA.EMAIL LIKE '" + email + "'";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader rdr = cmd.ExecuteReader();
            flowLayoutPanel2.Controls.Clear();

            // === HEADER PANEL ===
            Panel headerPanel = new Panel
            {
                Width = 650,
                Height = 50,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(1)
            };

            headerPanel.Controls.Add(CreateHeaderLabel("NAME", 0));
            headerPanel.Controls.Add(CreateHeaderLabel("FROM", 105));
            headerPanel.Controls.Add(CreateHeaderLabel("TO", 225));
            headerPanel.Controls.Add(CreateHeaderLabel("REASON", 345));
            headerPanel.Controls.Add(CreateHeaderLabel("STATUS", 507));

            Panel bottomBorder = new Panel
            {
                Height = 2,
                Dock = DockStyle.Bottom,
                BackColor = Color.Gray
            };

            headerPanel.Controls.Add(bottomBorder);
            flowLayoutPanel2.Controls.Add(headerPanel);

            // === DATA ROWS ===
            while (rdr.Read())
            {
                string name = rdr["NAME"].ToString();
                string from = Convert.ToDateTime(rdr["FROM_DATE"]).ToString("yyyy-MM-dd");
                string to = Convert.ToDateTime(rdr["TO_DATE"]).ToString("yyyy-MM-dd");
                string reason = rdr["REASON"].ToString();
                string status = rdr["STATUS"].ToString();

                Panel taskPanel = new Panel
                {
                    Width = 640,
                    Height = 50,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(5)
                };

                taskPanel.Controls.Add(CreateDataLabel(name, 0));
                taskPanel.Controls.Add(CreateDataLabel(from, 100));
                taskPanel.Controls.Add(CreateDataLabel(to, 210));
                taskPanel.Controls.Add(CreateDataLabel(reason, 340));
                taskPanel.Controls.Add(CreateDataLabel(status, 500));

                flowLayoutPanel2.Controls.Add(taskPanel);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reciptionist reciptionist = new Reciptionist(email);
            reciptionist.Show();
            this.Visible = false;
        }

        private void btnsubmit_Click_1(object sender, EventArgs e)
        {
            SqlConnection con = GetConnection();

            try
            {
                // Get the maximum numeric value from LEAVE_ID (handle mixed data types)
                string IdQuery = @"
            SELECT ISNULL(MAX(CAST(
                CASE WHEN ISNUMERIC(LEAVE_ID) = 1 THEN LEAVE_ID 
                ELSE '0' END AS INT)), 0) + 1 
            FROM LEAVE_REQUEST";

                SqlCommand cmd1 = new SqlCommand(IdQuery, con);
                int nextId = (int)cmd1.ExecuteScalar();

                // Insert with quotes since LEAVE_ID is nvarchar
                string q = "INSERT INTO LEAVE_REQUEST VALUES ('" + nextId + "', '" + email + "', '" +
                           txtfromdate.Value.ToString("yyyy-MM-dd") + "', '" +
                           txttodate.Value.ToString("yyyy-MM-dd") + "', '" + cmreason.Text + "', 'Pending')";

                SqlCommand cmd2 = new SqlCommand(q, con);
                cmd2.ExecuteNonQuery();

                MessageBox.Show("Leave request submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_history();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
