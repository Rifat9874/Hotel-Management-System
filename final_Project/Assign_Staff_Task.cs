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
    public partial class Assign_Staff_Task : Form
    {
        public Assign_Staff_Task()
        {
            InitializeComponent();
        }

        private void Assign_Staff_Task_Load(object sender, EventArgs e)
        {
            load_rooms();
            load_staff_email();
            load_room_assignment();
        }

        SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }

        void load_staff_email()
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    string q = @"SELECT EMAIL FROM USER_TABLE WHERE USER_TYPE = 'STAFF'";
                    SqlCommand cmd = new SqlCommand(q, con);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    cmemail.Items.Clear();
                    while (rdr.Read())
                    {
                        string email = rdr["EMAIL"].ToString();
                        cmemail.Items.Add(email);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading staff emails: " + ex.Message);
            }
        }

        void load_rooms()
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    string q = @"SELECT ROOM_ID FROM ROOM_TABLE";
                    SqlCommand cmd = new SqlCommand(q, con);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    cmroom.Items.Clear();
                    while (rdr.Read())
                    {
                        string rId = rdr["ROOM_ID"].ToString();
                        cmroom.Items.Add(rId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading rooms: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(cmroom.Text) || string.IsNullOrEmpty(cmemail.Text))
            {
                MessageBox.Show("Please select both room and staff email!");
                return;
            }

            try
            {
                using (SqlConnection con = GetConnection())
                {
                    DateTime today = DateTime.Now;

                    // Generate unique ID properly
                    string IdQuery = @"SELECT ISNULL(MAX(CAST(ASSIGNMENT_ID AS INT)), 0) + 1 
                                     FROM ROOM_ASSIGNMENTS 
                                     WHERE ISNUMERIC(ASSIGNMENT_ID) = 1";
                    SqlCommand cmd1 = new SqlCommand(IdQuery, con);
                    int newId = (int)cmd1.ExecuteScalar();
                    string Id = newId.ToString("D4"); // 4-digit format (0001, 0002, etc.)

                    // Use parameterized query to prevent SQL injection
                    string s = @"INSERT INTO ROOM_ASSIGNMENTS 
                               (ASSIGNMENT_ID, ROOM_ID, ASSIGNED_DATE, EMAIL, ROOM_STATUS) 
                               VALUES (@id, @room, @date, @email, @status)";

                    SqlCommand cmd2 = new SqlCommand(s, con);
                    cmd2.Parameters.AddWithValue("@id", Id);
                    cmd2.Parameters.AddWithValue("@room", cmroom.Text);
                    cmd2.Parameters.AddWithValue("@date", today.ToString("yyyy-MM-dd"));
                    cmd2.Parameters.AddWithValue("@email", cmemail.Text);
                    cmd2.Parameters.AddWithValue("@status", "DIRTY");

                    int rowsAffected = cmd2.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Task assigned successfully!");
                        load_room_assignment();

                        // Clear selections
                        cmroom.SelectedIndex = -1;
                        cmemail.SelectedIndex = -1;
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Primary key violation
                {
                    MessageBox.Show("Duplicate assignment detected. Please try again.");
                }
                else
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
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
                Top = 8
            };
        }

        void load_room_assignment()
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    string q = @"SELECT ASSIGNMENT_ID, ROOM_ID, ASSIGNED_DATE, EMAIL, ROOM_STATUS 
                               FROM ROOM_ASSIGNMENTS 
                               ORDER BY ASSIGNMENT_ID DESC";
                    SqlCommand cmd = new SqlCommand(q, con);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    flowLayoutPanel1.Controls.Clear();

                    // Create header
                    Panel headerPanel = new Panel
                    {
                        Width = 780,
                        Height = 40,
                        BorderStyle = BorderStyle.None,
                        Margin = new Padding(5)
                    };

                    headerPanel.Controls.Add(CreateHeaderLabel("Assign ID", 0, 14));
                    headerPanel.Controls.Add(CreateHeaderLabel("Room ID", 120, 14));
                    headerPanel.Controls.Add(CreateHeaderLabel("Assigned Date", 240, 14));
                    headerPanel.Controls.Add(CreateHeaderLabel("Email", 440, 14));
                    headerPanel.Controls.Add(CreateHeaderLabel("Room Status", 615, 14));

                    flowLayoutPanel1.Controls.Add(headerPanel);

                    // Loop through records
                    while (rdr.Read())
                    {
                        string aId = rdr["ASSIGNMENT_ID"].ToString();
                        string rId = rdr["ROOM_ID"].ToString();
                        string email = rdr["EMAIL"].ToString();
                        string aDate = Convert.ToDateTime(rdr["ASSIGNED_DATE"]).ToString("yyyy-MM-dd");
                        string rStatus = rdr["ROOM_STATUS"].ToString();

                        Panel taskPanel = new Panel
                        {
                            Width = 780,
                            Height = 35,
                            BorderStyle = BorderStyle.None,
                            Margin = new Padding(5)
                        };

                        taskPanel.Controls.Add(CreateDataLabel(aId, 10, 12));
                        taskPanel.Controls.Add(CreateDataLabel(rId, 130, 12));
                        taskPanel.Controls.Add(CreateDataLabel(aDate, 250, 12));
                        taskPanel.Controls.Add(CreateDataLabel(email, 415, 12));
                        taskPanel.Controls.Add(CreateDataLabel(rStatus, 635, 12));

                        flowLayoutPanel1.Controls.Add(taskPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading room assignments: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Admin_Dashboard admin_Dashboard = new Admin_Dashboard();
            admin_Dashboard.Show();
            this.Hide(); // Use Hide() instead of Visible = false for better practice
        }

        // Clean up when form closes
        private void Assign_Staff_Task_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Any cleanup code if needed
        }
    }
}