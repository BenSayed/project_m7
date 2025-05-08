namespace project_m7
{
    partial class delet_account
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
            this.txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnDeleteAccount = new Guna.UI2.WinForms.Guna2Button();
            this.btnDisableCard = new Guna.UI2.WinForms.Guna2Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEnableCard = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(97)))), ((int)(((byte)(109)))));
            this.txtPassword.BorderRadius = 12;
            this.txtPassword.BorderThickness = 2;
            this.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPassword.DefaultText = "";
            this.txtPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPassword.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPassword.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F);
            this.txtPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPassword.Location = new System.Drawing.Point(37, 110);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.PlaceholderText = "Password";
            this.txtPassword.SelectedText = "";
            this.txtPassword.Size = new System.Drawing.Size(452, 67);
            this.txtPassword.TabIndex = 0;
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnDeleteAccount
            // 
            this.btnDeleteAccount.BorderRadius = 12;
            this.btnDeleteAccount.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDeleteAccount.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDeleteAccount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDeleteAccount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDeleteAccount.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(97)))), ((int)(((byte)(109)))));
            this.btnDeleteAccount.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F);
            this.btnDeleteAccount.ForeColor = System.Drawing.Color.White;
            this.btnDeleteAccount.Location = new System.Drawing.Point(15, 232);
            this.btnDeleteAccount.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnDeleteAccount.Name = "btnDeleteAccount";
            this.btnDeleteAccount.Size = new System.Drawing.Size(214, 70);
            this.btnDeleteAccount.TabIndex = 1;
            this.btnDeleteAccount.Text = "Delete Account";
            this.btnDeleteAccount.Click += new System.EventHandler(this.btnDeleteAccount_Click);
            // 
            // btnDisableCard
            // 
            this.btnDisableCard.BorderRadius = 12;
            this.btnDisableCard.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDisableCard.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDisableCard.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDisableCard.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDisableCard.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(97)))), ((int)(((byte)(109)))));
            this.btnDisableCard.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F);
            this.btnDisableCard.ForeColor = System.Drawing.Color.White;
            this.btnDisableCard.Location = new System.Drawing.Point(234, 232);
            this.btnDisableCard.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnDisableCard.Name = "btnDisableCard";
            this.btnDisableCard.Size = new System.Drawing.Size(255, 70);
            this.btnDisableCard.TabIndex = 2;
            this.btnDisableCard.Text = "on/off Card";
            this.btnDisableCard.Click += new System.EventHandler(this.btnDisableCard_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(97)))), ((int)(((byte)(109)))));
            this.label1.Location = new System.Drawing.Point(113, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(376, 50);
            this.label1.TabIndex = 3;
            this.label1.Text = "Manage Account";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEnableCard
            // 
            this.btnEnableCard.BorderRadius = 5;
            this.btnEnableCard.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEnableCard.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEnableCard.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEnableCard.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEnableCard.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnEnableCard.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEnableCard.ForeColor = System.Drawing.Color.White;
            this.btnEnableCard.Location = new System.Drawing.Point(12, 150);
            this.btnEnableCard.Name = "btnEnableCard";
            this.btnEnableCard.Size = new System.Drawing.Size(200, 45);
            this.btnEnableCard.TabIndex = 2;
            this.btnEnableCard.Text = "تفعيل البطاقة";
            this.btnEnableCard.Click += new System.EventHandler(this.btnEnableCard_Click);
            // 
            // delet_account
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(513, 349);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDisableCard);
            this.Controls.Add(this.btnDeleteAccount);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnEnableCard);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "delet_account";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إدارة الحساب";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox txtPassword;
        private Guna.UI2.WinForms.Guna2Button btnDeleteAccount;
        private Guna.UI2.WinForms.Guna2Button btnDisableCard;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button btnEnableCard;
    }
}