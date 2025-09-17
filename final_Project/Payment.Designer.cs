namespace final_Project
{
    partial class Payment
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panelBankTransfer = new System.Windows.Forms.Panel();
            this.txtBankTransactionId = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBankAccountNo = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBankName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panelMobilePayment = new System.Windows.Forms.Panel();
            this.btnSendOTP = new System.Windows.Forms.Button();
            this.txtOTP = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtTransactionId = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMobileNumber = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxPaymentMethod = new System.Windows.Forms.ComboBox();
            this.txtexpdate = new System.Windows.Forms.DateTimePicker();
            this.txtcode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtcardnumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnpayment = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnback = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.panelBankTransfer.SuspendLayout();
            this.panelMobilePayment.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(165)))), ((int)(((byte)(32)))));
            this.panel1.Controls.Add(this.pictureBoxLogo);
            this.panel1.Controls.Add(this.lblTotalAmount);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.panelBankTransfer);
            this.panel1.Controls.Add(this.panelMobilePayment);
            this.panel1.Controls.Add(this.comboBoxPaymentMethod);
            this.panel1.Controls.Add(this.txtexpdate);
            this.panel1.Controls.Add(this.txtcode);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtcardnumber);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(30, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(840, 520);
            this.panel1.TabIndex = 0;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Location = new System.Drawing.Point(650, 20);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(150, 80);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 15;
            this.pictureBoxLogo.TabStop = false;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Segoe Print", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmount.ForeColor = System.Drawing.Color.White;
            this.lblTotalAmount.Location = new System.Drawing.Point(450, 40);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(178, 43);
            this.lblTotalAmount.TabIndex = 14;
            this.lblTotalAmount.Text = "Total: 0 BDT";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe Print", 12F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(40, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(253, 35);
            this.label4.TabIndex = 13;
            this.label4.Text = "Select Payment Method";
            // 
            // panelBankTransfer
            // 
            this.panelBankTransfer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(222)))), ((int)(((byte)(179)))));
            this.panelBankTransfer.Controls.Add(this.txtBankTransactionId);
            this.panelBankTransfer.Controls.Add(this.label11);
            this.panelBankTransfer.Controls.Add(this.txtBankAccountNo);
            this.panelBankTransfer.Controls.Add(this.label10);
            this.panelBankTransfer.Controls.Add(this.txtBankName);
            this.panelBankTransfer.Controls.Add(this.label9);
            this.panelBankTransfer.Location = new System.Drawing.Point(45, 280);
            this.panelBankTransfer.Name = "panelBankTransfer";
            this.panelBankTransfer.Size = new System.Drawing.Size(550, 180);
            this.panelBankTransfer.TabIndex = 12;
            this.panelBankTransfer.Visible = false;
            // 
            // txtBankTransactionId
            // 
            this.txtBankTransactionId.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtBankTransactionId.Location = new System.Drawing.Point(200, 120);
            this.txtBankTransactionId.Name = "txtBankTransactionId";
            this.txtBankTransactionId.Size = new System.Drawing.Size(320, 28);
            this.txtBankTransactionId.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe Print", 11F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label11.Location = new System.Drawing.Point(20, 120);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(158, 33);
            this.label11.TabIndex = 4;
            this.label11.Text = "Transaction ID";
            // 
            // txtBankAccountNo
            // 
            this.txtBankAccountNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtBankAccountNo.Location = new System.Drawing.Point(200, 70);
            this.txtBankAccountNo.Name = "txtBankAccountNo";
            this.txtBankAccountNo.Size = new System.Drawing.Size(320, 28);
            this.txtBankAccountNo.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe Print", 11F, System.Drawing.FontStyle.Bold);
            this.label10.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label10.Location = new System.Drawing.Point(20, 70);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(180, 33);
            this.label10.TabIndex = 2;
            this.label10.Text = "Account Number";
            // 
            // txtBankName
            // 
            this.txtBankName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtBankName.Location = new System.Drawing.Point(200, 20);
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Size = new System.Drawing.Size(320, 28);
            this.txtBankName.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe Print", 11F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label9.Location = new System.Drawing.Point(20, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(127, 33);
            this.label9.TabIndex = 0;
            this.label9.Text = "Bank Name";
            // 
            // panelMobilePayment
            // 
            this.panelMobilePayment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(222)))), ((int)(((byte)(179)))));
            this.panelMobilePayment.Controls.Add(this.btnSendOTP);
            this.panelMobilePayment.Controls.Add(this.txtOTP);
            this.panelMobilePayment.Controls.Add(this.label13);
            this.panelMobilePayment.Controls.Add(this.txtPassword);
            this.panelMobilePayment.Controls.Add(this.label12);
            this.panelMobilePayment.Controls.Add(this.txtTransactionId);
            this.panelMobilePayment.Controls.Add(this.label8);
            this.panelMobilePayment.Controls.Add(this.txtMobileNumber);
            this.panelMobilePayment.Controls.Add(this.label7);
            this.panelMobilePayment.Location = new System.Drawing.Point(45, 280);
            this.panelMobilePayment.Name = "panelMobilePayment";
            this.panelMobilePayment.Size = new System.Drawing.Size(550, 220);
            this.panelMobilePayment.TabIndex = 11;
            this.panelMobilePayment.Visible = false;
            // 
            // btnSendOTP
            // 
            this.btnSendOTP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(92)))), ((int)(((byte)(122)))));
            this.btnSendOTP.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Bold);
            this.btnSendOTP.ForeColor = System.Drawing.Color.White;
            this.btnSendOTP.Location = new System.Drawing.Point(400, 20);
            this.btnSendOTP.Name = "btnSendOTP";
            this.btnSendOTP.Size = new System.Drawing.Size(120, 35);
            this.btnSendOTP.TabIndex = 8;
            this.btnSendOTP.Text = "Send OTP";
            this.btnSendOTP.UseVisualStyleBackColor = false;
            this.btnSendOTP.Click += new System.EventHandler(this.btnSendOTP_Click);
            // 
            // txtOTP
            // 
            this.txtOTP.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtOTP.Location = new System.Drawing.Point(200, 170);
            this.txtOTP.Name = "txtOTP";
            this.txtOTP.Size = new System.Drawing.Size(320, 28);
            this.txtOTP.TabIndex = 7;
            this.txtOTP.UseSystemPasswordChar = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe Print", 11F, System.Drawing.FontStyle.Bold);
            this.label13.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label13.Location = new System.Drawing.Point(20, 170);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(111, 33);
            this.label13.TabIndex = 6;
            this.label13.Text = "OTP Code";
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtPassword.Location = new System.Drawing.Point(200, 120);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(320, 28);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe Print", 11F, System.Drawing.FontStyle.Bold);
            this.label12.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label12.Location = new System.Drawing.Point(20, 120);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(106, 33);
            this.label12.TabIndex = 4;
            this.label12.Text = "Password";
            // 
            // txtTransactionId
            // 
            this.txtTransactionId.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtTransactionId.Location = new System.Drawing.Point(200, 70);
            this.txtTransactionId.Name = "txtTransactionId";
            this.txtTransactionId.Size = new System.Drawing.Size(320, 28);
            this.txtTransactionId.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe Print", 11F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label8.Location = new System.Drawing.Point(20, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(158, 33);
            this.label8.TabIndex = 2;
            this.label8.Text = "Transaction ID";
            // 
            // txtMobileNumber
            // 
            this.txtMobileNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtMobileNumber.Location = new System.Drawing.Point(200, 20);
            this.txtMobileNumber.Name = "txtMobileNumber";
            this.txtMobileNumber.Size = new System.Drawing.Size(180, 28);
            this.txtMobileNumber.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe Print", 11F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label7.Location = new System.Drawing.Point(20, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(160, 33);
            this.label7.TabIndex = 0;
            this.label7.Text = "Mobile Number";
            // 
            // comboBoxPaymentMethod
            // 
            this.comboBoxPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPaymentMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.comboBoxPaymentMethod.FormattingEnabled = true;
            this.comboBoxPaymentMethod.Items.AddRange(new object[] {
            "Credit/Debit Card",
            "bKash",
            "Rocket",
            "Nagad",
            "Bank Transfer"});
            this.comboBoxPaymentMethod.Location = new System.Drawing.Point(45, 60);
            this.comboBoxPaymentMethod.Name = "comboBoxPaymentMethod";
            this.comboBoxPaymentMethod.Size = new System.Drawing.Size(350, 33);
            this.comboBoxPaymentMethod.TabIndex = 10;
            this.comboBoxPaymentMethod.SelectedIndexChanged += new System.EventHandler(this.comboBoxPaymentMethod_SelectedIndexChanged);
            // 
            // txtexpdate
            // 
            this.txtexpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtexpdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtexpdate.Location = new System.Drawing.Point(45, 220);
            this.txtexpdate.Name = "txtexpdate";
            this.txtexpdate.Size = new System.Drawing.Size(200, 28);
            this.txtexpdate.TabIndex = 9;
            this.txtexpdate.Visible = false;
            // 
            // txtcode
            // 
            this.txtcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtcode.Location = new System.Drawing.Point(300, 220);
            this.txtcode.Name = "txtcode";
            this.txtcode.Size = new System.Drawing.Size(120, 28);
            this.txtcode.TabIndex = 8;
            this.txtcode.UseSystemPasswordChar = true;
            this.txtcode.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe Print", 11F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(300, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 33);
            this.label3.TabIndex = 7;
            this.label3.Text = "CVV Code";
            this.label3.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe Print", 11F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(40, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 33);
            this.label2.TabIndex = 6;
            this.label2.Text = "Expiration Date";
            this.label2.Visible = false;
            // 
            // txtcardnumber
            // 
            this.txtcardnumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtcardnumber.Location = new System.Drawing.Point(45, 130);
            this.txtcardnumber.Name = "txtcardnumber";
            this.txtcardnumber.Size = new System.Drawing.Size(350, 28);
            this.txtcardnumber.TabIndex = 5;
            this.txtcardnumber.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe Print", 11F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(40, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 33);
            this.label1.TabIndex = 4;
            this.label1.Text = "Card Number";
            this.label1.Visible = false;
            // 
            // btnpayment
            // 
            this.btnpayment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(92)))), ((int)(((byte)(122)))));
            this.btnpayment.Font = new System.Drawing.Font("Segoe Print", 14F, System.Drawing.FontStyle.Bold);
            this.btnpayment.ForeColor = System.Drawing.Color.White;
            this.btnpayment.Location = new System.Drawing.Point(350, 610);
            this.btnpayment.Name = "btnpayment";
            this.btnpayment.Size = new System.Drawing.Size(200, 60);
            this.btnpayment.TabIndex = 1;
            this.btnpayment.Text = "Confirm Payment";
            this.btnpayment.UseVisualStyleBackColor = false;
            this.btnpayment.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(92)))), ((int)(((byte)(122)))));
            this.button2.Font = new System.Drawing.Font("Segoe Print", 11F, System.Drawing.FontStyle.Bold);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(700, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(170, 45);
            this.button2.TabIndex = 2;
            this.button2.Text = "Print Receipt";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnback
            // 
            this.btnback.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnback.Font = new System.Drawing.Font("Segoe Print", 11F, System.Drawing.FontStyle.Bold);
            this.btnback.ForeColor = System.Drawing.Color.White;
            this.btnback.Location = new System.Drawing.Point(20, 15);
            this.btnback.Name = "btnback";
            this.btnback.Size = new System.Drawing.Size(130, 45);
            this.btnback.TabIndex = 3;
            this.btnback.Text = "Back";
            this.btnback.UseVisualStyleBackColor = false;
            this.btnback.Click += new System.EventHandler(this.btnback_Click_1);
            // 
            // Payment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 700);
            this.Controls.Add(this.btnback);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnpayment);
            this.Controls.Add(this.panel1);
            this.Name = "Payment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Payment Gateway - Luxury Hotel";
            this.Load += new System.EventHandler(this.Payment_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.panelBankTransfer.ResumeLayout(false);
            this.panelBankTransfer.PerformLayout();
            this.panelMobilePayment.ResumeLayout(false);
            this.panelMobilePayment.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelBankTransfer;
        private System.Windows.Forms.TextBox txtBankTransactionId;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtBankAccountNo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtBankName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panelMobilePayment;
        private System.Windows.Forms.Button btnSendOTP;
        private System.Windows.Forms.TextBox txtOTP;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtTransactionId;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMobileNumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxPaymentMethod;
        private System.Windows.Forms.DateTimePicker txtexpdate;
        private System.Windows.Forms.TextBox txtcode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtcardnumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnpayment;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnback;
    }
}