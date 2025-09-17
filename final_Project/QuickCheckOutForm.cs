using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace final_Project
{
    public partial class QuickCheckOutForm : Form
    {
        private string receptionistEmail;

        public QuickCheckOutForm(string email)
        {
            receptionistEmail = email;
            InitializeComponent();
            LoadActiveBookings();
        }

        private void LoadActiveBookings()
        {
            try
            {
                // FIXED: Use proper connection string
                string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT BOOKING_ID, EMAIL, ROOM_ID, CHECK_IN, CHECK_OUT
                                   FROM BOOKINGS_TABLE 
                                   WHERE BOOKING_STATUS = 'CHECK-IN' 
                                   ORDER BY CHECK_IN DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    cmbBookings.Items.Clear(); // Clear existing items

                    while (reader.Read())
                    {
                        string bookingId = reader["BOOKING_ID"].ToString();
                        string roomId = reader["ROOM_ID"].ToString();
                        string email = reader["EMAIL"].ToString();
                        string checkIn = Convert.ToDateTime(reader["CHECK_IN"]).ToString("yyyy-MM-dd");
                        string checkOut = Convert.ToDateTime(reader["CHECK_OUT"]).ToString("yyyy-MM-dd");

                        string bookingInfo = $"Room {roomId} - {email} ({checkIn} to {checkOut})";
                        cmbBookings.Items.Add(new ComboBoxItem(bookingInfo, bookingId));
                    }

                    if (cmbBookings.Items.Count > 0)
                        cmbBookings.SelectedIndex = 0;
                    else
                        MessageBox.Show("No active check-ins found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bookings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (cmbBookings.SelectedItem != null)
            {
                string bookingId = ((ComboBoxItem)cmbBookings.SelectedItem).Value;

                DialogResult confirm = MessageBox.Show("Are you sure you want to check out this guest?",
                    "Confirm Check-Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True";

                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = "UPDATE BOOKINGS_TABLE SET BOOKING_STATUS = 'CHECK-OUT' WHERE BOOKING_ID = @BookingId";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@BookingId", bookingId);

                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Check-out successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error during check-out: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a booking to check out.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Helper class to store both display text and value
        private class ComboBoxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }

            public ComboBoxItem(string text, string value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}