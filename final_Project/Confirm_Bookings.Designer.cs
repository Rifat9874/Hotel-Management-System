namespace final_Project
{
    partial class Confirm_Bookings
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
            this.checkInDate = new System.Windows.Forms.DateTimePicker();
            this.checkOutDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtuser = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkInDate
            // 
            this.checkInDate.CalendarMonthBackground = System.Drawing.SystemColors.ActiveCaption;
            this.checkInDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkInDate.Location = new System.Drawing.Point(54, 183);
            this.checkInDate.Name = "checkInDate";
            this.checkInDate.Size = new System.Drawing.Size(355, 30);
            this.checkInDate.TabIndex = 0;
            // 
            // checkOutDate
            // 
            this.checkOutDate.CalendarMonthBackground = System.Drawing.SystemColors.ActiveCaption;
            this.checkOutDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkOutDate.Location = new System.Drawing.Point(468, 183);
            this.checkOutDate.Name = "checkOutDate";
            this.checkOutDate.Size = new System.Drawing.Size(353, 30);
            this.checkOutDate.TabIndex = 1;
            this.checkOutDate.Value = new System.DateTime(2025, 7, 8, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe Print", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(156, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 43);
            this.label1.TabIndex = 2;
            this.label1.Text = "Check In";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe Print", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(565, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 43);
            this.label2.TabIndex = 3;
            this.label2.Text = "Check Out";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe Print", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(12, 495);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(420, 44);
            this.label3.TabIndex = 4;
            this.label3.Text = "Current Bookings Of This Room";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 565);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(866, 256);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(92)))), ((int)(((byte)(122)))));
            this.button1.Font = new System.Drawing.Font("Segoe Print", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button1.Location = new System.Drawing.Point(18, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 43);
            this.button1.TabIndex = 6;
            this.button1.Text = "Back";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(92)))), ((int)(((byte)(122)))));
            this.button2.Font = new System.Drawing.Font("Segoe Print", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button2.Location = new System.Drawing.Point(310, 263);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(242, 49);
            this.button2.TabIndex = 7;
            this.button2.Text = "Confirm Booking";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtuser
            // 
            this.txtuser.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtuser.BackColor = System.Drawing.Color.Transparent;
            this.txtuser.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtuser.ForeColor = System.Drawing.Color.Black;
            this.txtuser.Location = new System.Drawing.Point(519, 22);
            this.txtuser.Name = "txtuser";
            this.txtuser.Size = new System.Drawing.Size(335, 40);
            this.txtuser.TabIndex = 22;
            this.txtuser.Text = "User";
            this.txtuser.UseVisualStyleBackColor = false;
            this.txtuser.Click += new System.EventHandler(this.txtuser_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe Print", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DarkRed;
            this.label4.Location = new System.Drawing.Point(141, 333);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(542, 30);
            this.label4.TabIndex = 23;
            this.label4.Text = "After Confirming Booking You Have To Pay Within 24 Hours";
            // 
            // Confirm_Bookings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(153)))), ((int)(((byte)(163)))));
            this.ClientSize = new System.Drawing.Size(866, 821);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtuser);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkOutDate);
            this.Controls.Add(this.checkInDate);
            this.Name = "Confirm_Bookings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.Load += new System.EventHandler(this.Confirm_Bookings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker checkInDate;
        private System.Windows.Forms.DateTimePicker checkOutDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button txtuser;
        private System.Windows.Forms.Label label4;
    }
}