namespace project_m7
{
    partial class Deposit
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
            this.cardNumberTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.passwordTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.amountTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.depositButton = new Guna.UI2.WinForms.Guna2Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.cardNumberLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.amountLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cardNumberTextBox
            // 
            this.cardNumberTextBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(97)))), ((int)(((byte)(109)))));
            this.cardNumberTextBox.BorderRadius = 12;
            this.cardNumberTextBox.BorderThickness = 2;
            this.cardNumberTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cardNumberTextBox.DefaultText = "";
            this.cardNumberTextBox.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cardNumberTextBox.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F);
            this.cardNumberTextBox.Location = new System.Drawing.Point(170, 80);
            this.cardNumberTextBox.Name = "cardNumberTextBox";
            this.cardNumberTextBox.PasswordChar = '\0';
            this.cardNumberTextBox.PlaceholderText = "Enter Card Number";
            this.cardNumberTextBox.SelectedText = "";
            this.cardNumberTextBox.Size = new System.Drawing.Size(300, 40);
            this.cardNumberTextBox.TabIndex = 0;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(97)))), ((int)(((byte)(109)))));
            this.passwordTextBox.BorderRadius = 12;
            this.passwordTextBox.BorderThickness = 2;
            this.passwordTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.passwordTextBox.DefaultText = "";
            this.passwordTextBox.FillColor = System.Drawing.Color.WhiteSmoke;
            this.passwordTextBox.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F);
            this.passwordTextBox.Location = new System.Drawing.Point(170, 140);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '●';
            this.passwordTextBox.PlaceholderText = "Enter Password";
            this.passwordTextBox.SelectedText = "";
            this.passwordTextBox.Size = new System.Drawing.Size(300, 40);
            this.passwordTextBox.TabIndex = 1;
            // 
            // amountTextBox
            // 
            this.amountTextBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(97)))), ((int)(((byte)(109)))));
            this.amountTextBox.BorderRadius = 12;
            this.amountTextBox.BorderThickness = 2;
            this.amountTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.amountTextBox.DefaultText = "";
            this.amountTextBox.FillColor = System.Drawing.Color.WhiteSmoke;
            this.amountTextBox.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F);
            this.amountTextBox.Location = new System.Drawing.Point(170, 200);
            this.amountTextBox.Name = "amountTextBox";
            this.amountTextBox.PlaceholderText = "Enter Amount";
            this.amountTextBox.SelectedText = "";
            this.amountTextBox.Size = new System.Drawing.Size(300, 40);
            this.amountTextBox.TabIndex = 2;
            // 
            // depositButton
            // 
            this.depositButton.BorderRadius = 12;
            this.depositButton.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(97)))), ((int)(((byte)(109)))));
            this.depositButton.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11F);
            this.depositButton.ForeColor = System.Drawing.Color.White;
            this.depositButton.Location = new System.Drawing.Point(220, 260);
            this.depositButton.Name = "depositButton";
            this.depositButton.Size = new System.Drawing.Size(200, 50);
            this.depositButton.TabIndex = 3;
            this.depositButton.Text = "Deposit";
            this.depositButton.Click += new System.EventHandler(this.DepositButton_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Arial Rounded MT Bold", 16F);
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(97)))), ((int)(((byte)(109)))));
            this.titleLabel.Location = new System.Drawing.Point(180, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(280, 40);
            this.titleLabel.TabIndex = 4;
            this.titleLabel.Text = "Deposit Form";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardNumberLabel
            // 
            this.cardNumberLabel.AutoSize = true;
            this.cardNumberLabel.Font = new System.Drawing.Font("Arial", 10F);
            this.cardNumberLabel.Location = new System.Drawing.Point(40, 90);
            this.cardNumberLabel.Name = "cardNumberLabel";
            this.cardNumberLabel.Size = new System.Drawing.Size(130, 23);
            this.cardNumberLabel.TabIndex = 5;
            this.cardNumberLabel.Text = "Card Number:";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Font = new System.Drawing.Font("Arial", 10F);
            this.passwordLabel.Location = new System.Drawing.Point(40, 150);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(100, 23);
            this.passwordLabel.TabIndex = 6;
            this.passwordLabel.Text = "Password:";
            // 
            // amountLabel
            // 
            this.amountLabel.AutoSize = true;
            this.amountLabel.Font = new System.Drawing.Font("Arial", 10F);
            this.amountLabel.Location = new System.Drawing.Point(40, 210);
            this.amountLabel.Name = "amountLabel";
            this.amountLabel.Size = new System.Drawing.Size(81, 23);
            this.amountLabel.TabIndex = 7;
            this.amountLabel.Text = "Amount:";
            // 
            // Deposit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(500, 340);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.cardNumberLabel);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.amountLabel);
            this.Controls.Add(this.cardNumberTextBox);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.amountTextBox);
            this.Controls.Add(this.depositButton);
            this.Name = "Deposit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Deposit";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox cardNumberTextBox;
        private Guna.UI2.WinForms.Guna2TextBox passwordTextBox;
        private Guna.UI2.WinForms.Guna2TextBox amountTextBox;
        private Guna.UI2.WinForms.Guna2Button depositButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label cardNumberLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label amountLabel;
    }
} 