using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_m7
{
    public partial class PasswordResetForm : Form
    {
        private readonly DatabaseHelper dbHelper;

        public PasswordResetForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            SetupForm();
        }

        private void SetupForm()
        {
            // Set up text boxes
            guna2TextBox1.PlaceholderText = "Enter Card Number";
            guna2TextBox2.PlaceholderText = "Enter New Password";
            guna2TextBox3.PlaceholderText = "Confirm New Password";
            
            // Set password character for password fields
            guna2TextBox2.PasswordChar = '●';
            guna2TextBox3.PasswordChar = '●';

            // Set up button
            guna2Button1.Text = "Reset Password";
            guna2Button1.Click += guna2Button1_Click;

            // Set form title
            this.Text = "Reset Password";

            // Set up card number validation
            guna2TextBox1.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            // Limit card number to 16 digits
            guna2TextBox1.MaxLength = 16;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string cardnum = guna2TextBox1.Text.Trim();
                string newPassword = guna2TextBox2.Text.Trim();
                string confirmPassword = guna2TextBox3.Text.Trim();

                if (string.IsNullOrEmpty(cardnum))
                {
                    MessageBox.Show("Please enter your card number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    guna2TextBox1.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(newPassword))
                {
                    MessageBox.Show("Please enter your new password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    guna2TextBox2.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(confirmPassword))
                {
                    MessageBox.Show("Please confirm your new password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    guna2TextBox3.Focus();
                    return;
                }

                if (newPassword != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    guna2TextBox2.Text = "";
                    guna2TextBox3.Text = "";
                    guna2TextBox2.Focus();
                    return;
                }

                if (cardnum.Length != 16)
                {
                    MessageBox.Show("Card number must be exactly 16 digits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    guna2TextBox1.Focus();
                    return;
                }

                if (!dbHelper.VerifyCardNumber(cardnum))
                {
                    MessageBox.Show("Card number not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    guna2TextBox1.Focus();
                    return;
                }

                if (dbHelper.UpdatePassword(cardnum, newPassword))
                {
                    MessageBox.Show("Password reset successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to reset password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
