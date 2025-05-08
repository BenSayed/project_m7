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
    public partial class history : Form
    {
        private readonly string connectionString = "Data Source=HASSAN;Initial Catalog=Bank_app;Integrated Security=True;TrustServerCertificate=True";
        private string cardNumber;
        
        public history()
        {
            InitializeComponent();
            // Default constructor will not load transactions until a card number is provided
            titleLabel.Text = "Transaction History";
        }
        
        public history(string cardNumber) : this()
        {
            this.cardNumber = cardNumber;
        }
        
        private void history_Load(object sender, EventArgs e)
        {
            LoadTransactionHistory();
        }
        
        private void LoadTransactionHistory()
        {
            try
            {
                // Check if cardNumber is null or empty
                if (string.IsNullOrEmpty(cardNumber))
                {
                    MessageBox.Show("Card number is not provided. Cannot load transaction history.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // Get account holder name for the title
                    string nameQuery = "SELECT CONCAT(firstname, ' ', lastname) as fullname FROM accounts WHERE cardnum = @cardnum";
                    using (SqlCommand nameCommand = new SqlCommand(nameQuery, connection))
                    {
                        nameCommand.Parameters.AddWithValue("@cardnum", cardNumber);
                        object nameResult = nameCommand.ExecuteScalar();
                        if (nameResult != null)
                        {
                            titleLabel.Text = $"Transaction History for {nameResult}";
                        }
                    }
                    
                    // Get transaction history
                    string query = @"
                        SELECT 
                            transaction_id as 'ID',
                            transaction_type as 'Type',
                            amount as 'Amount',
                            transaction_date as 'Date',
                            balance_after as 'Balance After',
                            destination_cardnum as 'Destination',
                            description as 'Description'
                        FROM transactions 
                        WHERE cardnum = @cardnum
                        ORDER BY transaction_date DESC";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cardnum", cardNumber);
                        
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        transactionsGridView.DataSource = dataTable;
                        
                        // Format columns
                        if (transactionsGridView.Columns.Contains("Amount"))
                        {
                            transactionsGridView.Columns["Amount"].DefaultCellStyle.Format = "C2";
                        }
                        
                        if (transactionsGridView.Columns.Contains("Balance After"))
                        {
                            transactionsGridView.Columns["Balance After"].DefaultCellStyle.Format = "C2";
                        }
                        
                        if (transactionsGridView.Columns.Contains("Date"))
                        {
                            transactionsGridView.Columns["Date"].DefaultCellStyle.Format = "g";
                        }
                        
                        // Hide ID column
                        if (transactionsGridView.Columns.Contains("ID"))
                        {
                            transactionsGridView.Columns["ID"].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading transaction history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnBack_Click(object sender, EventArgs e)
        {
            including_banking inc = new including_banking(cardNumber);
            inc.Show();
            this.Close();
        }
    }
}
