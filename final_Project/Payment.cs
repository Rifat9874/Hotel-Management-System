using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;

namespace final_Project
{
    public partial class Payment : Form
    {
        // FIXED: Made appropriate fields readonly
        private readonly string bookingId;
        private readonly string checkIn;
        private readonly string checkOut;
        private readonly string email;
        private string roomId; // This can't be readonly since it's assigned in find_room()
        private int totalBill;
        private string selectedPaymentMethod = "";

        public Payment()
        {
            InitializeComponent();
        }

        public Payment(string roomId, string email, string bookingId, string checkIn, string checkOut)
        {
            this.email = email;
            this.bookingId = bookingId;
            this.roomId = roomId;
            this.checkIn = checkIn;
            this.checkOut = checkOut;
            InitializeComponent();
        }

        SqlConnection GetConnection()
        {
            string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HMSDB1;Integrated Security=True;TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }

        private void Payment_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(roomId))
            {
                find_room();
            }

            totalBill = total_Bill(roomId);
            lblTotalAmount.Text = $"Total: {totalBill} BDT";

            panelMobilePayment.Visible = false;
            panelBankTransfer.Visible = false;
            HideCardFields();
            pictureBoxLogo.Visible = false;
        }

        private void HideCardFields()
        {
            txtcardnumber.Visible = false;
            txtexpdate.Visible = false;
            txtcode.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
        }

        private void ShowCardFields()
        {
            txtcardnumber.Visible = true;
            txtexpdate.Visible = true;
            txtcode.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
        }

        void change_status()
        {
            using (SqlConnection sqlConnection = GetConnection())
            {
                string q = "UPDATE BOOKINGS_TABLE SET BOOKING_STATUS='PAID' WHERE BOOKING_ID = @BookingId";
                SqlCommand cmd = new SqlCommand(q, sqlConnection);
                cmd.Parameters.AddWithValue("@BookingId", bookingId);
                cmd.ExecuteNonQuery();
            }
        }

        void find_room()
        {
            using (SqlConnection sqlConnection = GetConnection())
            {
                string q = "SELECT ROOM_ID FROM BOOKINGS_TABLE WHERE BOOKING_ID = @BookingId";
                SqlCommand cmd = new SqlCommand(q, sqlConnection);
                cmd.Parameters.AddWithValue("@BookingId", bookingId);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    roomId = rdr["ROOM_ID"].ToString();
                }
                rdr.Close();
            }
        }

        void payment()
        {
            int bill = total_Bill(roomId);
            using (SqlConnection sqlConnection = GetConnection())
            {
                string Id = " ";
                string IdQuerey = "SELECT ISNULL(MAX(P_ID), 0) + 1 FROM PAYMENT";
                SqlCommand cmd1 = new SqlCommand(IdQuerey, sqlConnection);
                Id = ((int)cmd1.ExecuteScalar()).ToString();

                string paymentMethod = comboBoxPaymentMethod.SelectedItem.ToString();

                if (paymentMethod == "Credit/Debit Card")
                {
                    string q = @"INSERT INTO PAYMENT (P_ID, BOOKING_ID, CARD_NUMBER, EXP_DATE, CODE, BILL) 
                         VALUES (@P_ID, @BOOKING_ID, @CARD_NUMBER, @EXP_DATE, @CODE, @BILL)";

                    SqlCommand cmd = new SqlCommand(q, sqlConnection);
                    cmd.Parameters.AddWithValue("@P_ID", Id);
                    cmd.Parameters.AddWithValue("@BOOKING_ID", bookingId);
                    cmd.Parameters.AddWithValue("@CARD_NUMBER", txtcardnumber.Text);
                    cmd.Parameters.AddWithValue("@EXP_DATE", txtexpdate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@CODE", txtcode.Text);
                    cmd.Parameters.AddWithValue("@BILL", bill);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string paymentDetails = "";
                    string transactionId = "";

                    switch (paymentMethod)
                    {
                        case "bKash":
                            paymentDetails = $"bKash: {txtMobileNumber.Text}";
                            transactionId = $"{txtTransactionId.Text} (Pass: {txtPassword.Text}, OTP: {txtOTP.Text})";
                            break;
                        case "Rocket":
                            paymentDetails = $"Rocket: {txtMobileNumber.Text}";
                            transactionId = $"{txtTransactionId.Text} (Pass: {txtPassword.Text}, OTP: {txtOTP.Text})";
                            break;
                        case "Nagad":
                            paymentDetails = $"Nagad: {txtMobileNumber.Text}";
                            transactionId = $"{txtTransactionId.Text} (Pass: {txtPassword.Text}, OTP: {txtOTP.Text})";
                            break;
                        case "Bank Transfer":
                            paymentDetails = $"Bank: {txtBankName.Text}, Acc: {txtBankAccountNo.Text}";
                            transactionId = txtBankTransactionId.Text;
                            break;
                    }

                    string q = @"INSERT INTO PAYMENT (P_ID, BOOKING_ID, CARD_NUMBER, EXP_DATE, CODE, BILL) 
                         VALUES (@P_ID, @BOOKING_ID, @CARD_NUMBER, @EXP_DATE, @CODE, @BILL)";

                    SqlCommand cmd = new SqlCommand(q, sqlConnection);
                    cmd.Parameters.AddWithValue("@P_ID", Id);
                    cmd.Parameters.AddWithValue("@BOOKING_ID", bookingId);
                    cmd.Parameters.AddWithValue("@CARD_NUMBER", paymentDetails);
                    cmd.Parameters.AddWithValue("@EXP_DATE", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CODE", transactionId);
                    cmd.Parameters.AddWithValue("@BILL", bill);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void BtnBack_Click(object sender, EventArgs e) // FIXED: Changed btnback_Click to BtnBack_Click
        {
            Confirm_Bookings confirm_Bookings = new Confirm_Bookings(roomId, email);
            confirm_Bookings.Show();
            this.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerateReceipt();
        }

        private void GenerateReceipt()
        {
            try
            {
                // Ask user where to save the file
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                saveFileDialog.FileName = $"Hotel_Receipt_{bookingId}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                saveFileDialog.Title = "Save Receipt As";

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return; // User cancelled
                }

                string filePath = saveFileDialog.FileName;

                // Create PDF document
                Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                // Add styling
                BaseColor primaryColor = new BaseColor(218, 165, 32); // Gold color
                BaseColor secondaryColor = new BaseColor(43, 92, 122); // Dark blue
                BaseColor lightGray = new BaseColor(240, 240, 240);

                // Create header table
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                headerTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER; // FIXED: Added namespace

                // Hotel header
                PdfPCell headerCell = new PdfPCell(new Phrase("RIFAT  GRAND HOTEL ",
                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 20, iTextSharp.text.Font.BOLD, primaryColor)));
                headerCell.HorizontalAlignment = Element.ALIGN_LEFT;
                headerCell.Border = iTextSharp.text.Rectangle.NO_BORDER; // FIXED: Added namespace
                headerCell.PaddingBottom = 10f;
                headerTable.AddCell(headerCell);

                // Receipt title
                PdfPCell receiptTitleCell = new PdfPCell(new Phrase("PAYMENT RECEIPT",
                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD, secondaryColor)));
                receiptTitleCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                receiptTitleCell.Border = iTextSharp.text.Rectangle.NO_BORDER; // FIXED: Added namespace
                receiptTitleCell.PaddingBottom = 10f;
                headerTable.AddCell(receiptTitleCell);

                document.Add(headerTable);

                // Add separator
                document.Add(new Chunk("\n"));
                PdfPTable separatorTable = new PdfPTable(1);
                separatorTable.WidthPercentage = 100;
                PdfPCell separatorCell = new PdfPCell();
                separatorCell.Border = iTextSharp.text.Rectangle.NO_BORDER; // FIXED: Added namespace
                separatorCell.FixedHeight = 2f;
                separatorCell.BackgroundColor = primaryColor;
                separatorTable.AddCell(separatorCell);
                document.Add(separatorTable);
                document.Add(new Chunk("\n"));

                // Customer information table
                PdfPTable infoTable = new PdfPTable(2);
                infoTable.WidthPercentage = 100;
                infoTable.SetWidths(new float[] { 30, 70 });
                infoTable.SpacingBefore = 10f;
                infoTable.SpacingAfter = 10f;

                AddInfoRow(infoTable, "Receipt Number:", bookingId, secondaryColor);
                AddInfoRow(infoTable, "Issue Date:", DateTime.Now.ToString("dd MMMM yyyy HH:mm"), secondaryColor);
                AddInfoRow(infoTable, "Guest Email:", email, secondaryColor);
                AddInfoRow(infoTable, "Room Number:", roomId, secondaryColor);
                AddInfoRow(infoTable, "Check-in Date:", checkIn, secondaryColor);
                AddInfoRow(infoTable, "Check-out Date:", checkOut, secondaryColor);

                document.Add(infoTable);

                // Payment details section
                PdfPTable paymentHeaderTable = new PdfPTable(1);
                paymentHeaderTable.WidthPercentage = 100;
                PdfPCell paymentHeaderCell = new PdfPCell(new Phrase("PAYMENT DETAILS",
                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, BaseColor.WHITE)));
                paymentHeaderCell.BackgroundColor = secondaryColor;
                paymentHeaderCell.HorizontalAlignment = Element.ALIGN_CENTER;
                paymentHeaderCell.Padding = 8f;
                paymentHeaderTable.AddCell(paymentHeaderCell);
                paymentHeaderTable.SpacingBefore = 15f;
                paymentHeaderTable.SpacingAfter = 10f;
                document.Add(paymentHeaderTable);

                // Payment details table
                PdfPTable paymentTable = new PdfPTable(2);
                paymentTable.WidthPercentage = 100;
                paymentTable.SetWidths(new float[] { 40, 60 });

                AddPaymentRow(paymentTable, "Total Amount:", $"{totalBill} BDT", primaryColor);
                AddPaymentRow(paymentTable, "Payment Method:", comboBoxPaymentMethod.SelectedItem?.ToString() ?? "N/A", primaryColor);
                AddPaymentRow(paymentTable, "Payment Status:", "PAID", primaryColor);
                AddPaymentRow(paymentTable, "Transaction Time:", DateTime.Now.ToString("HH:mm:ss"), primaryColor);

                document.Add(paymentTable);

                // Total amount highlight
                PdfPTable totalTable = new PdfPTable(1);
                totalTable.WidthPercentage = 100;
                totalTable.SpacingBefore = 15f;

                PdfPCell totalCell = new PdfPCell(new Phrase($"TOTAL PAID: {totalBill} BDT",
                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD, BaseColor.WHITE)));
                totalCell.BackgroundColor = primaryColor;
                totalCell.HorizontalAlignment = Element.ALIGN_CENTER;
                totalCell.Padding = 12f;
                totalTable.AddCell(totalCell);
                document.Add(totalTable);

                // Thank you message
                Paragraph thankYou = new Paragraph("\nThank you for choosing Luxury Hotel & Resort!\n" +
                    "We look forward to serving you again.\n\n",
                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.ITALIC, BaseColor.DARK_GRAY));
                thankYou.Alignment = Element.ALIGN_CENTER;
                document.Add(thankYou);

                // Contact information
                Paragraph contactInfo = new Paragraph("For any inquiries, please contact:01993182070\n" +
                    "📞 Front Desk: +880-01705510384\n" +
                    "📧 Email: info@luxuryhotel.com\n" +
                    "🏨 Address: Marine Drive Road kolatoli,Cox's Bazar Bangladesh",
                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.GRAY));
                contactInfo.Alignment = Element.ALIGN_CENTER;
                document.Add(contactInfo);

                document.Close();

                MessageBox.Show($"Receipt generated successfully!\nSaved to: {filePath}", "Receipt Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Ask if user wants to open the file
                DialogResult result = MessageBox.Show("Do you want to open the receipt now?", "Open Receipt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating receipt: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method for info rows
        private void AddInfoRow(PdfPTable table, string label, string value, BaseColor color)
        {
            PdfPCell labelCell = new PdfPCell(new Phrase(label,
                new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, color)));
            labelCell.Border = iTextSharp.text.Rectangle.NO_BORDER; // FIXED: Added namespace
            labelCell.Padding = 5f;
            table.AddCell(labelCell);

            PdfPCell valueCell = new PdfPCell(new Phrase(value,
                new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            valueCell.Border = iTextSharp.text.Rectangle.NO_BORDER; // FIXED: Added namespace
            valueCell.Padding = 5f;
            table.AddCell(valueCell);
        }

        // Helper method for payment rows
        private void AddPaymentRow(PdfPTable table, string label, string value, BaseColor color)
        {
            PdfPCell labelCell = new PdfPCell(new Phrase(label,
                new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.BOLD, color)));
            labelCell.Border = iTextSharp.text.Rectangle.NO_BORDER; // FIXED: Added namespace
            labelCell.Padding = 6f;
            table.AddCell(labelCell);

            PdfPCell valueCell = new PdfPCell(new Phrase(value,
                new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            valueCell.Border = iTextSharp.text.Rectangle.NO_BORDER; // FIXED: Added namespace
            valueCell.Padding = 6f;
            table.AddCell(valueCell);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidatePayment())
            {
                return;
            }

            if (comboBoxPaymentMethod.SelectedItem.ToString() == "bKash" ||
                comboBoxPaymentMethod.SelectedItem.ToString() == "Rocket" ||
                comboBoxPaymentMethod.SelectedItem.ToString() == "Nagad")
            {
                if (!VerifyOTP())
                {
                    MessageBox.Show("OTP verification failed. Please check your OTP code.", "Verification Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            payment();
            change_status();
            MessageBox.Show("Payment successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool VerifyOTP()
        {
            if (txtOTP.Text.Length == 6 && Regex.IsMatch(txtOTP.Text, @"^\d{6}$"))
            {
                return true;
            }
            MessageBox.Show("Please enter a valid 6-digit OTP code.", "OTP Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private bool ValidatePayment()
        {
            if (comboBoxPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("Please select a payment method.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string paymentMethod = comboBoxPaymentMethod.SelectedItem.ToString();

            switch (paymentMethod)
            {
                case "Credit/Debit Card":
                    if (!ValidateCardPayment()) return false;
                    break;
                case "bKash":
                case "Rocket":
                case "Nagad":
                    if (!ValidateMobilePayment()) return false;
                    break;
                case "Bank Transfer":
                    if (!ValidateBankPayment()) return false;
                    break;
            }

            return true;
        }

        private bool ValidateCardPayment()
        {
            if (!Regex.IsMatch(txtcardnumber.Text, @"^\d{16}$"))
            {
                MessageBox.Show("Please enter a valid 16-digit card number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtexpdate.Value < DateTime.Now)
            {
                MessageBox.Show("Card expiration date cannot be in the past.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(txtcode.Text, @"^\d{3,4}$"))
            {
                MessageBox.Show("Please enter a valid CVV (3 or 4 digits).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool ValidateMobilePayment()
        {
            if (!Regex.IsMatch(txtMobileNumber.Text, @"^01[3-9]\d{8}$"))
            {
                MessageBox.Show("Please enter a valid mobile number (11 digits starting with 013-019).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTransactionId.Text))
            {
                MessageBox.Show("Please enter your transaction ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter your mobile banking password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(txtOTP.Text, @"^\d{6}$"))
            {
                MessageBox.Show("Please enter a valid 6-digit OTP code.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool ValidateBankPayment()
        {
            if (string.IsNullOrWhiteSpace(txtBankName.Text))
            {
                MessageBox.Show("Please enter bank name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtBankAccountNo.Text))
            {
                MessageBox.Show("Please enter bank account number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtBankTransactionId.Text))
            {
                MessageBox.Show("Please enter bank transaction ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void comboBoxPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPaymentMethod.SelectedItem == null) return;

            selectedPaymentMethod = comboBoxPaymentMethod.SelectedItem.ToString();

            panelMobilePayment.Visible = false;
            panelBankTransfer.Visible = false;
            HideCardFields();

            switch (selectedPaymentMethod)
            {
                case "Credit/Debit Card":
                    ShowCardFields();
                    break;
                case "bKash":
                case "Rocket":
                case "Nagad":
                    panelMobilePayment.Visible = true;
                    break;
                case "Bank Transfer":
                    panelBankTransfer.Visible = true;
                    break;
            }
        }

        private void btnSendOTP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMobileNumber.Text) || !Regex.IsMatch(txtMobileNumber.Text, @"^01[3-9]\d{8}$"))
            {
                MessageBox.Show("Please enter a valid mobile number first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GenerateOTP();
        }

        private void GenerateOTP()
        {
            Random random = new Random();
            txtOTP.Text = random.Next(100000, 999999).ToString();
            MessageBox.Show($"OTP sent to {txtMobileNumber.Text}: {txtOTP.Text}\n(This is a simulation)", "OTP Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        int total_Bill(string roomId)
        {
            int bill = 0;
            using (SqlConnection sqlConnection = GetConnection())
            {
                string q = "SELECT PPN FROM ROOM_TABLE WHERE ROOM_ID = @RoomId";
                SqlCommand cmd = new SqlCommand(q, sqlConnection);
                cmd.Parameters.AddWithValue("@RoomId", roomId);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    if (rdr["PPN"] != DBNull.Value)
                    {
                        bill = Convert.ToInt32(rdr["PPN"]);
                    }
                }
                rdr.Close();
            }

            DateTime checkInDate = DateTime.Parse(checkIn);
            DateTime checkOutDate = DateTime.Parse(checkOut);
            int numberOfDays = (checkOutDate - checkInDate).Days;

            return bill * Math.Max(1, numberOfDays);
        }

        private int CalculateStayDuration()
        {
            try
            {
                DateTime checkInDate = DateTime.Parse(checkIn);
                DateTime checkOutDate = DateTime.Parse(checkOut);
                return (checkOutDate - checkInDate).Days;
            }
            catch
            {
                return 1;
            }
        }

        private void btnback_Click_1(object sender, EventArgs e)
        {
            Confirm_Bookings confirm_Bookings = new Confirm_Bookings(roomId, email);
            confirm_Bookings.Show();
            this.Hide();
        }
    }
}
