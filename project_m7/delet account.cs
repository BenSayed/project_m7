using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace project_m7
{
    public partial class delet_account : Form
    {
        private string cardNumber;
        private DatabaseHelper dbHelper;

        public delet_account(string cardNumber)
        {
            InitializeComponent();
            this.cardNumber = cardNumber;
            this.dbHelper = new DatabaseHelper();
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            try
            {
                // Check password
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Please enter your password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Verify password
                if (!dbHelper.VerifyPassword(cardNumber, txtPassword.Text))
                {
                    MessageBox.Show("Incorrect password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Confirm deletion
                DialogResult result = MessageBox.Show("Are you sure you want to delete your account?", "Confirm Deletion", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Delete account from database
                    if (dbHelper.DeleteAccount(cardNumber))
                    {
                        MessageBox.Show("Account deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        login loginForm = new login();
                        loginForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDisableCard_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter your password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to disable your card?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dbHelper.ToggleCardStatus(cardNumber, txtPassword.Text))
                {
                    MessageBox.Show("Card disabled successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to disable card. Please check your password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEnableCard_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter your password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to enable your card?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dbHelper.ToggleCardStatus(cardNumber, txtPassword.Text))
                {
                    MessageBox.Show("Card enabled successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to enable card. Please check your password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void delet_account_Load(object sender, EventArgs e)
        {
            bool isEnabled = dbHelper.IsCardEnabled(cardNumber);
            btnDisableCard.Visible = isEnabled;
            btnEnableCard.Visible = !isEnabled;
        }
    }
}
