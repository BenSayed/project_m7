using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_m7
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
           
        }

        private void ValidateAndDisableAccount()
        {
            try
            {
                string name = guna2TextBox1.Text.Trim();
                string phoneNumber = guna2TextBox2.Text.Trim();
                string password = guna2TextBox3.Text.Trim();
                
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please fill in all fields", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                using (SqlConnection connection = new SqlConnection("Data Source=HASSAN;Initial Catalog=Bank_app;Integrated Security=True;TrustServerCertificate=True"))
                {
                    connection.Open();
                    
                    // Check if name matches by concatenating firstname and lastname
                    string nameQuery = "SELECT COUNT(*) FROM accounts WHERE CONCAT(firstname, ' ', lastname) = @fullName";
                    bool nameMatches = false;
                    
                    using (SqlCommand nameCommand = new SqlCommand(nameQuery, connection))
                    {
                        nameCommand.Parameters.AddWithValue("@fullName", name);
                        int nameCount = Convert.ToInt32(nameCommand.ExecuteScalar());
                        nameMatches = (nameCount > 0);
                        
                        if (!nameMatches)
                        {
                            MessageBox.Show("Name not found in our records", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    
                    // Check if phone number matches
                    string phoneQuery = "SELECT COUNT(*) FROM accounts WHERE phone_number = @phoneNumber";
                    bool phoneMatches = false;
                    
                    using (SqlCommand phoneCommand = new SqlCommand(phoneQuery, connection))
                    {
                        phoneCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        int phoneCount = Convert.ToInt32(phoneCommand.ExecuteScalar());
                        phoneMatches = (phoneCount > 0);
                        
                        if (!phoneMatches)
                        {
                            MessageBox.Show("Phone number not found in our records", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    
                    // Check if password matches
                    string passwordQuery = "SELECT cardnum FROM accounts WHERE CONCAT(firstname, ' ', lastname) = @fullName AND phone_number = @phoneNumber AND password = @password";
                    string cardNumber = null;
                    
                    using (SqlCommand passwordCommand = new SqlCommand(passwordQuery, connection))
                    {
                        passwordCommand.Parameters.AddWithValue("@fullName", name);
                        passwordCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        passwordCommand.Parameters.AddWithValue("@password", password);
                        
                        object result = passwordCommand.ExecuteScalar();
                        if (result != null)
                        {
                            cardNumber = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Invalid password", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    
                    // Disable the account by setting is_enabled = 0
                    string disableQuery = "UPDATE accounts SET is_enabled = 0 WHERE cardnum = @cardNum";
                    
                    using (SqlCommand disableCommand = new SqlCommand(disableQuery, connection))
                    {
                        disableCommand.Parameters.AddWithValue("@cardNum", cardNumber);
                        int rowsAffected = disableCommand.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Account has been disabled successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to disable account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing request: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ValidateAndDisableAccount();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Report_Load(object sender, EventArgs e)
        {

        }

    }
}
