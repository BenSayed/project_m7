using System;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace project_m7
{
    public partial class TransferForm : Form
    {
        private readonly DatabaseHelper dbHelper;
        private readonly string sourceCardNumber;

        public TransferForm(string sourceCardNumber)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.sourceCardNumber = sourceCardNumber;
            SetupForm();
        }

        private void SetupForm()
        {
            // Set up text boxes
            cardNumberTextBox.PlaceholderText = "Enter Destination Card Number";
            passwordTextBox.PlaceholderText = "Enter Your Password";
            amountTextBox.PlaceholderText = "Enter Amount";
            
            // Set password character for password field
            passwordTextBox.PasswordChar = '●';

            // Set up button
            transferButton.Text = "Transfer";
            transferButton.Click += TransferButton_Click;

            // Set form title
            this.Text = "Transfer Money";

            // Set up card number validation
            cardNumberTextBox.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            // Limit card number to 16 digits
            cardNumberTextBox.MaxLength = 16;
            
            // Set up amount textbox to only accept numbers and decimal point
            amountTextBox.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                {
                    e.Handled = true;
                }

                // Only allow one decimal point
                if (e.KeyChar == '.' && (sender as Guna2TextBox).Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
            };
        }

        private void TransferButton_Click(object sender, EventArgs e)
        {
            string destinationCardNumber = cardNumberTextBox.Text.Trim();
            string password = passwordTextBox.Text.Trim();
            string amountText = amountTextBox.Text.Trim();

            if (string.IsNullOrEmpty(destinationCardNumber) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(amountText))
            {
                MessageBox.Show("Please fill in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (destinationCardNumber.Length != 16)
            {
                MessageBox.Show("Card number must be exactly 16 digits", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Verify source account password
            var loginResult = dbHelper.LoginUser(sourceCardNumber, password);
            if (!loginResult.Success)
            {
                MessageBox.Show("Invalid password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if destination account exists
            if (!dbHelper.VerifyCardNumber(destinationCardNumber))
            {
                MessageBox.Show("Destination account not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if source account has sufficient balance
            decimal sourceBalance = dbHelper.GetBalance(sourceCardNumber);
            if (amount > sourceBalance)
            {
                MessageBox.Show($"Insufficient funds. Your balance is {sourceBalance:C}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Perform transfer
            if (dbHelper.TransferMoney(sourceCardNumber, destinationCardNumber, amount))
            {
                MessageBox.Show($"Transfer successful! {amount:C} transferred to account {destinationCardNumber}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Transfer failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void transferButton_Click_1(object sender, EventArgs e)
        {

        }
    }
} 