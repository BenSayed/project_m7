using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace project_m7
{
    public partial class Form2 : Form
    {
        private DatabaseHelper dbHelper;

        public Form2()
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
                string password = guna2TextBox5.Text.Trim();
                string confirmPassword = guna2TextBox3.Text.Trim();

                if (string.IsNullOrEmpty(password))
                {
                    guna2TextBox5.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(confirmPassword))
                {
                    guna2TextBox3.Focus();
                    return;
                }

                if (password != confirmPassword)
                {
                    guna2TextBox5.Text = "";
                    guna2TextBox3.Text = "";
                    guna2TextBox5.Focus();
                    return;
                }

                // Generate random card number
                string cardNumber = GenerateRandomCardNumber();

                var registerResult = dbHelper.RegisterUser(cardNumber, password);
                if (registerResult.Success)
                {
                    MessageBox.Show($"Your card number is: {cardNumber}\nPlease save this number for future use.", 
                        "Card Number", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form1 form1 = new Form1();
                    form1.Show();
                    this.Hide();
                }
                else
                {
                    guna2TextBox5.Focus();
                }
            }
            catch (Exception ex)
            {
                // Handle error silently
            }
        }

        private void LinkLabel1_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }

        private string GenerateRandomCardNumber()
        {
            Random random = new Random();
            string cardNumber = "";
            for (int i = 0; i < 16; i++)
            {
                cardNumber += random.Next(0, 10).ToString();
            }
            return cardNumber;
        }
    }
}
