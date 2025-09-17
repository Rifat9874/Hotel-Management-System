using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace final_Project
{
    public partial class Update_Booking_Status : Form
    {
        string bookingId,email;
        public Update_Booking_Status()
        {
            InitializeComponent();
        }
        public Update_Booking_Status(string bookingId,string email)
        {
            this.email = email;
            this.bookingId= bookingId;
            InitializeComponent();
        }

        private void Update_Booking_Status_Load(object sender, EventArgs e)
        {
            load_details();
        }
        SqlConnection GetConnection()
        {
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }
        //load detail about a particular booking
        void load_details()
        {
            SqlConnection con = GetConnection();
            string q= "SELECT  RA.EMAIL,RA.BOOKING_DATE,RA.ROOM_ID, RA.CHECK_IN, RA.CHECK_OUT, RA.BOOKING_STATUS, UT.NAME " +
                "FROM BOOKINGS_TABLE RA JOIN USER_TABLE UT ON RA.EMAIL = UT.EMAIL WHERE BOOKING_ID LIKE '"+bookingId+"'";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                txtname.Text = rdr["NAME"].ToString();
                txtroom.Text = rdr["ROOM_ID"].ToString();
                txtcheckin.Text = rdr["CHECK_IN"].ToString();
                txtcheckout.Text = rdr["CHECK_OUT"].ToString();
                txtbdate.Text = rdr["BOOKING_DATE"].ToString();
                txtstatus.Text = rdr["BOOKING_STATUS"].ToString();
            }
        }
        //Back button
        private void btnback_Click(object sender, EventArgs e)
        {
            Reciptionist reciptionist = new Reciptionist(email);
            reciptionist.Show();
            this.Visible = false;
        }
        //Booked button (for update status as Booked according to payment issue)
        private void btnbooked_Click(object sender, EventArgs e)
        {
            SqlConnection conn = GetConnection();
            string q = "UPDATE BOOKINGS_TABLE SET BOOKING_STATUS='BOOKED' WHERE BOOKING_ID LIKE  '" + bookingId + "'";
            SqlCommand cmd = new SqlCommand(q, conn);
            cmd.ExecuteNonQuery();
            load_details();
        }
        //Ceck-In button (for update status as Ceck-In according to to check-in time)
        private void btncheckin_Click(object sender, EventArgs e)
        {
            SqlConnection conn = GetConnection();
            string q = "UPDATE BOOKINGS_TABLE SET BOOKING_STATUS='CHECK-IN' WHERE BOOKING_ID LIKE  '" + bookingId + "'";
            SqlCommand cmd = new SqlCommand(q, conn);
            cmd.ExecuteNonQuery();
            load_details();
        }
        //Ceck-Out button (for update status as Ceck-Out according to to check-out time)
        private void btncheck_out_Click(object sender, EventArgs e)
        {
            SqlConnection conn = GetConnection();
            string q = "UPDATE BOOKINGS_TABLE SET BOOKING_STATUS='CHECK-OUT' WHERE BOOKING_ID LIKE  '" + bookingId + "'";
            SqlCommand cmd = new SqlCommand(q, conn);
            cmd.ExecuteNonQuery();
            load_details();
        }
        //Remove button (for delete booking according to payment issue)
        private void btnremove_Click(object sender, EventArgs e)
        {
            SqlConnection conn = GetConnection();
            string q = "DELETE FROM BOOKINGS_TABLE WHERE BOOKING_ID LIKE  '" + bookingId + "'";
            SqlCommand cmd = new SqlCommand(q, conn);
            cmd.ExecuteNonQuery();
            load_details();
            MessageBox.Show("Customer Removed Successfully");
        }
    }
}
