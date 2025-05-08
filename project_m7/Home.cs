using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_m7
{
    public partial class Home : Form
    {
        private readonly DatabaseHelper dbHelper;
        private string CardNum;

        public Home(string CardNum)
        {
            InitializeComponent();
            this.CardNum = CardNum;
            dbHelper = new DatabaseHelper();
            LoadUserData();
        }
        private void LoadUserData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=HASSAN;Initial Catalog=Bank_app;Integrated Security=True;TrustServerCertificate=True"))
                {
                    connection.Open();
                    string query = "SELECT cardnum, balance, firstname, lastname FROM accounts WHERE cardnum = @cardnum";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cardnum", CardNum);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Display card number
                                label5.Text =  reader["cardnum"].ToString();
                                // Display balance
                                label3.Text =  reader["firstname"].ToString()+" "+ reader["lastname"].ToString();
                                label7.Text = reader["balance"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No account found with this card number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            including_banking including_Banking = new including_banking(CardNum);
            including_Banking.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        

        //private void guna2Button6_Click(object sender, EventArgs e)
        //{
        //    // Open the Settings form and pass the card number
        //    Sittings settingsForm = new Sittings(CardNum);
        //    settingsForm.Show();
        //    this.Hide();
        //}

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Logout functionality
          
                login loginForm = new login();
                loginForm.Show();
            this.Close();
           
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Donate d = new Donate(CardNum);
            d.Show();
            
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }
    }
}
