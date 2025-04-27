using System;
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
            guna2Button1.Click += ResetButton_Click;

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

        private void ResetButton_Click(object sender, EventArgs e)
        {
            string cardNumber = guna2TextBox1.Text.Trim();
            string newPassword = guna2TextBox2.Text.Trim();
            string confirmPassword = guna2TextBox3.Text.Trim();

            if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cardNumber.Length != 16)
            {
                MessageBox.Show("Card number must be exactly 16 digits", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = dbHelper.UpdatePassword(cardNumber, "", newPassword);
            if (result.Success)
            {
                MessageBox.Show(result.Message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
