using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace project_m7
{
    public partial class Form1 : Form
    {
        private readonly DatabaseHelper dbHelper;

        public Form1()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            SetupForm();
            guna2Button1.Click += guna2Button1_Click;
        }

        private void SetupForm()
        {
            // Set up text boxes
            guna2TextBox1.PlaceholderText = "Enter Card Number";
            guna2TextBox2.PlaceholderText = "Enter Password";
            guna2TextBox2.PasswordChar = '●';

            // Set up button
            guna2Button1.Text = "Login";

            // Set up link labels
            linkLabel1.Text = "Sign up";
            linkLabel1.Click += LinkLabel1_Click;

            linkLabel2.Text = "Forget password";
            linkLabel2.Click += LinkLabel2_Click;

            // Set form title
            this.Text = "Bank Login";

            // Set up card number validation
            guna2TextBox1.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            // Limit card number to 16 digits
            guna2TextBox1.MaxLength = 16;
        }

        private void LinkLabel1_Click(object sender, EventArgs e)
        {
            Form2 registerForm = new Form2();
            registerForm.Show();
            this.Hide();
        }

        private void LinkLabel2_Click(object sender, EventArgs e)
        {
            PasswordResetForm resetForm = new PasswordResetForm();
            resetForm.Show();
        }
           
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string cardnum = guna2TextBox1.Text.Trim();
                string password = guna2TextBox2.Text.Trim();

                if (string.IsNullOrEmpty(cardnum))
                {
                    MessageBox.Show("Please enter your card number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    guna2TextBox1.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter your password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    guna2TextBox2.Focus();
                    return;
                }

                if (cardnum.Length != 16)
                {
                    MessageBox.Show("Card number must be exactly 16 digits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    guna2TextBox1.Focus();
                    return;
                }

                var loginResult = dbHelper.LoginUser(cardnum, password);
                if (loginResult.Success)
                {
                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form3 form3 = new Form3(cardnum);
                    form3.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show(loginResult.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (loginResult.Message.Contains("password"))
                    {
                        guna2TextBox2.Text = "";
                        guna2TextBox2.Focus();
                    }
                    else
                    {
                        guna2TextBox1.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void SetRoundedRegion(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            control.Region = new Region(path);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
