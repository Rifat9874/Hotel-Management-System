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
    public partial class Facility_Booking : Form
    {
        private string email;
        private string facilityName;

        public Facility_Booking(string email, string facilityName)
        {
            InitializeComponent();
            this.email = email;
            this.facilityName = facilityName;
        }

        private void Facility_Booking_Load(object sender, EventArgs e)
        {
            lblFacilityName.Text = facilityName;
            dtpBookingDate.MinDate = DateTime.Today;
            dtpBookingDate.Value = DateTime.Today;

            // Set default values based on facility type
            switch (facilityName)
            {
                case "Swimming Pool":
                    lblPrice.Text = "TK1000.00 per person";
                    numGuests.Value = 1;
                    numGuests.Maximum = 4;
                    break;
                case "Breakfast Buffet":
                    lblPrice.Text = "TK1200.00 per person";
                    numGuests.Value = 1;
                    numGuests.Maximum = 4;
                    break;
                case "Spa Services":
                    lblPrice.Text = "TK2500.00 per treatment";
                    numGuests.Value = 1;
                    numGuests.Maximum = 2;
                    break;
                case "Fitness Center":
                    lblPrice.Text = "TK1000.00 per person";
                    numGuests.Value = 1;
                    numGuests.Maximum = 1;
                    break;
                case "Business Center":
                    lblPrice.Text = "TK800.00 per hour";
                    numGuests.Value = 1;
                    numGuests.Maximum = 1;
                    lblGuests.Text = "Hours:";
                    break;
            }

            CalculateTotal();
        }

        private void CalculateTotal()
        {
            decimal pricePerUnit = 0;
            int units = (int)numGuests.Value;

            switch (facilityName)
            {
                case "Swimming Pool": pricePerUnit = 1000.00m; break;
                case "Breakfast Buffet": pricePerUnit = 1200.00m; break;
                case "Spa Services": pricePerUnit = 2500.00m; break;
                case "Fitness Center": pricePerUnit = 1000.00m; break;
                case "Business Center": pricePerUnit = 800.00m; break;
            }

            decimal total = pricePerUnit * units;
            lblTotal.Text = $"Total: TK{total:F2}";
        }

        private void numGuests_ValueChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = GetConnection();
                string query = @"INSERT INTO FACILITIES_BOOKING (EMAIL, FACILITY_NAME, BOOKING_DATE, GUESTS_COUNT, TOTAL_PRICE, BOOKING_STATUS) 
                                VALUES (@email, @facilityName, @bookingDate, @guestsCount, @totalPrice, 'PENDING')";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@facilityName", facilityName);
                cmd.Parameters.AddWithValue("@bookingDate", dtpBookingDate.Value);
                cmd.Parameters.AddWithValue("@guestsCount", (int)numGuests.Value);

                decimal pricePerUnit = 0;
                switch (facilityName)
                {
                    case "Swimming Pool": pricePerUnit = 1000.00m; break;
                    case "Breakfast Buffet": pricePerUnit = 1200.00m; break;
                    case "Spa Services": pricePerUnit = 2500.00m; break;
                    case "Fitness Center": pricePerUnit = 1000.00m; break;
                    case "Business Center": pricePerUnit = 800.00m; break;
                }

                decimal totalPrice = pricePerUnit * (int)numGuests.Value;
                cmd.Parameters.AddWithValue("@totalPrice", totalPrice);

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show($"Your {facilityName} booking has been confirmed!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error booking facility: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        SqlConnection GetConnection()
        {
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }
    }
}