namespace final_Project
{
    partial class Facility_Booking
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblFacilityName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpBookingDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.numGuests = new System.Windows.Forms.NumericUpDown();
            this.lblGuests = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnBook = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numGuests)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFacilityName
            // 
            this.lblFacilityName.AutoSize = true;
            this.lblFacilityName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblFacilityName.Location = new System.Drawing.Point(12, 9);
            this.lblFacilityName.Name = "lblFacilityName";
            this.lblFacilityName.Size = new System.Drawing.Size(158, 32);
            this.lblFacilityName.TabIndex = 0;
            this.lblFacilityName.Text = "Facility Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label2.Location = new System.Drawing.Point(14, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Booking Date:";
            // 
            // dtpBookingDate
            // 
            this.dtpBookingDate.Location = new System.Drawing.Point(128, 60);
            this.dtpBookingDate.Name = "dtpBookingDate";
            this.dtpBookingDate.Size = new System.Drawing.Size(250, 22);
            this.dtpBookingDate.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label3.Location = new System.Drawing.Point(14, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Price:";
            // 
            // numGuests
            // 
            this.numGuests.Location = new System.Drawing.Point(128, 140);
            this.numGuests.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGuests.Name = "numGuests";
            this.numGuests.Size = new System.Drawing.Size(120, 22);
            this.numGuests.TabIndex = 4;
            this.numGuests.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGuests.ValueChanged += new System.EventHandler(this.numGuests_ValueChanged);
            // 
            // lblGuests
            // 
            this.lblGuests.AutoSize = true;
            this.lblGuests.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblGuests.Location = new System.Drawing.Point(14, 140);
            this.lblGuests.Name = "lblGuests";
            this.lblGuests.Size = new System.Drawing.Size(65, 23);
            this.lblGuests.TabIndex = 5;
            this.lblGuests.Text = "Guests:";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPrice.Location = new System.Drawing.Point(124, 100);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(53, 23);
            this.lblPrice.TabIndex = 6;
            this.lblPrice.Text = "Price:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(14, 180);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(71, 28);
            this.lblTotal.TabIndex = 7;
            this.lblTotal.Text = "Total: ";
            // 
            // btnBook
            // 
            this.btnBook.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(161)))), ((int)(((byte)(188)))));
            this.btnBook.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBook.Location = new System.Drawing.Point(200, 180);
            this.btnBook.Name = "btnBook";
            this.btnBook.Size = new System.Drawing.Size(120, 35);
            this.btnBook.TabIndex = 8;
            this.btnBook.Text = "Book Now";
            this.btnBook.UseVisualStyleBackColor = false;
            this.btnBook.Click += new System.EventHandler(this.btnBook_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(161)))), ((int)(((byte)(188)))));
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCancel.Location = new System.Drawing.Point(326, 180);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 35);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Facility_Booking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(432, 233);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBook);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblGuests);
            this.Controls.Add(this.numGuests);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpBookingDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblFacilityName);
            this.Name = "Facility_Booking";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Book Facility";
            this.Load += new System.EventHandler(this.Facility_Booking_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numGuests)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFacilityName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpBookingDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numGuests;
        private System.Windows.Forms.Label lblGuests;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnBook;
        private System.Windows.Forms.Button btnCancel;
    }
}