using System;
using System.Windows.Forms;

namespace project_m7
{
    public partial class Deposit : Form
    {
        private readonly DatabaseHelper dbHelper;
       
        private string cardNum;

        public Deposit()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            SetupForm();
        }
        
        // Constructor that accepts a card number
        public Deposit(string cardNumber)
        {
            try
            {
                InitializeComponent();
                dbHelper = new DatabaseHelper();
                this.cardNum = cardNumber;
                
                if (cardNumberTextBox != null)
                {
                    cardNumberTextBox.Text = cardNumber;
                    cardNumberTextBox.Enabled = false; // Disable editing since we already have the card number
                }
                
                SetupForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Deposit constructor: {ex.Message}", "Error");
            }
        }

        private void SetupForm()
        {
            try
            {
                // Set button click handler
                if (depositButton != null)
                {
                    depositButton.Click += DepositButton_Click;
                }
                
                // Set up card number validation
                if (cardNumberTextBox != null)
                {
                    cardNumberTextBox.KeyPress += (sender, e) =>
                    {
                        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                        {
                            e.Handled = true;
                        }
                    };

                    // Limit card number to 16 digits
                    cardNumberTextBox.MaxLength = 16;
                }
                
                // Set up amount textbox to only accept numbers and decimal point
                if (amountTextBox != null)
                {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in SetupForm: {ex.Message}", "Error");
            }
        }

        private void DepositButton_Click(object sender, EventArgs e)
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

            var result = dbHelper.Deposit(cardNumber, amount/2);
            if (result)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to process deposit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Deposit_Load(object sender, EventArgs e)
        {

        }
    }
} 