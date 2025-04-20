using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Guna.UI2.WinForms;

namespace project_m7
{
    public partial class Form3 : Form
    {
        private string cardNumber;
        private SqlConnection connection;
        private decimal currentBalance;

        public Form3(string cardNumber)
        {
            InitializeComponent();
            this.cardNumber = cardNumber;
            InitializeDatabase();
            LoadUserData();
            SetupButtons();
        }

        private void SetupButtons()
        {
            // Wire up button click events
            if (guna2Button1 != null) guna2Button1.Click += Guna2Button1_Click; // Deposit
            if (guna2Button2 != null) guna2Button2.Click += Guna2Button2_Click; // Withdraw
            if (guna2Button3 != null) guna2Button3.Click += Guna2Button3_Click; // Transfer
        }

        private void label5_Click(object sender, EventArgs e)
        {
            // Handle label5 click event - navigate back to login form
            //Form1 loginForm = new Form1();
            //loginForm.Show();
            //this.Close();
        }

        private void InitializeDatabase()
        {
            try
            {
                string connectionString = "Data Source=HASSAN;Initial Catalog=Bank_app;Integrated Security=True;TrustServerCertificate=True";
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserData()
        {
            try
            {
                string query = "SELECT balance FROM accounts WHERE cardnum = @cardnum";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cardnum", cardNumber);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Update balance
                            currentBalance = Convert.ToDecimal(reader["balance"]);
                            label9.Text = $"{currentBalance:N2}$";  // Display actual balance
                        }
                        else
                        {
                            MessageBox.Show("User data not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateBalance(decimal amount, bool isDeposit)
        {
            try
            {
                string query = "UPDATE accounts SET balance = balance + @amount WHERE cardnum = @cardnum";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    decimal adjustedAmount = isDeposit ? amount : -amount;
                    command.Parameters.AddWithValue("@amount", adjustedAmount);
                    command.Parameters.AddWithValue("@cardnum", cardNumber);
                    
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        currentBalance += adjustedAmount;
                        label9.Text = $"{currentBalance:N2}$";  // Display updated balance
                        string action = isDeposit ? "Deposit" : "Withdrawal";
                        MessageBox.Show($"{action} successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating balance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Guna2Button1_Click(object sender, EventArgs e)
        {
            // Deposit
            using (var inputForm = new Form())
            {
                inputForm.Text = "Deposit";
                inputForm.Size = new Size(300, 150);
                inputForm.StartPosition = FormStartPosition.CenterParent;

                var amountTextBox = new Guna2TextBox
                {
                    PlaceholderText = "Enter amount",
                    Location = new Point(50, 30),
                    Size = new Size(200, 30),
                    BorderRadius = 5
                };

                var okButton = new Guna2Button
                {
                    Text = "OK",
                    Location = new Point(100, 70),
                    Size = new Size(100, 30),
                    BorderRadius = 5,
                    FillColor = Color.DarkSlateBlue
                };

                okButton.Click += (s, args) =>
                {
                    if (decimal.TryParse(amountTextBox.Text, out decimal amount) && amount > 0)
                    {
                        UpdateBalance(amount, true);
                        inputForm.DialogResult = DialogResult.OK;
                        inputForm.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                inputForm.Controls.Add(amountTextBox);
                inputForm.Controls.Add(okButton);
                inputForm.ShowDialog();
            }
        }

        private void Guna2Button2_Click(object sender, EventArgs e)
        {
            // Withdraw
            using (var inputForm = new Form())
            {
                inputForm.Text = "Withdraw";
                inputForm.Size = new Size(300, 150);
                inputForm.StartPosition = FormStartPosition.CenterParent;

                var amountTextBox = new Guna2TextBox
                {
                    PlaceholderText = "Enter amount",
                    Location = new Point(50, 30),
                    Size = new Size(200, 30),
                    BorderRadius = 5
                };

                var okButton = new Guna2Button
                {
                    Text = "OK",
                    Location = new Point(100, 70),
                    Size = new Size(100, 30),
                    BorderRadius = 5,
                    FillColor = Color.DarkSlateBlue
                };

                okButton.Click += (s, args) =>
                {
                    if (decimal.TryParse(amountTextBox.Text, out decimal amount) && amount > 0)
                    {
                        if (amount <= currentBalance)
                        {
                            UpdateBalance(amount, false);
                            inputForm.DialogResult = DialogResult.OK;
                            inputForm.Close();
                        }
                        else
                        {
                            MessageBox.Show("Insufficient funds.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                inputForm.Controls.Add(amountTextBox);
                inputForm.Controls.Add(okButton);
                inputForm.ShowDialog();
            }
        }

        private void Guna2Button3_Click(object sender, EventArgs e)
        {
            // Transfer functionality can be implemented here
            MessageBox.Show("Transfer functionality coming soon!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (connection != null)
            {
                connection.Close();
            }
            Application.Exit();
        }
    }
}

