namespace final_Project
{
    partial class Rooms
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.txtsearch = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelCheckOut = new System.Windows.Forms.Label();
            this.labelCheckIn = new System.Windows.Forms.Label();
            this.btnCheckAvailability = new System.Windows.Forms.Button();
            this.dateTimePickerCheckOut = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerCheckIn = new System.Windows.Forms.DateTimePicker();
            this.txtuser = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(207)))), ((int)(((byte)(206)))));
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 64);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1431, 662);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Segoe Print", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Available",
            "Booked",
            "Alphabetically",
            "Low to High",
            "High to Low"});
            this.comboBox1.Location = new System.Drawing.Point(113, 14);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(172, 39);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.Text = "Default sorting";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // txtsearch
            // 
            this.txtsearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtsearch.Font = new System.Drawing.Font("Segoe Print", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsearch.Location = new System.Drawing.Point(1110, 10);
            this.txtsearch.Name = "txtsearch";
            this.txtsearch.Size = new System.Drawing.Size(116, 40);
            this.txtsearch.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(92)))), ((int)(((byte)(122)))));
            this.button1.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button1.Location = new System.Drawing.Point(1232, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 40);
            this.button1.TabIndex = 3;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.panel1.Controls.Add(this.labelCheckOut);
            this.panel1.Controls.Add(this.labelCheckIn);
            this.panel1.Controls.Add(this.btnCheckAvailability);
            this.panel1.Controls.Add(this.dateTimePickerCheckOut);
            this.panel1.Controls.Add(this.dateTimePickerCheckIn);
            this.panel1.Controls.Add(this.txtuser);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.txtsearch);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1431, 64);
            this.panel1.TabIndex = 1;
            // 
            // labelCheckOut
            // 
            this.labelCheckOut.AutoSize = true;
            this.labelCheckOut.Font = new System.Drawing.Font("Segoe Print", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCheckOut.Location = new System.Drawing.Point(644, 22);
            this.labelCheckOut.Name = "labelCheckOut";
            this.labelCheckOut.Size = new System.Drawing.Size(38, 23);
            this.labelCheckOut.TabIndex = 10;
            this.labelCheckOut.Text = "Out:";
            // 
            // labelCheckIn
            // 
            this.labelCheckIn.AutoSize = true;
            this.labelCheckIn.Font = new System.Drawing.Font("Segoe Print", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCheckIn.Location = new System.Drawing.Point(277, 23);
            this.labelCheckIn.Name = "labelCheckIn";
            this.labelCheckIn.Size = new System.Drawing.Size(74, 23);
            this.labelCheckIn.TabIndex = 9;
            this.labelCheckIn.Text = "Check-In:";
            // 
            // btnCheckAvailability
            // 
            this.btnCheckAvailability.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(92)))), ((int)(((byte)(122)))));
            this.btnCheckAvailability.Font = new System.Drawing.Font("Segoe Print", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckAvailability.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnCheckAvailability.Location = new System.Drawing.Point(978, 16);
            this.btnCheckAvailability.Name = "btnCheckAvailability";
            this.btnCheckAvailability.Size = new System.Drawing.Size(120, 30);
            this.btnCheckAvailability.TabIndex = 8;
            this.btnCheckAvailability.Text = "Check Dates";
            this.btnCheckAvailability.UseVisualStyleBackColor = false;
            this.btnCheckAvailability.Click += new System.EventHandler(this.BtnCheckAvailability_Click);
            // 
            // dateTimePickerCheckOut
            // 
            this.dateTimePickerCheckOut.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerCheckOut.Location = new System.Drawing.Point(688, 15);
            this.dateTimePickerCheckOut.Name = "dateTimePickerCheckOut";
            this.dateTimePickerCheckOut.Size = new System.Drawing.Size(284, 34);
            this.dateTimePickerCheckOut.TabIndex = 7;
            this.dateTimePickerCheckOut.Value = new System.DateTime(2025, 1, 20, 0, 0, 0, 0);
            // 
            // dateTimePickerCheckIn
            // 
            this.dateTimePickerCheckIn.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerCheckIn.Location = new System.Drawing.Point(357, 16);
            this.dateTimePickerCheckIn.Name = "dateTimePickerCheckIn";
            this.dateTimePickerCheckIn.Size = new System.Drawing.Size(289, 34);
            this.dateTimePickerCheckIn.TabIndex = 6;
            this.dateTimePickerCheckIn.Value = new System.DateTime(2025, 1, 18, 0, 0, 0, 0);
            // 
            // txtuser
            // 
            this.txtuser.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtuser.BackColor = System.Drawing.Color.Transparent;
            this.txtuser.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtuser.ForeColor = System.Drawing.Color.Black;
            this.txtuser.Location = new System.Drawing.Point(1314, 10);
            this.txtuser.Name = "txtuser";
            this.txtuser.Size = new System.Drawing.Size(108, 40);
            this.txtuser.TabIndex = 5;
            this.txtuser.Text = "User";
            this.txtuser.UseVisualStyleBackColor = false;
            this.txtuser.Click += new System.EventHandler(this.Button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(92)))), ((int)(((byte)(122)))));
            this.button2.Font = new System.Drawing.Font("Segoe Print", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button2.Location = new System.Drawing.Point(12, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 44);
            this.button2.TabIndex = 4;
            this.button2.Text = "Back";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Rooms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Linen;
            this.ClientSize = new System.Drawing.Size(1431, 726);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "Rooms";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rooms";
            this.Load += new System.EventHandler(this.Rooms_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox txtsearch;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button txtuser;
        private System.Windows.Forms.DateTimePicker dateTimePickerCheckIn;
        private System.Windows.Forms.Label labelCheckOut;
        private System.Windows.Forms.Label labelCheckIn;
        private System.Windows.Forms.Button btnCheckAvailability;
        private System.Windows.Forms.DateTimePicker dateTimePickerCheckOut;
    }
}