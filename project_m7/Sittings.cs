using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace project_m7
{
    public partial class Sittings : Form
    {
        private readonly DatabaseHelper dbHelper;
        private string cardNumber;

        public Sittings()
        {
            InitializeComponent();
            this.Load += new EventHandler(Sittings_Load);
        }

        public Sittings(string cardNumber) : this()
        {
            this.cardNumber = cardNumber;
            dbHelper = new DatabaseHelper();

            // Check if account is enabled or disabled and update button text accordingly
            using (var connection = new SqlConnection(
                "Data Source=HASSAN;Initial Catalog=Bank_app;Integrated Security=True;TrustServerCertificate=True"))
            {
                connection.Open();
                string checkQuery = "SELECT is_enabled FROM accounts WHERE cardnum = @cardnum";
                using (var checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@cardnum", cardNumber);
                    object result = checkCommand.ExecuteScalar();
                    if (result != null)
                    {
                        bool isEnabled = Convert.ToBoolean(result);
                        btnDisableAccount.Text = isEnabled ? "Disable Account" : "Enable Account";
                    }
                }
            }
        }

        private void btnDisableAccount_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text.Trim();
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your password.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dbHelper.VerifyPassword(cardNumber, password))
            {
                // Check current account status
                bool isCurrentlyEnabled = false;
                using (var connection = new SqlConnection(
                    "Data Source=HASSAN;Initial Catalog=Bank_app;Integrated Security=True;TrustServerCertificate=True"))
                {
                    connection.Open();
                    string checkQuery = "SELECT is_enabled FROM accounts WHERE cardnum = @cardnum";
                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@cardnum", cardNumber);
                        object result = checkCommand.ExecuteScalar();
                        if (result != null)
                        {
                            isCurrentlyEnabled = Convert.ToBoolean(result);
                        }
                    }

                    // Toggle the account status (enable if disabled, disable if enabled)
                    int newStatus = isCurrentlyEnabled ? 0 : 1; // 0 = disabled, 1 = enabled
                    string updateQuery = "UPDATE accounts SET is_enabled = @newStatus WHERE cardnum = @cardnum";
                    
                    using (var updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@newStatus", newStatus);
                        updateCommand.Parameters.AddWithValue("@cardnum", cardNumber);
                        int rowsAffected = updateCommand.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            string statusMessage = newStatus == 1 ? 
                                "Your account has been enabled successfully." : 
                                "Your account has been disabled successfully.";
                                 
                            string statusTitle = newStatus == 1 ? "Account Enabled" : "Account Disabled";
                             
                            MessageBox.Show(statusMessage, statusTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                             
                           
                            this.Close();
                        }
                        else
                        {
                            string actionText = newStatus == 1 ? "enable" : "disable";
                            MessageBox.Show($"Failed to {actionText} account. Please try again.", 
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Incorrect password. Please try again.", 
                    "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text.Trim();
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your password.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dbHelper.VerifyPassword(cardNumber, password))
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete your account? This action cannot be undone.", 
                    "Confirm Deletion", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (dbHelper.DeleteAccount(cardNumber))
                    {
                        MessageBox.Show("Your account has been deleted successfully.", 
                            "Account Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Return to login screen
                        start start = new start();
                        start.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete account. Please try again.", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Incorrect password. Please try again.", 
                    "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // Don't exit application here, just close this form
        }

        private void titleLabel_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void Sittings_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return;
                
            // Check if account is enabled or disabled and update button text accordingly
            using (var connection = new SqlConnection(
                "Data Source=HASSAN;Initial Catalog=Bank_app;Integrated Security=True;TrustServerCertificate=True"))
            {
                connection.Open();
                string checkQuery = "SELECT is_enabled FROM accounts WHERE cardnum = @cardnum";
                using (var checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@cardnum", cardNumber);
                    object result = checkCommand.ExecuteScalar();
                    if (result != null)
                    {
                        bool isEnabled = Convert.ToBoolean(result);
                        btnDisableAccount.Text = isEnabled ? "Disable Account" : "Enable Account";
                    }
                }
            }
        }
    }
}
