using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace project_m7
{
    public partial class Form2 : Form
    {
        private SqlConnection connection;

        public Form2()
        {
            InitializeComponent();
            InitializeDatabase();
            guna2Button1.Click += Guna2Button1_Click;
            linkLabel1.Click += LinkLabel1_Click;
            SetupPasswordFields();
            SetupInputValidation();
        }

        private void SetupPasswordFields()
        {
            // Set password char for password fields
            guna2TextBox5.PasswordChar = '●';
            guna2TextBox3.PasswordChar = '●';
        }

        private void SetupInputValidation()
        {
            // Allow only numbers in card number field
            guna2TextBox6.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            // Allow only numbers in phone number field
            guna2TextBox7.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
        }

        private bool ValidateInputs()
        {
            // Check if any field is empty
            if (string.IsNullOrEmpty(guna2TextBox1.Text) || string.IsNullOrEmpty(guna2TextBox2.Text) ||
                string.IsNullOrEmpty(guna2TextBox7.Text) || string.IsNullOrEmpty(guna2TextBox6.Text) ||
                string.IsNullOrEmpty(guna2TextBox4.Text) || string.IsNullOrEmpty(guna2TextBox5.Text) ||
                string.IsNullOrEmpty(guna2TextBox3.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate card number (must be exactly 16 digits)
            if (!Regex.IsMatch(guna2TextBox6.Text.Trim(), @"^\d{16}$"))
            {
                MessageBox.Show("Card number must be exactly 16 digits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                guna2TextBox6.Focus();
                return false;
            }

            // Validate phone number (must be 11 digits and start with 01)
            if (!Regex.IsMatch(guna2TextBox7.Text.Trim(), @"^01\d{9}$"))
            {
                MessageBox.Show("Phone number must be 11 digits and start with 01.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                guna2TextBox7.Focus();
                return false;
            }

            // Validate email format
            //if (!Regex.IsMatch(guna2TextBox4.Text.Trim(), @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            //{
            //    MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    guna2TextBox4.Focus();
            //    return false;
            //}

            // Check if passwords match
            if (guna2TextBox5.Text != guna2TextBox3.Text)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                guna2TextBox5.Focus();
                return false;
            }

            // Check privacy policy agreement
            if (!checkBox1.Checked)
            {
                MessageBox.Show("Please agree to the privacy policy.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
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

        private void LinkLabel1_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        private void Guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate all inputs first
                if (!ValidateInputs())
                {
                    return;
                }

                // First insert into accounts table
                string accountQuery = @"
                    INSERT INTO accounts (firstname, lastname, cardnum, password)
                    VALUES (@FirstName, @LastName, @CardNum, @Password)";

                using (SqlCommand command = new SqlCommand(accountQuery, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", guna2TextBox1.Text.Trim());
                    command.Parameters.AddWithValue("@LastName", guna2TextBox2.Text.Trim());
                    command.Parameters.AddWithValue("@CardNum", guna2TextBox6.Text.Trim());
                    command.Parameters.AddWithValue("@Password", guna2TextBox5.Text.Trim());

                    command.ExecuteNonQuery();
                }

                // Then insert into user_details table with default balance of 0
                string detailsQuery = @"
                    INSERT INTO user_details (FirstName, SecondName, PhoneNumber, CardNumber, Email, Balance)
                    VALUES (@FirstName, @SecondName, @PhoneNumber, @CardNumber, @Email, 0)";

                using (SqlCommand command = new SqlCommand(detailsQuery, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", guna2TextBox1.Text.Trim());
                    command.Parameters.AddWithValue("@SecondName", guna2TextBox2.Text.Trim());
                    command.Parameters.AddWithValue("@PhoneNumber", guna2TextBox7.Text.Trim());
                    command.Parameters.AddWithValue("@CardNumber", guna2TextBox6.Text.Trim());
                    command.Parameters.AddWithValue("@Email", guna2TextBox4.Text.Trim());

                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Clear form
                guna2TextBox1.Text = "";
                guna2TextBox2.Text = "";
                guna2TextBox7.Text = "";
                guna2TextBox6.Text = "";
                guna2TextBox4.Text = "";
                guna2TextBox5.Text = "";
                guna2TextBox3.Text = "";
                checkBox1.Checked = false;

                // Return to login form
                Form1 loginForm = new Form1();
                loginForm.Show();
                this.Hide();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Unique constraint violation
                {
                    MessageBox.Show("This card number is already registered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {
        }

        private void guna2TextBox6_TextChanged(object sender, EventArgs e)
        {
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
