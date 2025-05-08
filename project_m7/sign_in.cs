using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace project_m7
{
    public partial class sign_in : Form
    {
        private DatabaseHelper dbHelper;

        public sign_in()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            SetupForm();
            guna2Button1.Click += guna2Button1_Click;
        }

        private void SetupForm()
        {
            // Set up text boxes
            guna2TextBox1.PlaceholderText = "Enter First Name";
            guna2TextBox2.PlaceholderText = "Enter Last Name";
            guna2TextBox5.PlaceholderText = "Enter Password";
            guna2TextBox3.PlaceholderText = "Confirm Password";
            
            // Set password character for password fields
            guna2TextBox5.PasswordChar = '●';
            guna2TextBox3.PasswordChar = '●';

            // Set up button
            guna2Button1.Text = "Create Account";

            // Set up link label
            linkLabel1.Text = "Back to Login";
            linkLabel1.Click += LinkLabel1_Click;

            // Set form title
            this.Text = "Create Account";
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Get user input
                string firstName = guna2TextBox1.Text.Trim();
                string lastName = guna2TextBox2.Text.Trim();
                string password = guna2TextBox5.Text;
                string confirmPassword = guna2TextBox3.Text;
                string phoneNumber = guna2TextBox4.Text.Trim();
                // Validate input
                if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(lastName) || 
                    string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
                {
                    MessageBox.Show("Please fill in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (password != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate phone number
               

                if (!checkBox1.Checked)
                {
                    MessageBox.Show("Please agree to the privacy policy", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Generate a random 16-digit card number
                Random random = new Random();
                string cardNumber = "";
                for (int i = 0; i < 16; i++)
                {
                    cardNumber += random.Next(0, 10).ToString();
                }

                // Register the user
                var result = dbHelper.RegisterUser(cardNumber, firstName, lastName, password, phoneNumber);

                if (result.Success)
                {
                    MessageBox.Show($"Registration successful!\nYour card number is: {cardNumber}\nPlease keep it safe.", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    login loginForm = new login();
                    loginForm.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LinkLabel1_Click(object sender, EventArgs e)
        {
            // Navigate back to the login form
            login loginForm = new login();
            loginForm.Show();
            this.Close();
        }

        // Event handler for the panel's Paint event
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int radius = 50;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = new GraphicsPath())
            {
                Rectangle bounds = ((Panel)sender).ClientRectangle;
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(bounds.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(bounds.Width - radius, bounds.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, bounds.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                using (Pen pen = new Pen(Color.Gray, 2))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        /// <summary>
        /// Validates if the provided phone number is in the correct format.
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate</param>
        /// <returns>True if the phone number is valid, false otherwise</returns>
      

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Create an instance of the start form and show it
            start start = new start();
            start.Show();
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
       
    }
}
