namespace final_Project
{
    partial class QuickCheckOutForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox cmbBookings;
        private System.Windows.Forms.Button btnCheckOut;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbBookings = new System.Windows.Forms.ComboBox();
            this.btnCheckOut = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // cmbBookings
            this.cmbBookings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBookings.FormattingEnabled = true;
            this.cmbBookings.Location = new System.Drawing.Point(30, 50);
            this.cmbBookings.Size = new System.Drawing.Size(300, 24);

            // btnCheckOut
            this.btnCheckOut.Location = new System.Drawing.Point(30, 90);
            this.btnCheckOut.Size = new System.Drawing.Size(140, 35);
            this.btnCheckOut.Text = "Check Out";
            this.btnCheckOut.Click += new System.EventHandler(this.btnCheckOut_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(190, 90);
            this.btnCancel.Size = new System.Drawing.Size(140, 35);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 20);
            this.label1.Text = "Select Booking to Check Out:";

            // QuickCheckOutForm
            this.ClientSize = new System.Drawing.Size(360, 150);
            this.Controls.Add(this.cmbBookings);
            this.Controls.Add(this.btnCheckOut);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Text = "Quick Check-Out";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}