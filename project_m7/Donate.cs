using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_m7
{
    public partial class Donate : Form
    {
        private DatabaseHelper dbHelper;
        string cardNumber;
        public Donate(string card)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            cardNumber = card;
            // Set up event handlers
            btnDonate.Click += BtnDonate_Click;
            btnBack.Click += BtnBack_Click;
        }

       

        private void BtnDonate_Click(object sender, EventArgs e)
        {
            try
            {
                // Get user input
                cardNumber = txtCardNumber.Text.Trim();
                string password = txtPassword.Text;
                
                // Validate amount input
                if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
                {
                    MessageBox.Show("Please enter a valid donation amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Process the donation
                var result = dbHelper.Donate(cardNumber, password, amount);

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "Donation Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Clear fields after successful donation
                    txtCardNumber.Clear();
                    txtPassword.Clear();
                    txtAmount.Clear();
                }
                else
                {
                    MessageBox.Show(result.Message, "Donation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnBack_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
    }
}
