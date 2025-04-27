using System;
using System.Windows.Forms;

namespace project_m7
{
    public partial class Withdral : Form
    {
        private readonly DatabaseHelper dbHelper;
        private decimal currentBalance;
        private string cardNum;

        public Withdral()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            SetupForm();
        }
        
        // Constructor that accepts a card number
        public Withdral(string cardNumber)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            this.cardNum = cardNumber;
            cardNumberTextBox.Text = cardNumber;
            cardNumberTextBox.Enabled = false; // Disable editing since we already have the card number
            SetupForm();
        }

        private void SetupForm()
        {
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
                if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
            };
        }

        private void WithdrawButton_Click(object sender, EventArgs e)
        {
            string cardNumber = cardNumberTextBox.Text.Trim();
            string password = passwordTextBox.Text.Trim();
            string amountText = amountTextBox.Text.Trim();

            if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(amountText))
            {
                MessageBox.Show("Please fill in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = dbHelper.Withdraw(cardNumber, amount);
            if (result)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to process withdrawal", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 