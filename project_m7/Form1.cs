using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace project_m7
{
    public partial class Form1 : Form
    {
        private SqlConnection connection;

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
            SetupForm();
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
        }

        private void LinkLabel1_Click(object sender, EventArgs e)
        {
            Form2 registerForm = new Form2();
            registerForm.Show();
            this.Hide();
        }

        private void LinkLabel2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please contact bank support to reset your password.", "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
           
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string cardNumber = guna2TextBox1.Text.Trim();
                string password = guna2TextBox2.Text.Trim();

                if (string.IsNullOrEmpty(cardNumber))
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

                // Check credentials
                string query = "SELECT password FROM accounts WHERE cardnum = @cardnum";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cardnum", cardNumber);
                    object result = command.ExecuteScalar();

                    if (result != null && result.ToString() == password)
                    {
                        MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form3 form3 = new Form3(cardNumber);
                        form3.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid card number or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        guna2TextBox2.Text = ""; // Clear password field
                        guna2TextBox2.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
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

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
        }

        private void label2_Click_1(object sender, EventArgs e)
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
