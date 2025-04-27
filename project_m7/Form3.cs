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
using project_m7;

namespace project_m7
{
    public partial class Form3 : Form
    {
        private string cardNumber;
        private SqlConnection connection;
        private decimal currentBalance;
        private DatabaseHelper dbHelper;
        

        public Form3(string cardNumber)
        {
            InitializeComponent();
            this.cardNumber = cardNumber;
            dbHelper = new DatabaseHelper();
            InitializeDatabase();
            LoadUserData();
            SetupButtons();
        }

        private void SetupButtons()
        {
            // Make sure buttons are enabled
            if (guna2Button1 != null) 
            {
                guna2Button1.Enabled = true;
                // Remove all handlers and add our deposit handler manually
                guna2Button1.Click -= new EventHandler(Guna2Button1_Click);
                guna2Button1.Click -= new EventHandler(guna2Button1_Click_1);
                guna2Button1.Click += new EventHandler(Guna2Button1_Click);
            }
            
            if (guna2Button2 != null) 
            {
                guna2Button2.Enabled = true;
                // Remove all handlers and add our withdrawal handler manually
                guna2Button2.Click -= new EventHandler(Guna2Button2_Click);
                guna2Button2.Click -= new EventHandler(guna2Button2_Click_1);
                guna2Button2.Click += new EventHandler(Guna2Button2_Click);
            }
            
            if (guna2Button3 != null) 
            {
                guna2Button3.Enabled = true;
                // Remove all handlers and add our transfer handler manually
                guna2Button3.Click -= new EventHandler(Guna2Button3_Click);
                guna2Button3.Click -= new EventHandler(guna2Button3_Click_1);
                guna2Button3.Click += new EventHandler(Guna2Button3_Click);
            }
            
          
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
                using (SqlConnection connection = new SqlConnection("Data Source=HASSAN;Initial Catalog=Bank_app;Integrated Security=True;TrustServerCertificate=True"))
                {
                    connection.Open();
                string query = "SELECT cardnum, balance FROM accounts WHERE cardnum = @cardnum";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cardnum", cardNumber);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                                // عرض رقم البطاقة
                                label6.Text = "Card Number: " + reader["cardnum"].ToString();

                                // تحديث الرصيد
                            currentBalance = Convert.ToDecimal(reader["balance"]);
                                label9.Text = $"{currentBalance:C}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       

        private void UpdateBalance(string cardNumber, decimal amount, bool isDeposit)
        {
            try
            {
                if (isDeposit)
                {
                    if (dbHelper.Deposit(cardNumber, amount))
                    {
                        currentBalance = dbHelper.GetBalance(cardNumber);
                        if (label9 != null)
                        {
                            label9.Text = $"{currentBalance:C}";
                        }
                        LoadUserData();



                    }
                }
                else
                {
                    if (dbHelper.Withdraw(cardNumber, amount))
                    {
                        currentBalance = dbHelper.GetBalance(cardNumber);
                        if (label9 != null)
                        {
                            label9.Text = $"{currentBalance:C}";
                        }
                        LoadUserData();
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
            try
            {
                Deposit depositForm = new Deposit(cardNumber);
                if (depositForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUserData(); // تحديث الرصيد من قاعدة البيانات
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening deposit form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                Withdral withdrawalForm = new Withdral(cardNumber);
                if (withdrawalForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUserData(); // تحديث الرصيد من قاعدة البيانات
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening withdrawal form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Guna2Button3_Click(object sender, EventArgs e)
        {
            string sourceCardNumber = cardNumber; // استخدام المتغير cardNumber مباشرة
            TransferForm transferForm = new TransferForm(sourceCardNumber);
            if (transferForm.ShowDialog() == DialogResult.OK)
            {
                LoadUserData();
            }
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

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            Guna2Button2_Click(sender, e);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // No action needed
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Log out action
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // No action needed
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // No action needed
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // No action needed
        }

        private void label6_Click(object sender, EventArgs e)
        {
            // No action needed
        }

        private void label7_Click(object sender, EventArgs e)
        {
            // No action needed
        }

        private void label8_Click(object sender, EventArgs e)
        {
            // No action needed
        }

        private void label9_Click(object sender, EventArgs e)
        {
            // No action needed
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            Guna2Button1_Click(sender, e);
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            Guna2Button3_Click(sender, e);
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            // Navigate back to login form
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // SetupTransactionLabels();
          
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            // إظهار نموذج تسجيل الدخول أولاً
            Form1 loginForm = new Form1();
            loginForm.Show();
            
            // إغلاق النموذج الحالي
            this.Hide();
        }
    }
}

